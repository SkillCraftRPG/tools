using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Languages;

internal class LanguageSearchResultsGraphType : SearchResultsGraphType<LanguageGraphType, LanguageModel>
{
  public LanguageSearchResultsGraphType() : base("LanguageSearchResults", "Represents the results of a language search.")
  {
  }
}
