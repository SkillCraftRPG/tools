namespace SkillCraft.Tools.Core.Castes.Models;

public record FeaturePayload
{
  public Guid? Id { get; set; }

  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
}
