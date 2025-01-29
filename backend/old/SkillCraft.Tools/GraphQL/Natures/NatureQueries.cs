using GraphQL;
using GraphQL.Types;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.Core.Natures.Queries;

namespace SkillCraft.Tools.GraphQL.Natures;

internal static class NatureQueries
{
  public static void Register(RootQuery root)
  {
    root.Field<NatureGraphType>("nature")
      .Authorize()
      .Description("Retrieves a single nature.")
      .Arguments(
        new QueryArgument<IdGraphType>() { Name = "id", Description = "The unique identifier of the nature." },
        new QueryArgument<StringGraphType>() { Name = "slug", Description = "The unique slug of the nature." })
      .ResolveAsync(async context => await context.ExecuteAsync(new ReadNatureQuery(
        context.GetArgument<Guid?>("id"),
        context.GetArgument<string?>("slug"))));

    root.Field<NonNullGraphType<NatureSearchResultsGraphType>>("natures")
      .Authorize()
      .Description("Searches a list of natures.")
      .Arguments(
        new QueryArgument<NonNullGraphType<SearchNaturesPayloadGraphType>>() { Name = "payload", Description = "The nature search parameters." })
      .ResolveAsync(async context => await context.ExecuteAsync(new SearchNaturesQuery(
        context.GetArgument<SearchNaturesPayload>("payload"))));
  }
}
