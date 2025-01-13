using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.GraphQL.Aspects;

internal class AspectGraphType : AggregateGraphType<AspectModel>
{
  public AspectGraphType() : base("Represents a character aspect.")
  {
    Field(x => x.UniqueSlug)
      .Description("The unique slug of the aspect.");
    Field(x => x.DisplayName)
      .Description("The display name of the aspect.");
    Field(x => x.Description)
      .Description("A textual description of the aspect. It may contain Markdown and HTML.");

    // TODO(fpion): Attributes
    // TODO(fpion): Skills
  }
}
