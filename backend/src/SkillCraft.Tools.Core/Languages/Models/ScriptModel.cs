namespace SkillCraft.Tools.Core.Languages.Models;

public class ScriptModel
{
  public Guid Id { get; set; }

  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }

  public override bool Equals(object? obj) => obj is ScriptModel script && script.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString() => $"{Name} | {GetType()} (Id={Id})";
}
