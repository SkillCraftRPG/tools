using GraphQL.Types;
using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.GraphQL.Specializations;

internal class SpecializationSortGraphType : EnumerationGraphType<SpecializationSort>
{
  public SpecializationSortGraphType()
  {
    Name = "SpecializationSort";
    Description = "Represents the available specialization fields for sorting.";

    AddValue(SpecializationSort.CreatedOn, "The specializations will be sorted by their creation date and time.");
    AddValue(SpecializationSort.DisplayName, "The specializations will be sorted by their display name.");
    AddValue(SpecializationSort.UniqueSlug, "The specializations will be sorted by their unique slug.");
    AddValue(SpecializationSort.UpdatedOn, "The specializations will be sorted by their latest update date and time.");
  }
  private void AddValue(SpecializationSort value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
