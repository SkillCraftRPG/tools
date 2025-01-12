using GraphQL;
using GraphQL.Types;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.Core.Customizations.Queries;

namespace SkillCraft.Tools.GraphQL.Customizations;

internal static class CustomizationQueries
{
  public static void Register(RootQuery root)
  {
    root.Field<CustomizationGraphType>("customization")
      .Authorize()
      .Description("Retrieves a single customization.")
      .Arguments(
        new QueryArgument<IdGraphType>() { Name = "id", Description = "The unique identifier of the customization." },
        new QueryArgument<StringGraphType>() { Name = "slug", Description = "The unique slug of the customization." })
      .ResolveAsync(async context => await context.ExecuteAsync(new ReadCustomizationQuery(
        context.GetArgument<Guid?>("id"),
        context.GetArgument<string?>("slug"))));

    root.Field<NonNullGraphType<CustomizationSearchResultsGraphType>>("customizations")
      .Authorize()
      .Description("Searches a list of customizations.")
      .Arguments(
      new QueryArgument<NonNullGraphType<SearchCustomizationsPayloadGraphType>>() { Name = "payload", Description = "The customization search parameters." })
      .ResolveAsync(async context => await context.ExecuteAsync(new SearchCustomizationsQuery(
        context.GetArgument<SearchCustomizationsPayload>("payload"))));
  }
}
