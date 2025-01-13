using GraphQL.Types;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Castes;

internal class SearchCastesPayloadGraphType : SearchPayloadInputGraphType<SearchCastesPayload>
{
  public SearchCastesPayloadGraphType() : base()
  {
    Field(x => x.Skill, type: typeof(SkillGraphType))
      .Description("When specified, only castes requiring this skill will match.");

    Field(x => x.Sort, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<CasteSortOptionGraphType>>>))
      .DefaultValue([])
      .Description("The sort parameters of the search.");
  }
}
