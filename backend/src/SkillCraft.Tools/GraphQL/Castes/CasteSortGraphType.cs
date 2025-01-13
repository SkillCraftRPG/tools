using GraphQL.Types;
using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.GraphQL.Castes;

internal class CasteSortGraphType : EnumerationGraphType<CasteSort>
{
  public CasteSortGraphType()
  {
    Name = "CasteSort";
    Description = "Represents the available caste fields for sorting.";

    AddValue(CasteSort.CreatedOn, "The castes will be sorted by their creation date and time.");
    AddValue(CasteSort.DisplayName, "The castes will be sorted by their display name.");
    AddValue(CasteSort.UniqueSlug, "The castes will be sorted by their unique slug.");
    AddValue(CasteSort.UpdatedOn, "The castes will be sorted by their latest update date and time.");
  }
  private void AddValue(CasteSort value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
