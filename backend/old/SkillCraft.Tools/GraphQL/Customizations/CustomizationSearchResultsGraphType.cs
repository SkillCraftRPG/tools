using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Customizations;

internal class CustomizationSearchResultsGraphType : SearchResultsGraphType<CustomizationGraphType, CustomizationModel>
{
  public CustomizationSearchResultsGraphType() : base("CustomizationSearchResults", "Represents the results of a customization search.")
  {
  }
}
