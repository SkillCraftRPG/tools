namespace SkillCraft.Tools.Core.Aspects.Models;

public record CreateOrReplaceAspectPayload
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  // TODO(fpion): Attributes
  // TODO(fpion): Skills
}
