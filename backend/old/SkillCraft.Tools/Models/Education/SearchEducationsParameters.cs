using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.Models.Search;

namespace SkillCraft.Tools.Models.Education;

public record SearchEducationsParameters : SearchParameters
{
  [FromQuery(Name = "skill")]
  public Skill? Skill { get; set; }

  public SearchEducationsPayload ToPayload()
  {
    SearchEducationsPayload payload = new()
    {
      Skill = Skill
    };
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out EducationSort field))
      {
        payload.Sort.Add(new EducationSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
