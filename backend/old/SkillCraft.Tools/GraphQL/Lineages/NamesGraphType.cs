using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class NamesGraphType : ObjectGraphType<NamesModel>
{
  public NamesGraphType()
  {
    Name = "Names";
    Description = "Represents the names of a lineage.";

    Field(x => x.Text)
      .Description("A textual description of the lineage names.");
    Field(x => x.Family)
      .Description("A list of family names.");
    Field(x => x.Female)
      .Description("A list of female names.");
    Field(x => x.Male)
      .Description("A list of male names.");
    Field(x => x.Unisex)
      .Description("A list of unisex names.");
    Field(x => x.Custom, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<NameCategoryGraphType>>>))
      .Description("The list of custom name categories.");
  }
}
