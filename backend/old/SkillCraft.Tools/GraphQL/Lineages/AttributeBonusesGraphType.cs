using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class AttributeBonusesGraphType : ObjectGraphType<AttributeBonusesModel>
{
  public AttributeBonusesGraphType()
  {
    Name = "AttributeBonuses";
    Description = "Represents the attribute bonuses granted by a lineage.";

    Field(x => x.Agility)
      .Description("The bonus granted to the Agility attribute.");
    Field(x => x.Coordination)
      .Description("The bonus granted to the Coordination attribute.");
    Field(x => x.Intellect)
      .Description("The bonus granted to the Intellect attribute.");
    Field(x => x.Presence)
      .Description("The bonus granted to the Presence attribute.");
    Field(x => x.Sensitivity)
      .Description("The bonus granted to the Sensitivity attribute.");
    Field(x => x.Spirit)
      .Description("The bonus granted to the Spirit attribute.");
    Field(x => x.Vigor)
      .Description("The bonus granted to the Vigor attribute.");

    Field(x => x.Extra)
      .Description("The number of attribute bonuses to be selected by the player.");
  }
}
