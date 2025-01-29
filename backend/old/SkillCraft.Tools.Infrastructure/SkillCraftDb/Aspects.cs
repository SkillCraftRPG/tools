using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Aspects
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Aspects));

  public static readonly ColumnId CreatedBy = new(nameof(AspectEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(AspectEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(AspectEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(AspectEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(AspectEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(AspectEntity.Version), Table);

  public static readonly ColumnId AspectId = new(nameof(AspectEntity.AspectId), Table);
  public static readonly ColumnId Description = new(nameof(AspectEntity.Description), Table);
  public static readonly ColumnId DiscountedSkill1 = new(nameof(AspectEntity.DiscountedSkill1), Table);
  public static readonly ColumnId DiscountedSkill2 = new(nameof(AspectEntity.DiscountedSkill2), Table);
  public static readonly ColumnId DisplayName = new(nameof(AspectEntity.DisplayName), Table);
  public static readonly ColumnId Id = new(nameof(AspectEntity.Id), Table);
  public static readonly ColumnId MandatoryAttribute1 = new(nameof(AspectEntity.MandatoryAttribute1), Table);
  public static readonly ColumnId MandatoryAttribute2 = new(nameof(AspectEntity.MandatoryAttribute2), Table);
  public static readonly ColumnId OptionalAttribute1 = new(nameof(AspectEntity.OptionalAttribute1), Table);
  public static readonly ColumnId OptionalAttribute2 = new(nameof(AspectEntity.OptionalAttribute2), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(AspectEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(AspectEntity.UniqueSlugNormalized), Table);
}
