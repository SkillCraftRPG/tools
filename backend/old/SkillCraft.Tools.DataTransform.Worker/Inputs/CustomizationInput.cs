using CsvHelper.Configuration.Attributes;
using SkillCraft.Tools.Core.Customizations;

namespace SkillCraft.Tools.DataTransform.Worker.Inputs;

internal class CustomizationInput
{
  [Name("id")]
  public Guid Id { get; set; }

  [Name("type")]
  public CustomizationType Type { get; set; }

  [Name("uniqueSlug")]
  public string UniqueSlug { get; set; } = string.Empty;

  [Name("displayName")]
  public string? DisplayName { get; set; }

  [Name("description")]
  public string? Description { get; set; }
}
