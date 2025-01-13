using GraphQL.Types;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Aspects;

internal class AspectSortOptionGraphType : SortOptionInputGraphType<AspectSortOption>
{
  public AspectSortOptionGraphType() : base()
  {
    Field(x => x.Field, type: typeof(NonNullGraphType<AspectSortGraphType>))
      .Description("The field on which to apply the sort.");
  }
}
