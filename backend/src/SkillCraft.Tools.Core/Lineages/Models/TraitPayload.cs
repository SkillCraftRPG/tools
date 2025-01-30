namespace SkillCraft.Tools.Core.Lineages.Models;

public record TraitPayload
{
  public Guid? Id { get; set; }

  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
}
