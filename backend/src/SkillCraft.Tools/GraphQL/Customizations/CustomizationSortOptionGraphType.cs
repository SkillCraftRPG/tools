using GraphQL.Types;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Customizations;

internal class CustomizationSortOptionGraphType : SortOptionInputGraphType<CustomizationSortOption>
{
  public CustomizationSortOptionGraphType() : base()
  {
    Field(x => x.Field, type: typeof(NonNullGraphType<CustomizationSortGraphType>))
      .Description("The field on which to apply the sort.");
  }
}
