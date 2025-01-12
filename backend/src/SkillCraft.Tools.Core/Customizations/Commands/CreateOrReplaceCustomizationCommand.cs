using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.Core.Customizations.Validators;

namespace SkillCraft.Tools.Core.Customizations.Commands;

public record CreateOrReplaceCustomizationResult(CustomizationModel? Customization = null, bool Created = false);

public record CreateOrReplaceCustomizationCommand(Guid? Id, CreateOrReplaceCustomizationPayload Payload, long? Version) : IRequest<CreateOrReplaceCustomizationResult>;

internal class CreateOrReplaceCustomizationCommandHandler : IRequestHandler<CreateOrReplaceCustomizationCommand, CreateOrReplaceCustomizationResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ICustomizationManager _customizationManager;
  private readonly ICustomizationQuerier _customizationQuerier;
  private readonly ICustomizationRepository _customizationRepository;

  public CreateOrReplaceCustomizationCommandHandler(
    IApplicationContext applicationContext,
    ICustomizationManager customizationManager,
    ICustomizationQuerier customizationQuerier,
    ICustomizationRepository customizationRepository)
  {
    _applicationContext = applicationContext;
    _customizationManager = customizationManager;
    _customizationQuerier = customizationQuerier;
    _customizationRepository = customizationRepository;
  }

  public async Task<CreateOrReplaceCustomizationResult> Handle(CreateOrReplaceCustomizationCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceCustomizationPayload payload = command.Payload;
    new CreateOrReplaceCustomizationValidator().ValidateAndThrow(payload);

    CustomizationId? customizationId = null;
    Customization? customization = null;
    if (command.Id.HasValue)
    {
      customizationId = new(command.Id.Value);
      customization = await _customizationRepository.LoadAsync(customizationId.Value, cancellationToken);
    }

    Slug uniqueSlug = new(payload.UniqueSlug);
    ActorId? actorId = _applicationContext.ActorId;
    bool created = false;
    if (customization == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceCustomizationResult();
      }

      customization = new(payload.Type, uniqueSlug, actorId, customizationId);
      created = true;
    }

    Customization reference = (command.Version.HasValue
      ? await _customizationRepository.LoadAsync(customization.Id, command.Version.Value, cancellationToken)
      : null) ?? customization;

    if (reference.UniqueSlug != uniqueSlug)
    {
      customization.UniqueSlug = uniqueSlug;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      customization.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      customization.Description = description;
    }

    customization.Update(actorId);

    await _customizationManager.SaveAsync(customization, cancellationToken);

    CustomizationModel model = await _customizationQuerier.ReadAsync(customization, cancellationToken);
    return new CreateOrReplaceCustomizationResult(model, created);
  }
}
