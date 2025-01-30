using Logitar.Cms.Core.Contents.Events;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class SpecializationEntity : AggregateEntity
{
  public int SpecializationId { get; private set; }
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

  public TalentEntity? RequiredTalent { get; private set; }
  public int? RequiredTalentId { get; private set; }
  public string? OtherRequirements { get; set; }

  public string? OtherOptions { get; set; }

  public string? ReservedTalentName { get; set; }
  public string? ReservedTalentDescription { get; set; }

  public List<TalentEntity> OptionalTalents { get; private set; } = [];

  public SpecializationEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();
  }

  private SpecializationEntity() : base()
  {
  }

  public override IReadOnlyCollection<ActorId> GetActorIds()
  {
    List<ActorId> actorIds = [.. base.GetActorIds()];
    if (RequiredTalent != null)
    {
      actorIds.AddRange(RequiredTalent.GetActorIds());
    }
    foreach (TalentEntity optionalTalent in OptionalTalents)
    {
      actorIds.AddRange(optionalTalent.GetActorIds());
    }
    return actorIds.AsReadOnly();
  }

  public IReadOnlyCollection<string> GetOtherRequirements()
  {
    return (OtherRequirements == null ? null : JsonSerializer.Deserialize<IReadOnlyCollection<string>>(OtherRequirements)) ?? [];
  }
  public IReadOnlyCollection<string> GetOtherOptions()
  {
    return (OtherOptions == null ? null : JsonSerializer.Deserialize<IReadOnlyCollection<string>>(OtherOptions)) ?? [];
  }
  public ReservedTalentModel? GetReservedTalent() => ReservedTalentName == null ? null : new(ReservedTalentName, ReservedTalentDescription);

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
