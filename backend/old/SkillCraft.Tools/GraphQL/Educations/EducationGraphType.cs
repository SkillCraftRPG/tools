using SkillCraft.Tools.Core.Educations.Models;

namespace SkillCraft.Tools.GraphQL.Educations;

internal class EducationGraphType : AggregateGraphType<EducationModel>
{
  public EducationGraphType() : base("Represents a character education.")
  {
    Field(x => x.UniqueSlug)
      .Description("The unique slug of the education.");
    Field(x => x.DisplayName)
      .Description("The display name of the education.");
    Field(x => x.Description)
      .Description("A textual description of the education. It may contain Markdown and HTML.");

    Field(x => x.Skill, type: typeof(SkillGraphType))
      .Description("The skill talent required by this education.");
    Field(x => x.WealthMultiplier)
      .Description("The starting wealth multiplier of characters having this education.");
  }
}
