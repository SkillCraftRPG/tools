using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.Models.Search;

namespace SkillCraft.Tools.Models.Customization;

public record SearchCustomizationsParameters : SearchParameters
{
  [FromQuery(Name = "type")]
  public CustomizationType? Type { get; set; }

  public SearchCustomizationsPayload ToPayload()
  {
    SearchCustomizationsPayload payload = new()
    {
      Type = Type
    };
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out CustomizationSort field))
      {
        payload.Sort.Add(new CustomizationSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
