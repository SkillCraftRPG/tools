﻿namespace SkillCraft.Tools.Core;

public abstract class AggregateModel
{
  public Guid Id { get; set; }

  public override bool Equals(object? obj) => obj is AggregateModel aggregate && aggregate.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString() => $"{GetType()} (Id={Id})";
}
