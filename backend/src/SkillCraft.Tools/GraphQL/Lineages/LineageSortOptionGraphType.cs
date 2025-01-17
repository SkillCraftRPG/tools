using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class LineageSortOptionGraphType : SortOptionInputGraphType<LineageSortOption>
{
  public LineageSortOptionGraphType() : base()
  {
    Field(x => x.Field, type: typeof(NonNullGraphType<LineageSortGraphType>))
      .Description("The field on which to apply the sort.");
  }
}
