using Logitar.EventSourcing;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Natures.Events;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class NatureEntity : AggregateEntity
{
  public int NatureId { get; private set; }
  public Guid Id { get; private set; }

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public Ability? Attribute { get; private set; }
  public int? GiftId { get; private set; }
  public CustomizationEntity? Gift { get; private set; }

  public NatureEntity(NatureCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private NatureEntity() : base()
  {
  }

  public override IReadOnlyCollection<ActorId> GetActorIds()
  {
    List<ActorId> actorIds = [.. base.GetActorIds()];
    if (Gift != null)
    {
      actorIds.AddRange(Gift.GetActorIds());
    }
    return actorIds.AsReadOnly();
  }

  public void Update(CustomizationEntity? gift, NatureUpdated @event)
  {
    Update(@event);

    if (@event.UniqueSlug != null)
    {
      UniqueSlug = @event.UniqueSlug.Value;
    }
    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value?.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }

    if (@event.Attribute != null)
    {
      Attribute = @event.Attribute.Value;
    }
    if (@event.GiftId != null)
    {
      Gift = gift;
      GiftId = gift?.CustomizationId;
    }
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
