using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Customizations;

namespace SkillCraft.Tools.Core.Natures.Events;

public record NatureUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public Change<Ability?>? Attribute { get; set; }
  public Change<CustomizationId?>? GiftId { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null
    || Attribute != null || GiftId != null;
}
