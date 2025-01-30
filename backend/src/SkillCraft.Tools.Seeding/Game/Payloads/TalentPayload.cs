using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.Seeding.Game.Payloads;

public record TalentPayload
{
  public Guid Id { get; set; }

  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public bool AllowMultiplePurchases { get; set; }
  public string? RequiredTalent { get; set; }
  public Skill? Skill { get; set; }
}
