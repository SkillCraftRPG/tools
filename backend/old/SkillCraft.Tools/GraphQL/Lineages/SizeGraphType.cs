using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class SizeGraphType : ObjectGraphType<SizeModel>
{
  public SizeGraphType()
  {
    Name = "Size";
    Description = "Represents the size parameters of a lineage.";

    Field(x => x.Category, type: typeof(NonNullGraphType<SizeCategoryGraphType>))
      .Description("The size category of this lineage.");
    Field(x => x.Roll)
      .Description("The size roll of this lineage, in centimeters (cm).");
  }
}
