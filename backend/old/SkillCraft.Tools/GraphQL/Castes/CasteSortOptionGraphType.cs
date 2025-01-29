using GraphQL.Types;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Castes;

internal class CasteSortOptionGraphType : SortOptionInputGraphType<CasteSortOption>
{
  public CasteSortOptionGraphType() : base()
  {
    Field(x => x.Field, type: typeof(NonNullGraphType<CasteSortGraphType>))
      .Description("The field on which to apply the sort.");
  }
}
