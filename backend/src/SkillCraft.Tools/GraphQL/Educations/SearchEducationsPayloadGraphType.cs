using GraphQL.Types;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Educations;

internal class SearchEducationsPayloadGraphType : SearchPayloadInputGraphType<SearchEducationsPayload>
{
  public SearchEducationsPayloadGraphType() : base()
  {
    Field(x => x.Skill, type: typeof(SkillGraphType))
      .Description("When specified, only educations requiring this skill will match.");

    Field(x => x.Sort, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<EducationSortOptionGraphType>>>))
      .DefaultValue([])
      .Description("The sort parameters of the search.");
  }
}
