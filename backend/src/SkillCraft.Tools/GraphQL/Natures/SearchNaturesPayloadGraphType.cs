using GraphQL.Types;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Natures;

internal class SearchNaturesPayloadGraphType : SearchPayloadInputGraphType<SearchNaturesPayload>
{
  public SearchNaturesPayloadGraphType() : base()
  {
    Field(x => x.Attribute, type: typeof(AttributeGraphType))
      .Description("When specified, only natures granting a bonus to this attribute will match.");
    Field(x => x.GiftId)
      .Description("When specified, only natures granting this gift will match.");

    Field(x => x.Sort, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<NatureSortOptionGraphType>>>))
      .DefaultValue([])
      .Description("The sort parameters of the search.");
  }
}
