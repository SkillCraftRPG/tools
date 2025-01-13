namespace SkillCraft.Tools.Core.Castes.Models;

public record FeatureModel
{
  public Guid Id { get; set; }

  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
}
