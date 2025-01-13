using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Languages.Events;

public record LanguageUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public Change<Script>? Script { get; set; }
  public Change<TypicalSpeakers>? TypicalSpeakers { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null
    || Script != null || TypicalSpeakers != null;
}
