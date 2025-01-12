using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Customizations.Events;

public record CustomizationUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null;
}
