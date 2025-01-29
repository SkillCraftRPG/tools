using GraphQL.Types;
using SkillCraft.Tools.Core.Customizations;

namespace SkillCraft.Tools.GraphQL.Customizations;

internal class CustomizationTypeGraphType : EnumerationGraphType<CustomizationType>
{
  public CustomizationTypeGraphType()
  {
    Name = "CustomizationType";
    Description = "Represents the available customization types.";

    AddValue(CustomizationType.Disability, "The customization is a disability. Disabilities are handicaps and detrimental to a character.");
    AddValue(CustomizationType.Gift, "The customization is a gift. Gifts are feats and beneficial to characters.");
  }
  private void AddValue(CustomizationType value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
