using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Aspects.Events;

public record AspectUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  // TODO(fpion): Attributes
  // TODO(fpion): Skills

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null;
}
