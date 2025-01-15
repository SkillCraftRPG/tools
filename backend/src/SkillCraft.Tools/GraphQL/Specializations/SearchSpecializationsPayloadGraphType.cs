using GraphQL.Types;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Specializations;

internal class SearchSpecializationsPayloadGraphType : SearchPayloadInputGraphType<SearchSpecializationsPayload>
{
  public SearchSpecializationsPayloadGraphType() : base()
  {
    // TODO(fpion): Filters; Talents
    Field(x => x.Tier, type: typeof(TierFilterGraphType))
      .Description("When specified, only specializations matching the filter will match.");

    Field(x => x.Sort, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<SpecializationSortOptionGraphType>>>))
      .DefaultValue([])
      .Description("The sort parameters of the search.");
  }
}
