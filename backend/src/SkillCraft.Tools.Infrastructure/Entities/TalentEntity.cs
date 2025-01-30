using Logitar.Cms.Core.Contents.Events;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class TalentEntity : AggregateEntity
{
  public int TalentId { get; private set; }
  public Guid Id { get; private set; }

  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public bool AllowMultiplePurchases { get; set; }
  public TalentEntity? RequiredTalent { get; private set; }
  public int? RequiredTalentId { get; private set; }
  public List<TalentEntity> RequiringTalents { get; private set; } = [];
  public Skill? Skill { get; set; }

  public List<SpecializationEntity> OptionalSpecializations { get; private set; } = [];
  public List<SpecializationEntity> RequiringSpecializations { get; private set; } = [];

  public TalentEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();
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

  public void SetRequiredTalent(TalentEntity? requiredTalent)
  {
    RequiredTalent = requiredTalent;
    RequiredTalentId = requiredTalent?.TalentId;
  }

  public void Update(ContentLocalePublished @event)
  {
    base.Update(@event);
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
