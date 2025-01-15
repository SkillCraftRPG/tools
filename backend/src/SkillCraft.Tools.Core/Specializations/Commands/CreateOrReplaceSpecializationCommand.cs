using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Core.Specializations.Validators;
using SkillCraft.Tools.Core.Talents;

namespace SkillCraft.Tools.Core.Specializations.Commands;

public record CreateOrReplaceSpecializationResult(SpecializationModel? Specialization = null, bool Created = false);

public record CreateOrReplaceSpecializationCommand(Guid? Id, CreateOrReplaceSpecializationPayload Payload, long? Version) : Activity, IRequest<CreateOrReplaceSpecializationResult>;

internal class CreateOrReplaceSpecializationCommandHandler : IRequestHandler<CreateOrReplaceSpecializationCommand, CreateOrReplaceSpecializationResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ISpecializationManager _specializationManager;
  private readonly ISpecializationQuerier _specializationQuerier;
  private readonly ISpecializationRepository _specializationRepository;
  private readonly ITalentRepository _talentRepository;

  public CreateOrReplaceSpecializationCommandHandler(
    IApplicationContext applicationContext,
    ISpecializationManager specializationManager,
    ISpecializationQuerier specializationQuerier,
    ISpecializationRepository specializationRepository,
    ITalentRepository talentRepository)
  {
    _applicationContext = applicationContext;
    _specializationManager = specializationManager;
    _specializationQuerier = specializationQuerier;
    _specializationRepository = specializationRepository;
    _talentRepository = talentRepository;
  }

  public async Task<CreateOrReplaceSpecializationResult> Handle(CreateOrReplaceSpecializationCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceSpecializationPayload payload = command.Payload;
    new CreateOrReplaceSpecializationValidator().ValidateAndThrow(payload);

    SpecializationId? specializationId = null;
    Specialization? specialization = null;
    if (command.Id.HasValue)
    {
      specializationId = new(command.Id.Value);
      specialization = await _specializationRepository.LoadAsync(specializationId.Value, cancellationToken);
    }

    Slug uniqueSlug = new(payload.UniqueSlug);
    ActorId? actorId = _applicationContext.ActorId;
    bool created = false;
    if (specialization == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceSpecializationResult();
      }

      specialization = new(payload.Tier, uniqueSlug, actorId, specializationId);
      created = true;
    }

    Specialization reference = (command.Version.HasValue
      ? await _specializationRepository.LoadAsync(specialization.Id, command.Version.Value, cancellationToken)
      : null) ?? specialization;

    if (reference.UniqueSlug != uniqueSlug)
    {
      specialization.UniqueSlug = uniqueSlug;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      specialization.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      specialization.Description = description;
    }

    await SetRequiredTalentAsync(specialization, reference, payload, cancellationToken);
    // TODO(fpion): OtherRequirements
    // TODO(fpion): OptionalTalentIds
    // TODO(fpion): OtherOptions
    ReservedTalent? reservedTalent = ToReservedTalent(payload.ReservedTalent);
    if (reference.ReservedTalent != reservedTalent)
    {
      specialization.ReservedTalent = reservedTalent;
    }

    specialization.Update(actorId);

    await _specializationManager.SaveAsync(specialization, cancellationToken);

    SpecializationModel model = await _specializationQuerier.ReadAsync(specialization, cancellationToken);
    return new CreateOrReplaceSpecializationResult(model, created);
  }

  private async Task SetRequiredTalentAsync(Specialization specialization, Specialization reference, CreateOrReplaceSpecializationPayload payload, CancellationToken cancellationToken)
  {
    TalentId? requiredTalentId = payload.RequiredTalentId.HasValue ? new(payload.RequiredTalentId.Value) : null;
    if (reference.RequiredTalentId != requiredTalentId)
    {
      Talent? talent = null;
      if (requiredTalentId.HasValue)
      {
        talent = await _talentRepository.LoadAsync(requiredTalentId.Value, cancellationToken)
          ?? throw new TalentNotFoundException(requiredTalentId.Value, nameof(payload.RequiredTalentId));
      }
      specialization.SetRequiredTalent(talent);
    }
  }

  private static ReservedTalent? ToReservedTalent(ReservedTalentModel? reservedTalent)
  {
    if (reservedTalent == null)
    {
      return null;
    }

    DisplayName name = new(reservedTalent.Name);
    Description[] descriptions = reservedTalent.Descriptions
      .Where(description => !string.IsNullOrWhiteSpace(description))
      .Select(description => new Description(description))
      .ToArray();
    return new ReservedTalent(name, descriptions);
  }
}
