using GraphQL.Types;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Languages;

internal class LanguageSortOptionGraphType : SortOptionInputGraphType<LanguageSortOption>
{
  public LanguageSortOptionGraphType() : base()
  {
    Field(x => x.Field, type: typeof(NonNullGraphType<LanguageSortGraphType>))
      .Description("The field on which to apply the sort.");
  }
}
