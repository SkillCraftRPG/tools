using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Educations.Events;

public record EducationUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public Change<Skill?>? Skill { get; set; }
  // TODO(fpion): WealthMultiplier

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null
    || Skill != null;
}
