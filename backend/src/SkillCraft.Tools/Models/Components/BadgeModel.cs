namespace SkillCraft.Tools.Models.Components;

public record BadgeModel
{
  public bool IsPill { get; set; }
  public BadgeVariant? Variant { get; set; } = BadgeVariant.Primary;
}
