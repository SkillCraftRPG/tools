using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Customizations
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Customizations));

  public static readonly ColumnId CreatedBy = new(nameof(CustomizationEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(CustomizationEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(CustomizationEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(CustomizationEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(CustomizationEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(CustomizationEntity.Version), Table);

  public static readonly ColumnId CustomizationId = new(nameof(CustomizationEntity.CustomizationId), Table);
  public static readonly ColumnId Description = new(nameof(CustomizationEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(CustomizationEntity.DisplayName), Table);
  public static readonly ColumnId Id = new(nameof(CustomizationEntity.Id), Table);
  public static readonly ColumnId Type = new(nameof(CustomizationEntity.Type), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(CustomizationEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(CustomizationEntity.UniqueSlugNormalized), Table);
}
