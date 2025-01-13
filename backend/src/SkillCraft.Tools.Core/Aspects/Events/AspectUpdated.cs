using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Aspects.Events;

public record AspectUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public AttributeSelection? Attributes { get; set; }
  public SkillSelection? Skills { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null
    || Attributes != null || Skills != null;
}
