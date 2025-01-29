using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Castes.Events;

public record CasteUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public Change<Skill?>? Skill { get; set; }
  public Change<Roll>? WealthRoll { get; set; }

  public Dictionary<Guid, Feature?> Features { get; set; } = [];

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null
    || Skill != null || WealthRoll != null || Features.Count > 0;
}
