using GraphQL.Types;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Customizations;

internal class SearchCustomizationsPayloadGraphType : SearchPayloadInputGraphType<SearchCustomizationsPayload>
{
  public SearchCustomizationsPayloadGraphType() : base()
  {
    Field(x => x.Type, type: typeof(CustomizationTypeGraphType))
      .Description("When specified, only customizations having this type will match.");

    Field(x => x.Sort, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<CustomizationSortOptionGraphType>>>))
      .DefaultValue([])
      .Description("The sort parameters of the search.");
  }
}
