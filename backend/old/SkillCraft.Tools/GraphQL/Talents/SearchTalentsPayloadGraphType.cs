using GraphQL.Types;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Talents;

internal class SearchTalentsPayloadGraphType : SearchPayloadInputGraphType<SearchTalentsPayload>
{
  public SearchTalentsPayloadGraphType() : base()
  {
    Field(x => x.AllowMultiplePurchases)
      .Description("When specified, only talents that allow or do not allow multiple purchases will match.");
    Field(x => x.RequiredTalentId)
      .Description("When specified, only talents requiring this talent will match.");
    Field(x => x.Skill)
      .Description("When specified, only talents associated to a skill or not will match. 'true' will match any talent associated to a skill, 'false' will match any talent not associated to a skill, and the name of the skill will only match talents associated to this skill.");
    Field(x => x.Tier, type: typeof(TierFilterGraphType))
      .Description("When specified, only talents matching the filter will match.");

    Field(x => x.Sort, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<TalentSortOptionGraphType>>>))
      .DefaultValue([])
      .Description("The sort parameters of the search.");
  }
}
