using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Castes;

internal class CasteSearchResultsGraphType : SearchResultsGraphType<CasteGraphType, CasteModel>
{
  public CasteSearchResultsGraphType() : base("CasteSearchResults", "Represents the results of a caste search.")
  {
  }
}
