using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Educations.Events;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class EducationEntity : AggregateEntity
{
  public int EducationId { get; private set; }
  public Guid Id { get; private set; }

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public Skill? Skill { get; private set; }
  // TODO(fpion): WealthMultiplier

  public EducationEntity(EducationCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private EducationEntity() : base()
  {
  }

  public void Update(EducationUpdated @event)
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

    if (@event.Skill != null)
    {
      Skill = @event.Skill.Value;
    }
    // TODO(fpion): WealthMultiplier
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
