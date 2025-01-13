namespace SkillCraft.Tools.Core.Castes.Models;

public record CreateOrReplaceCastePayload
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Skill? Skill { get; set; }
  // TODO(fpion): WealthRoll

  // TODO(fpion): Features
}
