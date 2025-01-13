using GraphQL;
using GraphQL.Types;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Core.Languages.Queries;

namespace SkillCraft.Tools.GraphQL.Languages;

internal static class LanguageQueries
{
  public static void Register(RootQuery root)
  {
    root.Field<LanguageGraphType>("language")
      .Authorize()
      .Description("Retrieves a single language.")
      .Arguments(
        new QueryArgument<IdGraphType>() { Name = "id", Description = "The unique identifier of the language." },
        new QueryArgument<StringGraphType>() { Name = "slug", Description = "The unique slug of the language." })
      .ResolveAsync(async context => await context.ExecuteAsync(new ReadLanguageQuery(
        context.GetArgument<Guid?>("id"),
        context.GetArgument<string?>("slug"))));

    root.Field<NonNullGraphType<LanguageSearchResultsGraphType>>("languages")
      .Authorize()
      .Description("Searches a list of languages.")
      .Arguments(
      new QueryArgument<NonNullGraphType<SearchLanguagesPayloadGraphType>>() { Name = "payload", Description = "The language search parameters." })
      .ResolveAsync(async context => await context.ExecuteAsync(new SearchLanguagesQuery(
        context.GetArgument<SearchLanguagesPayload>("payload"))));
  }
}
