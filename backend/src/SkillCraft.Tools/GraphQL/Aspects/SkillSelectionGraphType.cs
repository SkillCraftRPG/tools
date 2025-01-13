using GraphQL.Types;
using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.GraphQL.Aspects;

internal class SkillSelectionGraphType : ObjectGraphType<SkillSelectionModel>
{
  public SkillSelectionGraphType()
  {
    Name = "Skills";
    Description = "Represents the skill selection of an aspect.";

    Field(x => x.Discounted1, type: typeof(SkillGraphType))
      .Description("The first discounted skill talent of the aspect.");
    Field(x => x.Discounted2, type: typeof(SkillGraphType))
      .Description("The second discounted skill talent of the aspect.");
  }
}
