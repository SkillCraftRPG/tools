namespace SkillCraft.Tools.Core.Educations.Models;

public record CreateOrReplaceEducationPayload
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Skill? Skill { get; set; }
  public double? WealthMultiplier { get; set; }
}
