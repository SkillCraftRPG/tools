using GraphQL.Types;
using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.GraphQL.Aspects;

internal class AspectSortGraphType : EnumerationGraphType<AspectSort>
{
  public AspectSortGraphType()
  {
    Name = "AspectSort";
    Description = "Represents the available aspect fields for sorting.";

    AddValue(AspectSort.CreatedOn, "The aspects will be sorted by their creation date and time.");
    AddValue(AspectSort.DisplayName, "The aspects will be sorted by their display name.");
    AddValue(AspectSort.UniqueSlug, "The aspects will be sorted by their unique slug.");
    AddValue(AspectSort.UpdatedOn, "The aspects will be sorted by their latest update date and time.");
  }
  private void AddValue(AspectSort value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
