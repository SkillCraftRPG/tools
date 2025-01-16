using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Specializations;

internal class SpecializationSearchResultsGraphType : SearchResultsGraphType<SpecializationGraphType, SpecializationModel>
{
  public SpecializationSearchResultsGraphType() : base("SpecializationSearchResults", "Represents the results of a specialization search.")
  {
  }
}
