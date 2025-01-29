using GraphQL;
using GraphQL.Types;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Core.Castes.Queries;

namespace SkillCraft.Tools.GraphQL.Castes;

internal static class CasteQueries
{
  public static void Register(RootQuery root)
  {
    root.Field<CasteGraphType>("caste")
      .Authorize()
      .Description("Retrieves a single caste.")
      .Arguments(
        new QueryArgument<IdGraphType>() { Name = "id", Description = "The unique identifier of the caste." },
        new QueryArgument<StringGraphType>() { Name = "slug", Description = "The unique slug of the caste." })
      .ResolveAsync(async context => await context.ExecuteAsync(new ReadCasteQuery(
        context.GetArgument<Guid?>("id"),
        context.GetArgument<string?>("slug"))));

    root.Field<NonNullGraphType<CasteSearchResultsGraphType>>("castes")
      .Authorize()
      .Description("Searches a list of castes.")
      .Arguments(
        new QueryArgument<NonNullGraphType<SearchCastesPayloadGraphType>>() { Name = "payload", Description = "The caste search parameters." })
      .ResolveAsync(async context => await context.ExecuteAsync(new SearchCastesQuery(
        context.GetArgument<SearchCastesPayload>("payload"))));
  }
}
