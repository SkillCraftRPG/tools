using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Traits
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Traits));

  public static readonly ColumnId CreatedBy = new(nameof(TraitEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(TraitEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(TraitEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(TraitEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(TraitEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(TraitEntity.Version), Table);

  public static readonly ColumnId Description = new(nameof(TraitEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(TraitEntity.DisplayName), Table);
  public static readonly ColumnId TraitId = new(nameof(TraitEntity.TraitId), Table);
  public static readonly ColumnId Id = new(nameof(TraitEntity.Id), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(TraitEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(TraitEntity.UniqueSlugNormalized), Table);
}
