namespace SkillCraft.Tools.Core.Aspects.Models;

public record CreateOrReplaceAspectPayload
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public AttributeSelectionModel Attributes { get; set; } = new();
  public SkillSelectionModel Skills { get; set; } = new();
}
