using GraphQL;
using GraphQL.Types;
using SkillCraft.Tools.Core.Specializations.Queries;

namespace SkillCraft.Tools.GraphQL.Specializations;

internal static class SpecializationQueries
{
  public static void Register(RootQuery root)
  {
    root.Field<SpecializationGraphType>("specialization")
      //.Authorize() // ISSUE: https://github.com/SkillCraftRPG/tools/issues/5
      .Description("Retrieves a single specialization.")
      .Arguments(
        new QueryArgument<IdGraphType>() { Name = "id", Description = "The unique identifier of the specialization." },
        new QueryArgument<StringGraphType>() { Name = "slug", Description = "The unique slug of the specialization." })
      .ResolveAsync(async context => await context.ExecuteAsync(new ReadSpecializationQuery(
        context.GetArgument<Guid?>("id"),
        context.GetArgument<string?>("slug"))));
  }
}
