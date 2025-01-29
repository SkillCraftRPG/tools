using Logitar.EventSourcing;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Talents.Events;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class TalentEntity : AggregateEntity
{
  public int TalentId { get; private set; }
  public Guid Id { get; private set; }

  public int Tier { get; private set; }

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public bool AllowMultiplePurchases { get; private set; }
  public TalentEntity? RequiredTalent { get; private set; }
  public int? RequiredTalentId { get; private set; }
  public List<TalentEntity> RequiringTalents { get; private set; } = [];
  public Skill? Skill { get; private set; }

  public List<SpecializationEntity> OptionalSpecializations { get; private set; } = [];
  public List<SpecializationEntity> RequiringSpecializations { get; private set; } = [];

  public TalentEntity(TalentCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    Tier = @event.Tier;

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private TalentEntity() : base()
  {
  }

  public override IReadOnlyCollection<ActorId> GetActorIds()
  {
    List<ActorId> actorIds = [.. base.GetActorIds()];
    if (RequiredTalent != null)
    {
      actorIds.AddRange(RequiredTalent.GetActorIds());
    }
    return actorIds.AsReadOnly();
  }

  public void Update(TalentEntity? requiredTalent, TalentUpdated @event)
  {
    Update(@event);

    if (@event.UniqueSlug != null)
    {
      UniqueSlug = @event.UniqueSlug.Value;
    }
    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value?.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }

    if (@event.AllowMultiplePurchases.HasValue)
    {
      AllowMultiplePurchases = @event.AllowMultiplePurchases.Value;
    }
    if (@event.RequiredTalentId != null)
    {
      RequiredTalent = requiredTalent;
      RequiredTalentId = requiredTalent?.TalentId;
    }
    if (@event.Skill != null)
    {
      Skill = @event.Skill.Value;
    }
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
