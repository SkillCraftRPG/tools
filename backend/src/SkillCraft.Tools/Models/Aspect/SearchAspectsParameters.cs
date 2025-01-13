using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Core.Search;
using SkillCraft.Tools.Models.Search;

namespace SkillCraft.Tools.Models.Aspect;

public record SearchAspectsParameters : SearchParameters
{
  [FromQuery(Name = "attribute")]
  public Ability? Attribute { get; set; }

  [FromQuery(Name = "skill")]
  public Skill? Skill { get; set; }

  public SearchAspectsPayload ToPayload()
  {
    SearchAspectsPayload payload = new()
    {
      Attribute = Attribute,
      Skill = Skill
    };
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out AspectSort field))
      {
        payload.Sort.Add(new AspectSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
