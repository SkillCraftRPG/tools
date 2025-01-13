using GraphQL;
using GraphQL.Types;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Core.Aspects.Queries;

namespace SkillCraft.Tools.GraphQL.Aspects;

internal static class AspectQueries
{
  public static void Register(RootQuery root)
  {
    root.Field<AspectGraphType>("aspect")
      .Authorize()
      .Description("Retrieves a single aspect.")
      .Arguments(
        new QueryArgument<IdGraphType>() { Name = "id", Description = "The unique identifier of the aspect." },
        new QueryArgument<StringGraphType>() { Name = "slug", Description = "The unique slug of the aspect." })
      .ResolveAsync(async context => await context.ExecuteAsync(new ReadAspectQuery(
        context.GetArgument<Guid?>("id"),
        context.GetArgument<string?>("slug"))));

    root.Field<NonNullGraphType<AspectSearchResultsGraphType>>("aspects")
      .Authorize()
      .Description("Searches a list of aspects.")
      .Arguments(
      new QueryArgument<NonNullGraphType<SearchAspectsPayloadGraphType>>() { Name = "payload", Description = "The aspect search parameters." })
      .ResolveAsync(async context => await context.ExecuteAsync(new SearchAspectsQuery(
        context.GetArgument<SearchAspectsPayload>("payload"))));
  }
}
