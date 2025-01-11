using GraphQL.Types;
using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.GraphQL.Talents;

internal class TalentGraphType : AggregateGraphType<TalentModel>
{
  public TalentGraphType() : base("Represents a character talent.")
  {
    Field(x => x.Tier)
      .Description("The tier of the talent. This ranges from 0 to 3.");
    Field(x => x.UniqueSlug)
      .Description("The unique slug of the talent.");
    Field(x => x.DisplayName)
      .Description("The display name of the talent.");
    Field(x => x.Description)
      .Description("A textual description of the talent. It may contain Markdown and HTML.");

    Field(x => x.AllowMultiplePurchases)
      .Description("A value indicating whether or not this talent may be purchased multiple times.");
    Field(x => x.Skill, type: typeof(SkillGraphType))
      .Description("The skill this talent grants training to.");

    Field(x => x.RequiredTalent, type: typeof(TalentGraphType))
      .Description("The talent required by this talent.");
    Field(x => x.RequiringTalents, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<TalentGraphType>>>))
      .Description("The talents requiring this talent.");
  }
}
