using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class LineageLanguages
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.LineageLanguages));

  public static readonly ColumnId LanguageId = new(nameof(LineageLanguageEntity.LanguageId), Table);
  public static readonly ColumnId LineageId = new(nameof(LineageLanguageEntity.LineageId), Table);
}
