namespace SkillCraft.Tools.Core.Languages.Models;

public record CreateOrReplaceLanguagePayload
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  // TODO(fpion): Script
  // TODO(fpion): TypicalSpeakers
}
