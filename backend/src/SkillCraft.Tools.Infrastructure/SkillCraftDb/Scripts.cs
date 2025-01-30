using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Scripts
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Scripts));

  public static readonly ColumnId CreatedBy = new(nameof(ScriptEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(ScriptEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(ScriptEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(ScriptEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(ScriptEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(ScriptEntity.Version), Table);

  public static readonly ColumnId Description = new(nameof(ScriptEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(ScriptEntity.DisplayName), Table);
  public static readonly ColumnId ScriptId = new(nameof(ScriptEntity.ScriptId), Table);
  public static readonly ColumnId Id = new(nameof(ScriptEntity.Id), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(ScriptEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(ScriptEntity.UniqueSlugNormalized), Table);
}
