using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Talents;

namespace SkillCraft.Tools.Core.Specializations.Events;

public record SpecializationUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public Change<TalentId?>? RequiredTalentId { get; set; }
  public IReadOnlyCollection<OtherRequirement>? OtherRequirements { get; set; }

  public Dictionary<TalentId, bool> OptionalTalentIds { get; set; } = [];
  public IReadOnlyCollection<OtherOption>? OtherOptions { get; set; }

  public Change<ReservedTalent>? ReservedTalent { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null
    || RequiredTalentId != null || OtherRequirements != null
    || OptionalTalentIds.Count > 0 || OtherOptions != null
    || ReservedTalent != null;
}
