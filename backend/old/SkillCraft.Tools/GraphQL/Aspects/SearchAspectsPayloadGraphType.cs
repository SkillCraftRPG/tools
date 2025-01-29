using GraphQL.Types;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Aspects;

internal class SearchAspectsPayloadGraphType : SearchPayloadInputGraphType<SearchAspectsPayload>
{
  public SearchAspectsPayloadGraphType() : base()
  {
    Field(x => x.Attribute, type: typeof(AttributeGraphType))
      .Description("When specified, only aspects selecting this attribute will match.");
    Field(x => x.Skill, type: typeof(SkillGraphType))
      .Description("When specified, only aspects discounting this skill talent will match.");

    Field(x => x.Sort, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<AspectSortOptionGraphType>>>))
      .DefaultValue([])
      .Description("The sort parameters of the search.");
  }
}
