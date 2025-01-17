using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class WeightGraphType : ObjectGraphType<WeightModel>
{
  public WeightGraphType()
  {
    Name = "Weight";
    Description = "Represents the weight categories of a lineage. The numbers give the BMI which can by used to calculate a weight in kilograms (kg).";

    Field(x => x.Starved)
      .Description(string.Empty);
    Field(x => x.Skinny)
      .Description(string.Empty);
    Field(x => x.Normal)
      .Description(string.Empty);
    Field(x => x.Overweight)
      .Description(string.Empty);
    Field(x => x.Obese)
      .Description(string.Empty);
  }
}
