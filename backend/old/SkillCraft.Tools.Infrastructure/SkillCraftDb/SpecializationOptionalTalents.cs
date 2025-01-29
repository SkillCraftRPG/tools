using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class SpecializationOptionalTalents
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.SpecializationOptionalTalents));

  public static readonly ColumnId SpecializationId = new(nameof(SpecializationOptionalTalentEntity.SpecializationId), Table);
  public static readonly ColumnId TalentId = new(nameof(SpecializationOptionalTalentEntity.TalentId), Table);
}
