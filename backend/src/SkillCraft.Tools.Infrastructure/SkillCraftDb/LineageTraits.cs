using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class LineageTraits
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.LineageTraits));

  public static readonly ColumnId LineageId = new(nameof(LineageTraitEntity.LineageId), Table);
  public static readonly ColumnId TraitId = new(nameof(LineageTraitEntity.TraitId), Table);
}
