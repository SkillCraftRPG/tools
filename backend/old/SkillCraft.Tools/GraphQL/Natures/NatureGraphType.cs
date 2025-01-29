using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.GraphQL.Customizations;

namespace SkillCraft.Tools.GraphQL.Natures;

internal class NatureGraphType : AggregateGraphType<NatureModel>
{
  public NatureGraphType() : base("Represents a character nature.")
  {
    Field(x => x.UniqueSlug)
      .Description("The unique slug of the nature.");
    Field(x => x.DisplayName)
      .Description("The display name of the nature.");
    Field(x => x.Description)
      .Description("A textual description of the nature. It may contain Markdown and HTML.");

    Field(x => x.Attribute, type: typeof(AttributeGraphType))
      .Description("The attribute to which this nature grants a +1 bonus.");
    Field(x => x.Gift, type: typeof(CustomizationGraphType))
      .Description("The gift granted to characters with this nature.");
  }
}
