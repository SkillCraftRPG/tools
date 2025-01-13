using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Core.Aspects.Validators;
using SkillCraft.Tools.Core.Customizations;

namespace SkillCraft.Tools.Core.Aspects.Commands;

public record CreateOrReplaceAspectResult(AspectModel? Aspect = null, bool Created = false);

public record CreateOrReplaceAspectCommand(Guid? Id, CreateOrReplaceAspectPayload Payload, long? Version) : IRequest<CreateOrReplaceAspectResult>;

internal class CreateOrReplaceAspectCommandHandler : IRequestHandler<CreateOrReplaceAspectCommand, CreateOrReplaceAspectResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ICustomizationRepository _customizationRepository;
  private readonly IAspectManager _aspectManager;
  private readonly IAspectQuerier _aspectQuerier;
  private readonly IAspectRepository _aspectRepository;

  public CreateOrReplaceAspectCommandHandler(
    IApplicationContext applicationContext,
    ICustomizationRepository customizationRepository,
    IAspectManager aspectManager,
    IAspectQuerier aspectQuerier,
    IAspectRepository aspectRepository)
  {
    _applicationContext = applicationContext;
    _customizationRepository = customizationRepository;
    _aspectManager = aspectManager;
    _aspectQuerier = aspectQuerier;
    _aspectRepository = aspectRepository;
  }

  public async Task<CreateOrReplaceAspectResult> Handle(CreateOrReplaceAspectCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceAspectPayload payload = command.Payload;
    new CreateOrReplaceAspectValidator().ValidateAndThrow(payload);

    AspectId? aspectId = null;
    Aspect? aspect = null;
    if (command.Id.HasValue)
    {
      aspectId = new(command.Id.Value);
      aspect = await _aspectRepository.LoadAsync(aspectId.Value, cancellationToken);
    }

    Slug uniqueSlug = new(payload.UniqueSlug);
    ActorId? actorId = _applicationContext.ActorId;
    bool created = false;
    if (aspect == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceAspectResult();
      }

      aspect = new(uniqueSlug, actorId, aspectId);
      created = true;
    }

    Aspect reference = (command.Version.HasValue
      ? await _aspectRepository.LoadAsync(aspect.Id, command.Version.Value, cancellationToken)
      : null) ?? aspect;

    if (reference.UniqueSlug != uniqueSlug)
    {
      aspect.UniqueSlug = uniqueSlug;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      aspect.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      aspect.Description = description;
    }

    AttributeSelection attributes = new(payload.Attributes);
    if (reference.Attributes != attributes)
    {
      aspect.Attributes = attributes;
    }
    SkillSelection skills = new(payload.Skills);
    if (reference.Skills != skills)
    {
      aspect.Skills = skills;
    }

    aspect.Update(actorId);

    await _aspectManager.SaveAsync(aspect, cancellationToken);

    AspectModel model = await _aspectQuerier.ReadAsync(aspect, cancellationToken);
    return new CreateOrReplaceAspectResult(model, created);
  }
}
