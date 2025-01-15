using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Specializations.Events;

public record SpecializationUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  // TODO(fpion): RequiredTalentId
  // TODO(fpion): OtherRequirements
  // TODO(fpion): OptionalTalentIds
  // TODO(fpion): OtherOptions
  // TODO(fpion): ReservedTalent

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null;
}
