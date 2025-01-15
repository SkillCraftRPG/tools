using GraphQL.Types;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.GraphQL.Talents;

namespace SkillCraft.Tools.GraphQL.Specializations;

internal class SpecializationGraphType : AggregateGraphType<SpecializationModel>
{
  public SpecializationGraphType() : base("Represents a character specialization.")
  {
    Field(x => x.Tier)
      .Description("The tier of the specialization. This ranges from 1 to 3.");

    Field(x => x.UniqueSlug)
      .Description("The unique slug of the specialization.");
    Field(x => x.DisplayName)
      .Description("The display name of the specialization.");
    Field(x => x.Description)
      .Description("A textual description of the specialization. It may contain Markdown and HTML.");

    Field(x => x.RequiredTalent, type: typeof(TalentGraphType))
      .Description("The talent required to acquire this specialization.");
    Field(x => x.OtherRequirements, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<StringGraphType>>>))
      .Description("The other requirements to acquire this specialization.");

    Field(x => x.OptionalTalents, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<TalentGraphType>>>))
      .Description("The optional talents to acquire this specialization.");
    Field(x => x.OtherOptions, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<StringGraphType>>>))
      .Description("The other options to acquire this specialization.");

    Field(x => x.ReservedTalent, type: typeof(ReservedTalentGraphType))
      .Description("The reserved talent of the specialization.");
  }
}
