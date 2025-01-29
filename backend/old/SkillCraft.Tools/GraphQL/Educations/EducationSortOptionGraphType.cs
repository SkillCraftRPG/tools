using GraphQL.Types;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Educations;

internal class EducationSortOptionGraphType : SortOptionInputGraphType<EducationSortOption>
{
  public EducationSortOptionGraphType() : base()
  {
    Field(x => x.Field, type: typeof(NonNullGraphType<EducationSortGraphType>))
      .Description("The field on which to apply the sort.");
  }
}
