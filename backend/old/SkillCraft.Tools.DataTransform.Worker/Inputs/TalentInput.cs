using CsvHelper.Configuration.Attributes;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.DataTransform.Worker.Inputs;

internal class TalentInput
{
  [Name("id")]
  public Guid Id { get; set; }

  [Name("tier")]
  public int Tier { get; set; }

  [Name("uniqueSlug")]
  public string UniqueSlug { get; set; } = string.Empty;

  [Name("displayName")]
  public string? DisplayName { get; set; }

  [Name("description")]
  public string? Description { get; set; }

  [Name("allowMultiplePurchases")]
  public bool AllowMultiplePurchases { get; set; }

  [Name("requiredTalentId")]
  public Guid? RequiredTalentId { get; set; }

  [Name("skill")]
  public Skill? Skill { get; set; }
}
