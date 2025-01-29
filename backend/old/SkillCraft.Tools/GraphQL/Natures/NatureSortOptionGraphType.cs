using GraphQL.Types;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Natures;

internal class NatureSortOptionGraphType : SortOptionInputGraphType<NatureSortOption>
{
  public NatureSortOptionGraphType() : base()
  {
    Field(x => x.Field, type: typeof(NonNullGraphType<NatureSortGraphType>))
      .Description("The field on which to apply the sort.");
  }
}
