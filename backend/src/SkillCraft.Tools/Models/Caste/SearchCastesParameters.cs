using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Models.Search;

namespace SkillCraft.Tools.Models.Caste;

public record SearchCastesParameters : SearchParameters
{
  [FromQuery(Name = "skill")]
  public Skill? Skill { get; set; }

  public SearchCastesPayload ToPayload()
  {
    SearchCastesPayload payload = new()
    {
      Skill = Skill
    };
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out CasteSort field))
      {
        payload.Sort.Add(new CasteSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
