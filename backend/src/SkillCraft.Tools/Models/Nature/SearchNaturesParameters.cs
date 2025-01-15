using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.Models.Search;

namespace SkillCraft.Tools.Models.Nature;

public record SearchNaturesParameters : SearchParameters
{
  [FromQuery(Name = "attribute")]
  public Ability? Attribute { get; set; }

  [FromQuery(Name = "gift")]
  public Guid? GiftId { get; set; }

  public SearchNaturesPayload ToPayload()
  {
    SearchNaturesPayload payload = new()
    {
      Attribute = Attribute,
      GiftId = GiftId
    };
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out NatureSort field))
      {
        payload.Sort.Add(new NatureSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
