using GraphQL.Types;
using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.GraphQL.Specializations;

internal class ReservedTalentGraphType : ObjectGraphType<ReservedTalentModel>
{
  public ReservedTalentGraphType()
  {
    Name = "ReservedTalent";
    Description = "Represents the reserved talent of a specialization.";

    Field(x => x.Name)
      .Description("The name of the reserved talent.");
    Field(x => x.Descriptions, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<StringGraphType>>>))
      .Description("The textual descriptions of the reserved talent.");
  }
}
