using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.GraphQL.Languages;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class LanguagesGraphType : ObjectGraphType<LanguagesModel>
{
  public LanguagesGraphType()
  {
    Name = "Languages";
    Description = "Represents the languages spoken by a lineage.";

    Field(x => x.Items, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<LanguageGraphType>>>))
      .Description("The spoken languages imposed by this lineage.");
    Field(x => x.Extra)
      .Description("The number of languages chosen by the player.");
    Field(x => x.Text)
      .Description("A textual description of the languages spoken.");
  }
}
