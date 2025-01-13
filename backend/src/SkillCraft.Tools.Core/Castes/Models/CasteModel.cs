namespace SkillCraft.Tools.Core.Castes.Models;

public class CasteModel : AggregateModel
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Skill? Skill { get; set; }
  // TODO(fpion): WealthRoll

  // TODO(fpion): Features

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
