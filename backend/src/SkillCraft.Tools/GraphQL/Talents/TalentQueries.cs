using GraphQL;
using GraphQL.Types;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Core.Talents.Queries;

namespace SkillCraft.Tools.GraphQL.Talents;

internal static class TalentQueries
{
  public static void Register(RootQuery root)
  {
    root.Field<TalentGraphType>("talent")
      //.Authorize() // ISSUE: https://github.com/SkillCraftRPG/tools/issues/5
      .Description("Retrieves a single talent.")
      .Arguments(
        new QueryArgument<IdGraphType>() { Name = "id", Description = "The unique identifier of the talent." },
        new QueryArgument<StringGraphType>() { Name = "slug", Description = "The unique slug of the talent." })
      .ResolveAsync(async context => await context.ExecuteAsync(new ReadTalentQuery(
        context.GetArgument<Guid?>("id"),
        context.GetArgument<string?>("slug"))));

    root.Field<NonNullGraphType<TalentSearchResultsGraphType>>("talents")
      //.Authorize() // ISSUE: https://github.com/SkillCraftRPG/tools/issues/5
      .Description("Searches a list of talents.")
      .Arguments(
      new QueryArgument<NonNullGraphType<SearchTalentsPayloadGraphType>>() { Name = "payload", Description = "The talent search parameters." })
      .ResolveAsync(async context => await context.ExecuteAsync(new SearchTalentsQuery(
        context.GetArgument<SearchTalentsPayload>("payload"))));
  }
}
