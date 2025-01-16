using GraphQL.Types;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Specializations;

internal class SpecializationSortOptionGraphType : SortOptionInputGraphType<SpecializationSortOption>
{
  public SpecializationSortOptionGraphType() : base()
  {
    Field(x => x.Field, type: typeof(NonNullGraphType<SpecializationSortGraphType>))
      .Description("The field on which to apply the sort.");
  }
}
