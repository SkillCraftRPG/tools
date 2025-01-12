namespace SkillCraft.Tools.Core.Customizations.Models;

public record CreateOrReplaceCustomizationPayload
{
  public CustomizationType Type { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }
}
