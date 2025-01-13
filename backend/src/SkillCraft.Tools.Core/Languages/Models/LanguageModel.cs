namespace SkillCraft.Tools.Core.Languages.Models;

public class LanguageModel : AggregateModel
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Script { get; set; }
  public string? TypicalSpeakers { get; set; }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
