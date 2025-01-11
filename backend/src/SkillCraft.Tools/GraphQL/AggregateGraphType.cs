using GraphQL.Types;
using Logitar;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.GraphQL;

internal abstract class AggregateGraphType<T> : ObjectGraphType<T> where T : AggregateModel
{
  protected AggregateGraphType(string? description = null)
  {
    Name = typeof(T).Name.Remove("Model");
    Description = description;

    Field(x => x.Id).Description("The unique identifier of the aggregate.");
  }
}
