using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Educations;

internal class EducationSearchResultsGraphType : SearchResultsGraphType<EducationGraphType, EducationModel>
{
  public EducationSearchResultsGraphType() : base("EducationSearchResults", "Represents the results of a education search.")
  {
  }
}
