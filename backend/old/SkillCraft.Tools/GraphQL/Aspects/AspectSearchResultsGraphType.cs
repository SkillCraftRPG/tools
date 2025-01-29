using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Aspects;

internal class AspectSearchResultsGraphType : SearchResultsGraphType<AspectGraphType, AspectModel>
{
  public AspectSearchResultsGraphType() : base("AspectSearchResults", "Represents the results of a aspect search.")
  {
  }
}
