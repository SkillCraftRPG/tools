using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Natures
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Natures));

  public static readonly ColumnId CreatedBy = new(nameof(NatureEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(NatureEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(NatureEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(NatureEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(NatureEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(NatureEntity.Version), Table);

  public static readonly ColumnId Attribute = new(nameof(NatureEntity.Attribute), Table);
  public static readonly ColumnId Description = new(nameof(NatureEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(NatureEntity.DisplayName), Table);
  public static readonly ColumnId GiftId = new(nameof(NatureEntity.GiftId), Table);
  public static readonly ColumnId Id = new(nameof(NatureEntity.Id), Table);
  public static readonly ColumnId NatureId = new(nameof(NatureEntity.NatureId), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(NatureEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(NatureEntity.UniqueSlugNormalized), Table);
}
