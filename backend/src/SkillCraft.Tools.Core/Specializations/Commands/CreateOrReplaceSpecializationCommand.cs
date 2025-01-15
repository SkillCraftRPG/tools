using FluentValidation;
using Logitar;
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

    await SetTalentsAsync(specialization, reference, payload, cancellationToken);

    IEnumerable<OtherRequirement> otherRequirements = payload.OtherRequirements
      .Where(x => !string.IsNullOrWhiteSpace(x))
      .Select(x => new OtherRequirement(x));
    if (reference.OtherRequirements != otherRequirements)
    {
      specialization.SetOtherRequirements(otherRequirements);
    }

    IEnumerable<OtherOption> otherOptions = payload.OtherOptions
      .Where(x => !string.IsNullOrWhiteSpace(x))
      .Select(x => new OtherOption(x));
    if (reference.OtherOptions != otherOptions)
    {
      specialization.SetOtherOptions(otherOptions);
    }

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

  private async Task SetTalentsAsync(Specialization specialization, Specialization reference, CreateOrReplaceSpecializationPayload payload, CancellationToken cancellationToken)
  {
    HashSet<TalentId> talentIds = new(capacity: 1 + payload.OptionalTalentIds.Count);
    TalentId? requiredTalentId = null;
    if (payload.RequiredTalentId.HasValue)
    {
      requiredTalentId = new(payload.RequiredTalentId.Value);
      talentIds.Add(requiredTalentId.Value);
    }
    HashSet<TalentId> optionalTalentIds = payload.OptionalTalentIds.Select(id => new TalentId(id)).ToHashSet();
    talentIds.AddRange(optionalTalentIds);
    Dictionary<TalentId, Talent> talents = (await _talentRepository.LoadAsync(talentIds, cancellationToken))
      .ToDictionary(x => x.Id, x => x);

    IEnumerable<TalentId> missingTalents = optionalTalentIds.Except(talents.Keys).Distinct();
    if (missingTalents.Any())
    {
      throw new NotImplementedException(); // TODO(fpion): typed exception
    }

    if (reference.RequiredTalentId != requiredTalentId)
    {
      Talent? requiredTalent = null;
      if (requiredTalentId.HasValue && !talents.TryGetValue(requiredTalentId.Value, out requiredTalent))
      {
        throw new TalentNotFoundException(requiredTalentId.Value, nameof(payload.RequiredTalentId));
      }
      specialization.SetRequiredTalent(requiredTalent);
    }

    foreach (TalentId optionalTalentId in reference.OptionalTalentIds)
    {
      if (!optionalTalentIds.Contains(optionalTalentId))
      {
        specialization.RemoveOptionalTalent(optionalTalentId);
      }
    }
    foreach (TalentId optionalTalentId in optionalTalentIds)
    {
      Talent optionalTalent = talents[optionalTalentId];
      if (!reference.HasOptionalTalent(optionalTalent))
      {
        specialization.AddOptionalTalent(optionalTalent);
      }
    }

    // TODO(fpion): should we throw a BadRequest exception for each talent (where Tier >= Specialization.Tier), or for a list?
  }

  private static ReservedTalent? ToReservedTalent(ReservedTalentModel? reservedTalent)
  {
    if (reservedTalent == null)
    {
      return null;
    }

    DisplayName name = new(reservedTalent.Name);
    IReadOnlyCollection<Description> descriptions = reservedTalent.Descriptions
      .Where(description => !string.IsNullOrWhiteSpace(description))
      .Select(description => new Description(description))
      .ToList()
      .AsReadOnly();
    return new ReservedTalent(name, descriptions);
  }
}
