using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Castes
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Castes));

  public static readonly ColumnId CreatedBy = new(nameof(CasteEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(CasteEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(CasteEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(CasteEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(CasteEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(CasteEntity.Version), Table);

  public static readonly ColumnId CasteId = new(nameof(CasteEntity.CasteId), Table);
  public static readonly ColumnId Description = new(nameof(CasteEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(CasteEntity.DisplayName), Table);
  public static readonly ColumnId Id = new(nameof(CasteEntity.Id), Table);
  public static readonly ColumnId Skill = new(nameof(CasteEntity.Skill), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(CasteEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(CasteEntity.UniqueSlugNormalized), Table);
  public static readonly ColumnId WealthRoll = new(nameof(CasteEntity.WealthRoll), Table);
  // TODO(fpion): Features
}
