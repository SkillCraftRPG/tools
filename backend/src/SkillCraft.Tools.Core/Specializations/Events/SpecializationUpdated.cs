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
  // TODO(fpion): OtherRequirements
  // TODO(fpion): OptionalTalentIds
  // TODO(fpion): OtherOptions
  public Change<ReservedTalent>? ReservedTalent { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null
    || ReservedTalent != null;
}
