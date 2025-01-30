using Logitar.Cms.Core.Search;

namespace SkillCraft.Tools.Core.Talents.Models;

public record SearchTalentsPayload : SearchPayload
{
  public bool? AllowMultiplePurchases { get; set; }
  public Guid? RequiredTalentId { get; set; }
  public string? Skill { get; set; }
  public TierFilter? Tier { get; set; }

  public new List<TalentSortOption> Sort { get; set; } = [];
}
