using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class SpeedsGraphType : ObjectGraphType<SpeedsModel>
{
  public SpeedsGraphType()
  {
    Name = "Speeds";
    Description = "Represents the movement speeds of a lineage, in squares per action on a standard grid.";

    Field(x => x.Walk)
      .Description("The walking speed of the lineage.");
    Field(x => x.Climb)
      .Description("The climbing speed of the lineage.");
    Field(x => x.Swim)
      .Description("The swimming speed of the lineage.");
    Field(x => x.Fly)
      .Description("The flying speed of the lineage.");
    Field(x => x.Hover)
      .Description("The hovering speed of the lineage.");
    Field(x => x.Burrow)
      .Description("The burrowing speed of the lineage.");
  }
}
