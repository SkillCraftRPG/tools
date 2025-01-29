using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class AgesGraphType : ObjectGraphType<AgesModel>
{
  public AgesGraphType()
  {
    Name = "Ages";
    Description = "Represents the age categories of a lineage, in years.";

    Field(x => x.Adolescent)
      .Description(string.Empty);
    Field(x => x.Adult)
      .Description(string.Empty);
    Field(x => x.Mature)
      .Description(string.Empty);
    Field(x => x.Venerable)
      .Description(string.Empty);
  }
}
