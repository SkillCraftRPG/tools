namespace SkillCraft.Tools.Core.Castes.Models;

public class FeatureModel
{
  public Guid Id { get; set; }

  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }

  public override bool Equals(object? obj) => obj is FeatureModel feature && feature.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString() => $"{Name} | {GetType()} (Id={Id})";
}
