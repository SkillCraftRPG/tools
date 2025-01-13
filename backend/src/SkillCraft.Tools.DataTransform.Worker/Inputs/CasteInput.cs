using CsvHelper.Configuration.Attributes;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.DataTransform.Worker.Inputs;

internal class CasteInput
{
  [Name("id")]
  public Guid Id { get; set; }

  [Name("uniqueSlug")]
  public string UniqueSlug { get; set; } = string.Empty;

  [Name("displayName")]
  public string? DisplayName { get; set; }

  [Name("description")]
  public string? Description { get; set; }

  [Name("skill")]
  public Skill? Skill { get; set; }

  [Name("wealthRoll")]
  public string? WealthRoll { get; set; }

  [Name("traits[0].id")]
  public Guid? Feature1Id { get; set; }

  [Name("traits[0].name")]
  public string? Feature1Name { get; set; }

  [Name("traits[0].description")]
  public string? Feature1Description { get; set; }

  [Name("traits[1].id")]
  public Guid? Feature2Id { get; set; }

  [Name("traits[1].name")]
  public string? Feature2Name { get; set; }

  [Name("traits[1].description")]
  public string? Feature2Description { get; set; }
}
