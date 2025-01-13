using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Castes;
using SkillCraft.Tools.Core.Castes.Events;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class CasteEntity : AggregateEntity
{
  public int CasteId { get; private set; }
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
  public string? WealthRoll { get; private set; }

  public string? Features { get; private set; }

  public CasteEntity(CasteCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private CasteEntity() : base()
  {
  }

  public void Update(CasteUpdated @event)
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
    if (@event.WealthRoll != null)
    {
      WealthRoll = @event.WealthRoll.Value?.Value;
    }

    Dictionary<Guid, FeatureModel> features = GetFeatures();
    foreach (KeyValuePair<Guid, Feature?> feature in @event.Features)
    {
      if (feature.Value == null)
      {
        features.Remove(feature.Key);
      }
      else
      {
        features[feature.Key] = new FeatureModel
        {
          Id = feature.Key,
          Name = feature.Value.Name.Value,
          Description = feature.Value.Description?.Value
        };
      }
    }
    SetFeatures(features);
  }

  public Dictionary<Guid, FeatureModel> GetFeatures()
  {
    return (Features == null ? null : JsonSerializer.Deserialize<Dictionary<Guid, FeatureModel>>(Features)) ?? [];
  }
  private void SetFeatures(Dictionary<Guid, FeatureModel> features)
  {
    Features = features.Count < 1 ? null : JsonSerializer.Serialize(features);
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
