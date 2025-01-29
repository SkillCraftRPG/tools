namespace SkillCraft.Tools.Core.Natures.Models;

public record CreateOrReplaceNaturePayload
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Ability? Attribute { get; set; }
  public Guid? GiftId { get; set; }
}
