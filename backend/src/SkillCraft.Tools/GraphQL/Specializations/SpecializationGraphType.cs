using SkillCraft.Tools.Core.Specializations.Models;

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

    // TODO(fpion): RequiredTalent
    // TODO(fpion): OtherRequirements
    // TODO(fpion): OptionalTalents
    // TODO(fpion): OtherOptions
    Field(x => x.ReservedTalent, type: typeof(ReservedTalentGraphType))
      .Description("The reserved talent of the specialization.");
  }
}
