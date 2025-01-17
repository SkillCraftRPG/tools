using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.Models.Search;

namespace SkillCraft.Tools.Models.Lineage;

public record SearchLineagesParameters : SearchParameters
{
  [FromQuery(Name = "attribute")]
  public Ability? Attribute { get; set; }

  [FromQuery(Name = "language")]
  public Guid? LanguageId { get; set; }

  [FromQuery(Name = "parent")]
  public Guid? ParentId { get; set; }

  [FromQuery(Name = "size")]
  public SizeCategory? SizeCategory { get; set; }

  public SearchLineagesPayload ToPayload()
  {
    SearchLineagesPayload payload = new()
    {
      Attribute = Attribute,
      LanguageId = LanguageId,
      ParentId = ParentId,
      SizeCategory = SizeCategory
    };
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out LineageSort field))
      {
        payload.Sort.Add(new LineageSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
