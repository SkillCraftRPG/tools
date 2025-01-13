using CsvHelper.Configuration.Attributes;

namespace SkillCraft.Tools.DataTransform.Worker.Inputs;

internal class LanguageInput
{
  [Name("id")]
  public Guid Id { get; set; }

  [Name("uniqueSlug")]
  public string UniqueSlug { get; set; } = string.Empty;
  [Name("displayName")]
  public string? DisplayName { get; set; }
  [Name("description")]
  public string? Description { get; set; }

  [Name("script")]
  public string? Script { get; set; }
  [Name("typicalSpeakers")]
  public string? TypicalSpeakers { get; set; }
}
