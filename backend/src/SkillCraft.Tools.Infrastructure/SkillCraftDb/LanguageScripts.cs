using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class LanguageScripts
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.LanguageScripts));

  public static readonly ColumnId LanguageId = new(nameof(LanguageScriptEntity.LanguageId), Table);
  public static readonly ColumnId ScriptId = new(nameof(LanguageScriptEntity.ScriptId), Table);
}
