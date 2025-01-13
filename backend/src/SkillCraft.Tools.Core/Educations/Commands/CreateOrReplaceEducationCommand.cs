using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.Core.Educations.Validators;

namespace SkillCraft.Tools.Core.Educations.Commands;

public record CreateOrReplaceEducationResult(EducationModel? Education = null, bool Created = false);

public record CreateOrReplaceEducationCommand(Guid? Id, CreateOrReplaceEducationPayload Payload, long? Version) : IRequest<CreateOrReplaceEducationResult>;

internal class CreateOrReplaceEducationCommandHandler : IRequestHandler<CreateOrReplaceEducationCommand, CreateOrReplaceEducationResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ICustomizationRepository _customizationRepository;
  private readonly IEducationManager _educationManager;
  private readonly IEducationQuerier _educationQuerier;
  private readonly IEducationRepository _educationRepository;

  public CreateOrReplaceEducationCommandHandler(
    IApplicationContext applicationContext,
    ICustomizationRepository customizationRepository,
    IEducationManager educationManager,
    IEducationQuerier educationQuerier,
    IEducationRepository educationRepository)
  {
    _applicationContext = applicationContext;
    _customizationRepository = customizationRepository;
    _educationManager = educationManager;
    _educationQuerier = educationQuerier;
    _educationRepository = educationRepository;
  }

  public async Task<CreateOrReplaceEducationResult> Handle(CreateOrReplaceEducationCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceEducationPayload payload = command.Payload;
    new CreateOrReplaceEducationValidator().ValidateAndThrow(payload);

    EducationId? educationId = null;
    Education? education = null;
    if (command.Id.HasValue)
    {
      educationId = new(command.Id.Value);
      education = await _educationRepository.LoadAsync(educationId.Value, cancellationToken);
    }

    Slug uniqueSlug = new(payload.UniqueSlug);
    ActorId? actorId = _applicationContext.ActorId;
    bool created = false;
    if (education == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceEducationResult();
      }

      education = new(uniqueSlug, actorId, educationId);
      created = true;
    }

    Education reference = (command.Version.HasValue
      ? await _educationRepository.LoadAsync(education.Id, command.Version.Value, cancellationToken)
      : null) ?? education;

    if (reference.UniqueSlug != uniqueSlug)
    {
      education.UniqueSlug = uniqueSlug;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      education.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      education.Description = description;
    }

    if (reference.Skill != payload.Skill)
    {
      education.Skill = payload.Skill;
    }
    WealthMultiplier? wealthMultiplier = payload.WealthMultiplier.HasValue ? new(payload.WealthMultiplier.Value) : null;
    if (reference.WealthMultiplier != wealthMultiplier)
    {
      education.WealthMultiplier = wealthMultiplier;
    }

    education.Update(actorId);

    await _educationManager.SaveAsync(education, cancellationToken);

    EducationModel model = await _educationQuerier.ReadAsync(education, cancellationToken);
    return new CreateOrReplaceEducationResult(model, created);
  }
}
