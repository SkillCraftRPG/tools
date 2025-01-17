using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Lineages.Events;

public record LineageUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public AttributeBonuses? Attributes { get; set; }
  public Dictionary<Guid, Trait?> Traits { get; set; } = [];

  public Languages? Languages { get; set; }
  public Names? Names { get; set; }

  public Speeds? Speeds { get; set; }
  public Size? Size { get; set; }
  public Weight? Weight { get; set; }
  public Ages? Ages { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null
    || Attributes != null || Traits.Count > 0 || Languages != null || Names != null
    || Speeds != null || Size != null || Weight != null || Ages != null;
}
