using CsvHelper.Configuration.Attributes;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.DataTransform.Worker.Inputs;

internal class EducationInput
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
  [Name("wealthMultiplier")]
  public double? WealthMultiplier { get; set; }
}
