using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Lineages
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Lineages));

  public static readonly ColumnId CreatedBy = new(nameof(LineageEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(LineageEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(LineageEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(LineageEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(LineageEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(LineageEntity.Version), Table);

  public static readonly ColumnId AdolescentAge = new(nameof(LineageEntity.AdolescentAge), Table);
  public static readonly ColumnId AdultAge = new(nameof(LineageEntity.AdultAge), Table);
  public static readonly ColumnId Agility = new(nameof(LineageEntity.Agility), Table);
  public static readonly ColumnId BurrowSpeed = new(nameof(LineageEntity.BurrowSpeed), Table);
  public static readonly ColumnId ClimbSpeed = new(nameof(LineageEntity.ClimbSpeed), Table);
  public static readonly ColumnId Coordination = new(nameof(LineageEntity.Coordination), Table);
  public static readonly ColumnId CustomNames = new(nameof(LineageEntity.CustomNames), Table);
  public static readonly ColumnId Description = new(nameof(LineageEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(LineageEntity.DisplayName), Table);
  public static readonly ColumnId ExtraAttributes = new(nameof(LineageEntity.ExtraAttributes), Table);
  public static readonly ColumnId ExtraLanguages = new(nameof(LineageEntity.ExtraLanguages), Table);
  public static readonly ColumnId FamilyNames = new(nameof(LineageEntity.FamilyNames), Table);
  public static readonly ColumnId FemaleNames = new(nameof(LineageEntity.FemaleNames), Table);
  public static readonly ColumnId FlySpeed = new(nameof(LineageEntity.FlySpeed), Table);
  public static readonly ColumnId HoverSpeed = new(nameof(LineageEntity.HoverSpeed), Table);
  public static readonly ColumnId Id = new(nameof(LineageEntity.Id), Table);
  public static readonly ColumnId Intellect = new(nameof(LineageEntity.Intellect), Table);
  public static readonly ColumnId LanguagesText = new(nameof(LineageEntity.LanguagesText), Table);
  public static readonly ColumnId LineageId = new(nameof(LineageEntity.LineageId), Table);
  public static readonly ColumnId MaleNames = new(nameof(LineageEntity.MaleNames), Table);
  public static readonly ColumnId MatureAge = new(nameof(LineageEntity.MatureAge), Table);
  public static readonly ColumnId NamesText = new(nameof(LineageEntity.NamesText), Table);
  public static readonly ColumnId NormalRoll = new(nameof(LineageEntity.NormalRoll), Table);
  public static readonly ColumnId ObeseRoll = new(nameof(LineageEntity.ObeseRoll), Table);
  public static readonly ColumnId OverweightRoll = new(nameof(LineageEntity.OverweightRoll), Table);
  public static readonly ColumnId ParentId = new(nameof(LineageEntity.ParentId), Table);
  public static readonly ColumnId Presence = new(nameof(LineageEntity.Presence), Table);
  public static readonly ColumnId Sensitivity = new(nameof(LineageEntity.Sensitivity), Table);
  public static readonly ColumnId SizeCategory = new(nameof(LineageEntity.SizeCategory), Table);
  public static readonly ColumnId SizeRoll = new(nameof(LineageEntity.SizeRoll), Table);
  public static readonly ColumnId SkinnyRoll = new(nameof(LineageEntity.SkinnyRoll), Table);
  public static readonly ColumnId Spirit = new(nameof(LineageEntity.Spirit), Table);
  public static readonly ColumnId StarvedRoll = new(nameof(LineageEntity.StarvedRoll), Table);
  public static readonly ColumnId SwimSpeed = new(nameof(LineageEntity.SwimSpeed), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(LineageEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(LineageEntity.UniqueSlugNormalized), Table);
  public static readonly ColumnId UnisexNames = new(nameof(LineageEntity.UnisexNames), Table);
  public static readonly ColumnId VenerableAge = new(nameof(LineageEntity.VenerableAge), Table);
  public static readonly ColumnId Vigor = new(nameof(LineageEntity.Vigor), Table);
  public static readonly ColumnId WalkSpeed = new(nameof(LineageEntity.WalkSpeed), Table);
}
