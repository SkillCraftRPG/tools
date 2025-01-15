using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.Core.Natures.Validators;

namespace SkillCraft.Tools.Core.Natures.Commands;

public record CreateOrReplaceNatureResult(NatureModel? Nature = null, bool Created = false);

public record CreateOrReplaceNatureCommand(Guid? Id, CreateOrReplaceNaturePayload Payload, long? Version) : Activity, IRequest<CreateOrReplaceNatureResult>;

internal class CreateOrReplaceNatureCommandHandler : IRequestHandler<CreateOrReplaceNatureCommand, CreateOrReplaceNatureResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ICustomizationRepository _customizationRepository;
  private readonly INatureManager _natureManager;
  private readonly INatureQuerier _natureQuerier;
  private readonly INatureRepository _natureRepository;

  public CreateOrReplaceNatureCommandHandler(
    IApplicationContext applicationContext,
    ICustomizationRepository customizationRepository,
    INatureManager natureManager,
    INatureQuerier natureQuerier,
    INatureRepository natureRepository)
  {
    _applicationContext = applicationContext;
    _customizationRepository = customizationRepository;
    _natureManager = natureManager;
    _natureQuerier = natureQuerier;
    _natureRepository = natureRepository;
  }

  public async Task<CreateOrReplaceNatureResult> Handle(CreateOrReplaceNatureCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceNaturePayload payload = command.Payload;
    new CreateOrReplaceNatureValidator().ValidateAndThrow(payload);

    NatureId? natureId = null;
    Nature? nature = null;
    if (command.Id.HasValue)
    {
      natureId = new(command.Id.Value);
      nature = await _natureRepository.LoadAsync(natureId.Value, cancellationToken);
    }

    Slug uniqueSlug = new(payload.UniqueSlug);
    ActorId? actorId = _applicationContext.ActorId;
    bool created = false;
    if (nature == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceNatureResult();
      }

      nature = new(uniqueSlug, actorId, natureId);
      created = true;
    }

    Nature reference = (command.Version.HasValue
      ? await _natureRepository.LoadAsync(nature.Id, command.Version.Value, cancellationToken)
      : null) ?? nature;

    if (reference.UniqueSlug != uniqueSlug)
    {
      nature.UniqueSlug = uniqueSlug;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      nature.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      nature.Description = description;
    }

    if (reference.Attribute != payload.Attribute)
    {
      nature.Attribute = payload.Attribute;
    }

    CustomizationId? giftId = payload.GiftId.HasValue ? new(payload.GiftId.Value) : null;
    if (reference.GiftId != giftId)
    {
      Customization? gift = null;
      if (giftId.HasValue)
      {
        gift = await _customizationRepository.LoadAsync(giftId.Value, cancellationToken)
          ?? throw new CustomizationNotFoundException(giftId.Value, nameof(payload.GiftId));
      }
      nature.SetGift(gift);
    }

    nature.Update(actorId);

    await _natureManager.SaveAsync(nature, cancellationToken);

    NatureModel model = await _natureQuerier.ReadAsync(nature, cancellationToken);
    return new CreateOrReplaceNatureResult(model, created);
  }
}
