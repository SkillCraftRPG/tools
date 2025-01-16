using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Models.Search;

namespace SkillCraft.Tools.Models.Specialization;

public record SearchSpecializationsParameters : SearchParameters
{
  [FromQuery(Name = "talent")]
  public Guid? TalentId { get; set; }

  [FromQuery(Name = "tiers")]
  public IEnumerable<int>? TierValues { get; set; }

  [FromQuery(Name = "tier_operator")]
  public string? TierOperator { get; set; }

  public SearchSpecializationsPayload ToPayload()
  {
    SearchSpecializationsPayload payload = new()
    {
      TalentId = TalentId
    };
    if (TierValues != null)
    {
      payload.Tier = new TierFilter(TierOperator ?? string.Empty, TierValues);
    }
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out SpecializationSort field))
      {
        payload.Sort.Add(new SpecializationSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
