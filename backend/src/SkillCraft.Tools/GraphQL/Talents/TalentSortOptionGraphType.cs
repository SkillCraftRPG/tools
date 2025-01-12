using GraphQL.Types;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Talents;

internal class TalentSortOptionGraphType : SortOptionInputGraphType<TalentSortOption>
{
  public TalentSortOptionGraphType() : base()
  {
    Field(x => x.Field, type: typeof(NonNullGraphType<TalentSortGraphType>))
      .Description("The field on which to apply the sort.");
  }
}
