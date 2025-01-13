﻿using GraphQL.Types;
using Logitar;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.GraphQL.Actors;

namespace SkillCraft.Tools.GraphQL;

internal abstract class AggregateGraphType<T> : ObjectGraphType<T> where T : AggregateModel
{
  protected AggregateGraphType(string? description = null)
  {
    Name = typeof(T).Name.Remove("Model");
    Description = description;

    Field(x => x.Id)
      .Description("The unique identifier of the resource.");
    Field(x => x.Version)
      .Description("The version counter of the resource.");

    Field(x => x.CreatedBy, type: typeof(NonNullGraphType<ActorGraphType>))
      .Description("The actor who created the resource.");
    Field(x => x.CreatedOn)
      .Description("The date and time when the resource was created.");

    Field(x => x.UpdatedBy, type: typeof(NonNullGraphType<ActorGraphType>))
      .Description("The actor who updated the resource lastly.");
    Field(x => x.UpdatedOn)
      .Description("The date and time when the resource was updated lastly.");
  }
}
