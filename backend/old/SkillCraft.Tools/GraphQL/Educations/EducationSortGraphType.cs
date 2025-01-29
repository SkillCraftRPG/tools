using GraphQL.Types;
using SkillCraft.Tools.Core.Educations.Models;

namespace SkillCraft.Tools.GraphQL.Educations;

internal class EducationSortGraphType : EnumerationGraphType<EducationSort>
{
  public EducationSortGraphType()
  {
    Name = "EducationSort";
    Description = "Represents the available education fields for sorting.";

    AddValue(EducationSort.CreatedOn, "The educations will be sorted by their creation date and time.");
    AddValue(EducationSort.DisplayName, "The educations will be sorted by their display name.");
    AddValue(EducationSort.UniqueSlug, "The educations will be sorted by their unique slug.");
    AddValue(EducationSort.UpdatedOn, "The educations will be sorted by their latest update date and time.");
  }
  private void AddValue(EducationSort value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
