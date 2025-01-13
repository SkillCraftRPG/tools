using GraphQL;
using GraphQL.Types;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.Core.Educations.Queries;

namespace SkillCraft.Tools.GraphQL.Educations;

internal static class EducationQueries
{
  public static void Register(RootQuery root)
  {
    root.Field<EducationGraphType>("education")
      .Authorize()
      .Description("Retrieves a single education.")
      .Arguments(
        new QueryArgument<IdGraphType>() { Name = "id", Description = "The unique identifier of the education." },
        new QueryArgument<StringGraphType>() { Name = "slug", Description = "The unique slug of the education." })
      .ResolveAsync(async context => await context.ExecuteAsync(new ReadEducationQuery(
        context.GetArgument<Guid?>("id"),
        context.GetArgument<string?>("slug"))));

    root.Field<NonNullGraphType<EducationSearchResultsGraphType>>("educations")
      .Authorize()
      .Description("Searches a list of educations.")
      .Arguments(
      new QueryArgument<NonNullGraphType<SearchEducationsPayloadGraphType>>() { Name = "payload", Description = "The education search parameters." })
      .ResolveAsync(async context => await context.ExecuteAsync(new SearchEducationsQuery(
        context.GetArgument<SearchEducationsPayload>("payload"))));
  }
}
