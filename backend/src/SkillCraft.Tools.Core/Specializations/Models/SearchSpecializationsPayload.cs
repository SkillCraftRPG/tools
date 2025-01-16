using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.Core.Specializations.Models;

public record SearchSpecializationsPayload : SearchPayload
{
  public Guid? TalentId { get; set; }
  public TierFilter? Tier { get; set; }

  public new List<SpecializationSortOption> Sort { get; set; } = [];
}
