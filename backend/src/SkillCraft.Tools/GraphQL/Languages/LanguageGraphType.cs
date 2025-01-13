using SkillCraft.Tools.Core.Languages.Models;

namespace SkillCraft.Tools.GraphQL.Languages;

internal class LanguageGraphType : AggregateGraphType<LanguageModel>
{
  public LanguageGraphType() : base("Represents a character language.")
  {
    Field(x => x.UniqueSlug)
      .Description("The unique slug of the language.");
    Field(x => x.DisplayName)
      .Description("The display name of the language.");
    Field(x => x.Description)
      .Description("A textual description of the language. It may contain Markdown and HTML.");

    Field(x => x.Script)
      .Description("The main script used to write the language. Some languages have no written form.");
    Field(x => x.TypicalSpeakers)
      .Description("The typical speakers of the language. Extinct languages have no speaker left.");
  }
}
