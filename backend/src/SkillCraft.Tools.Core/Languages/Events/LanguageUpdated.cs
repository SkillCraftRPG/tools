using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Languages.Events;

public record LanguageUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  // TODO(fpion): Script
  // TODO(fpion): TypicalSpeakers

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null;
}
