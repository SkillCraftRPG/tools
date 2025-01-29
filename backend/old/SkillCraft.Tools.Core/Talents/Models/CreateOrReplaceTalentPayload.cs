namespace SkillCraft.Tools.Core.Talents.Models;

public record CreateOrReplaceTalentPayload
{
  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public bool AllowMultiplePurchases { get; set; }
  public Guid? RequiredTalentId { get; set; }
  public Skill? Skill { get; set; }
}
