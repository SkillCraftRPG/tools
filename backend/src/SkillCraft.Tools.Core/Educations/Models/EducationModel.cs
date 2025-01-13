namespace SkillCraft.Tools.Core.Educations.Models;

public class EducationModel : AggregateModel
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Skill? Skill { get; set; }
  // TODO(fpion): WealthMultiplier

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
