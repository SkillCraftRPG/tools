using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class NameCategoryGraphType : ObjectGraphType<NameCategory>
{
  public NameCategoryGraphType()
  {
    Name = "NameCategory";
    Description = "Represents a custom name category.";

    Field(x => x.Key)
      .Description("The unique key of the category.");
    Field(x => x.Values)
      .Description("The list of names in this category.");
  }
}
