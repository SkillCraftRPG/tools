using GraphQL.Types;
using SkillCraft.Tools.Core.Customizations.Models;

namespace SkillCraft.Tools.GraphQL.Customizations;

internal class CustomizationGraphType : AggregateGraphType<CustomizationModel>
{
  public CustomizationGraphType() : base("Represents a character customization.")
  {
    Field(x => x.Type, type: typeof(NonNullGraphType<CustomizationTypeGraphType>))
      .Description("The type of the customization.");
    Field(x => x.UniqueSlug)
      .Description("The unique slug of the customization.");
    Field(x => x.DisplayName)
      .Description("The display name of the customization.");
    Field(x => x.Description)
      .Description("A textual description of the customization. It may contain Markdown and HTML.");
  }
}
