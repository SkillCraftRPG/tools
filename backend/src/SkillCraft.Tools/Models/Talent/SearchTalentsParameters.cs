using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Models.Search;

namespace SkillCraft.Tools.Models.Talent;

public record SearchTalentsParameters : SearchParameters
{
  [FromQuery(Name = "multiple")]
  public bool? AllowMultiplePurchases { get; set; }

  [FromQuery(Name = "required")]
  public Guid? RequiredTalentId { get; set; }

  [FromQuery(Name = "skill")]
  public string? Skill { get; set; }

  [FromQuery(Name = "tiers")]
  public IEnumerable<int>? TierValues { get; set; }

  [FromQuery(Name = "tier_operator")]
  public string? TierOperator { get; set; }

  public SearchTalentsPayload ToPayload()
  {
    SearchTalentsPayload payload = new()
    {
      AllowMultiplePurchases = AllowMultiplePurchases,
      RequiredTalentId = RequiredTalentId,
      Skill = Skill
    };
    if (TierValues != null)
    {
      payload.Tier = new TierFilter(TierOperator ?? string.Empty, TierValues);
    }
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out TalentSort field))
      {
        payload.Sort.Add(new TalentSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
