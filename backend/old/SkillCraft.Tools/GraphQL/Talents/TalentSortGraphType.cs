using GraphQL.Types;
using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.GraphQL.Talents;

internal class TalentSortGraphType : EnumerationGraphType<TalentSort>
{
  public TalentSortGraphType()
  {
    Name = "TalentSort";
    Description = "Represents the available talent fields for sorting.";

    AddValue(TalentSort.CreatedOn, "The talents will be sorted by their creation date and time.");
    AddValue(TalentSort.DisplayName, "The talents will be sorted by their display name.");
    AddValue(TalentSort.UniqueSlug, "The talents will be sorted by their unique slug.");
    AddValue(TalentSort.UpdatedOn, "The talents will be sorted by their latest update date and time.");
  }
  private void AddValue(TalentSort value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
