using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Talents
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Talents));

  public static readonly ColumnId CreatedBy = new(nameof(TalentEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(TalentEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(TalentEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(TalentEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(TalentEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(TalentEntity.Version), Table);

  public static readonly ColumnId AllowMultiplePurchases = new(nameof(TalentEntity.AllowMultiplePurchases), Table);
  public static readonly ColumnId Description = new(nameof(TalentEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(TalentEntity.DisplayName), Table);
  public static readonly ColumnId Id = new(nameof(TalentEntity.Id), Table);
  public static readonly ColumnId RequiredTalentId = new(nameof(TalentEntity.RequiredTalentId), Table);
  public static readonly ColumnId Skill = new(nameof(TalentEntity.Skill), Table);
  public static readonly ColumnId TalentId = new(nameof(TalentEntity.TalentId), Table);
  public static readonly ColumnId Tier = new(nameof(TalentEntity.Tier), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(TalentEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(TalentEntity.UniqueSlugNormalized), Table);
}
