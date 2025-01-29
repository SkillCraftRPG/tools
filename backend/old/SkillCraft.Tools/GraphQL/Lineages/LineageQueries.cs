using GraphQL;
using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.Core.Lineages.Queries;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal static class LineageQueries
{
  public static void Register(RootQuery root)
  {
    root.Field<LineageGraphType>("lineage")
      .Authorize()
      .Description("Retrieves a single lineage.")
      .Arguments(
        new QueryArgument<IdGraphType>() { Name = "id", Description = "The unique identifier of the lineage." },
        new QueryArgument<StringGraphType>() { Name = "slug", Description = "The unique slug of the lineage." })
      .ResolveAsync(async context => await context.ExecuteAsync(new ReadLineageQuery(
        context.GetArgument<Guid?>("id"),
        context.GetArgument<string?>("slug"))));

    root.Field<NonNullGraphType<LineageSearchResultsGraphType>>("lineages")
      .Authorize()
      .Description("Searches a list of lineages.")
      .Arguments(
        new QueryArgument<NonNullGraphType<SearchLineagesPayloadGraphType>>() { Name = "payload", Description = "The lineage search parameters." })
      .ResolveAsync(async context => await context.ExecuteAsync(new SearchLineagesQuery(
        context.GetArgument<SearchLineagesPayload>("payload"))));
  }
}
