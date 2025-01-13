using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Natures;

internal class NatureSearchResultsGraphType : SearchResultsGraphType<NatureGraphType, NatureModel>
{
  public NatureSearchResultsGraphType() : base("NatureSearchResults", "Represents the results of a nature search.")
  {
  }
}
