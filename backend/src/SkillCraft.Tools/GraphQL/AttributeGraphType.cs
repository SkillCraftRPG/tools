using GraphQL.Types;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.GraphQL;

internal class AttributeGraphType : EnumerationGraphType<Ability>
{
  public AttributeGraphType()
  {
    Name = nameof(Attribute);
    Description = "Represents the available character attributes.";

    AddValue(Ability.Agility, string.Empty);
    AddValue(Ability.Coordination, string.Empty);
    AddValue(Ability.Intellect, string.Empty);
    AddValue(Ability.Presence, string.Empty);
    AddValue(Ability.Sensitivity, string.Empty);
    AddValue(Ability.Spirit, string.Empty);
    AddValue(Ability.Vigor, string.Empty);
  }
  private void AddValue(Ability value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
