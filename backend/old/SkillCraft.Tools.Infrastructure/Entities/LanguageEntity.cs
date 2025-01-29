using SkillCraft.Tools.Core.Languages.Events;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class LanguageEntity : AggregateEntity
{
  public int LanguageId { get; private set; }
  public Guid Id { get; private set; }

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public string? Script { get; private set; }
  public string? TypicalSpeakers { get; private set; }

  public List<LineageEntity> Lineages { get; private set; } = [];

  public LanguageEntity(LanguageCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private LanguageEntity() : base()
  {
  }

  public void Update(LanguageUpdated @event)
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

    if (@event.Script != null)
    {
      Script = @event.Script.Value?.Value;
    }
    if (@event.TypicalSpeakers != null)
    {
      TypicalSpeakers = @event.TypicalSpeakers.Value?.Value;
    }
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
