using GraphQL.Types;
using SkillCraft.Tools.Core.Natures.Models;

namespace SkillCraft.Tools.GraphQL.Natures;

internal class NatureSortGraphType : EnumerationGraphType<NatureSort>
{
  public NatureSortGraphType()
  {
    Name = "NatureSort";
    Description = "Represents the available nature fields for sorting.";

    AddValue(NatureSort.CreatedOn, "The natures will be sorted by their creation date and time.");
    AddValue(NatureSort.DisplayName, "The natures will be sorted by their display name.");
    AddValue(NatureSort.UniqueSlug, "The natures will be sorted by their unique slug.");
    AddValue(NatureSort.UpdatedOn, "The natures will be sorted by their latest update date and time.");
  }
  private void AddValue(NatureSort value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
