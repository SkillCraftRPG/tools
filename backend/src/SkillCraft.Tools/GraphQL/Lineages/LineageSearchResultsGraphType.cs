using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class LineageSearchResultsGraphType : SearchResultsGraphType<LineageGraphType, LineageModel>
{
  public LineageSearchResultsGraphType() : base("LineageSearchResults", "Represents the results of a lineage search.")
  {
  }
}
