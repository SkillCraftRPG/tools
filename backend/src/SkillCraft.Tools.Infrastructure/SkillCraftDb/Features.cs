using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Features
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Features));

  public static readonly ColumnId CreatedBy = new(nameof(FeatureEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(FeatureEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(FeatureEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(FeatureEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(FeatureEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(FeatureEntity.Version), Table);

  public static readonly ColumnId Description = new(nameof(FeatureEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(FeatureEntity.DisplayName), Table);
  public static readonly ColumnId FeatureId = new(nameof(FeatureEntity.FeatureId), Table);
  public static readonly ColumnId Id = new(nameof(FeatureEntity.Id), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(FeatureEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(FeatureEntity.UniqueSlugNormalized), Table);
}
