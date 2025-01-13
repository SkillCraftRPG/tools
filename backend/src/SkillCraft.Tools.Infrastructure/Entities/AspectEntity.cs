using SkillCraft.Tools.Core.Aspects.Events;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class AspectEntity : AggregateEntity
{
  public int AspectId { get; private set; }
  public Guid Id { get; private set; }

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  // TODO(fpion): Attributes
  // TODO(fpion): Skills

  public AspectEntity(AspectCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private AspectEntity() : base()
  {
  }

  public void Update(AspectUpdated @event)
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

    // TODO(fpion): Attributes
    // TODO(fpion): Skills
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
