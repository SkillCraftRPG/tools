using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class CasteFeatures
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.CasteFeatures));

  public static readonly ColumnId CasteId = new(nameof(CasteFeatureEntity.CasteId), Table);
  public static readonly ColumnId FeatureId = new(nameof(CasteFeatureEntity.FeatureId), Table);
}
