using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Core.Talents.Validators;

namespace SkillCraft.Tools.Core.Talents.Commands;

public record CreateOrReplaceTalentResult(TalentModel? Talent = null, bool Created = false);

public record CreateOrReplaceTalentCommand(Guid? Id, CreateOrReplaceTalentPayload Payload, long? Version) : Activity, IRequest<CreateOrReplaceTalentResult>;

internal class CreateOrReplaceTalentCommandHandler : IRequestHandler<CreateOrReplaceTalentCommand, CreateOrReplaceTalentResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ITalentManager _talentManager;
  private readonly ITalentQuerier _talentQuerier;
  private readonly ITalentRepository _talentRepository;

  public CreateOrReplaceTalentCommandHandler(
    IApplicationContext applicationContext,
    ITalentManager talentManager,
    ITalentQuerier talentQuerier,
    ITalentRepository talentRepository)
  {
    _applicationContext = applicationContext;
    _talentManager = talentManager;
    _talentQuerier = talentQuerier;
    _talentRepository = talentRepository;
  }

  public async Task<CreateOrReplaceTalentResult> Handle(CreateOrReplaceTalentCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceTalentPayload payload = command.Payload;
    new CreateOrReplaceTalentValidator().ValidateAndThrow(payload);

    TalentId? talentId = null;
    Talent? talent = null;
    if (command.Id.HasValue)
    {
      talentId = new(command.Id.Value);
      talent = await _talentRepository.LoadAsync(talentId.Value, cancellationToken);
    }

    Slug uniqueSlug = new(payload.UniqueSlug);
    ActorId? actorId = _applicationContext.ActorId;
    bool created = false;
    if (talent == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceTalentResult();
      }

      talent = new(payload.Tier, uniqueSlug, actorId, talentId);
      created = true;
    }

    Talent reference = (command.Version.HasValue
      ? await _talentRepository.LoadAsync(talent.Id, command.Version.Value, cancellationToken)
      : null) ?? talent;

    if (reference.UniqueSlug != uniqueSlug)
    {
      talent.UniqueSlug = uniqueSlug;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      talent.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      talent.Description = description;
    }

    if (reference.AllowMultiplePurchases != payload.AllowMultiplePurchases)
    {
      talent.AllowMultiplePurchases = payload.AllowMultiplePurchases;
    }
    if (reference.Skill != payload.Skill)
    {
      talent.Skill = payload.Skill;
    }

    TalentId? requiredTalentId = payload.RequiredTalentId.HasValue ? new(payload.RequiredTalentId.Value) : null;
    if (reference.RequiredTalentId != requiredTalentId)
    {
      Talent? requiredTalent = null;
      if (requiredTalentId.HasValue)
      {
        requiredTalent = await _talentRepository.LoadAsync(requiredTalentId.Value, cancellationToken)
          ?? throw new TalentNotFoundException(requiredTalentId.Value, nameof(payload.RequiredTalentId));
      }
      talent.SetRequiredTalent(requiredTalent);
    }

    talent.Update(actorId);

    await _talentManager.SaveAsync(talent, cancellationToken);

    TalentModel model = await _talentQuerier.ReadAsync(talent, cancellationToken);
    return new CreateOrReplaceTalentResult(model, created);
  }
}
