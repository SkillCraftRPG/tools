using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Customizations.Events;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class CustomizationEntity : AggregateEntity
{
  public int CustomizationId { get; private set; }
  public Guid Id { get; private set; }

  public CustomizationType Type { get; private set; }

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public CustomizationEntity(CustomizationCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    Type = @event.Type;

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private CustomizationEntity() : base()
  {
  }

  public void Update(CustomizationUpdated @event)
  {
    base.Update(@event);

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
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
