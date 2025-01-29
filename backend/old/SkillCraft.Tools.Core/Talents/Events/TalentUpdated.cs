using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Talents.Events;

public record TalentUpdated : DomainEvent, INotification
{
  public Slug? UniqueSlug { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public bool? AllowMultiplePurchases { get; set; }
  public Change<TalentId?>? RequiredTalentId { get; set; }
  public Change<Skill?>? Skill { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueSlug != null || DisplayName != null || Description != null
    || AllowMultiplePurchases != null || RequiredTalentId != null || Skill != null;
}
