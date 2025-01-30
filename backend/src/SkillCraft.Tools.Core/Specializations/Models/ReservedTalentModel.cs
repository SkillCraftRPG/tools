namespace SkillCraft.Tools.Core.Specializations.Models;

public record ReservedTalentModel
{
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }

  public ReservedTalentModel()
  {
  }

  public ReservedTalentModel(string name, string? description = null)
  {
    Name = name;
    Description = description;
  }
}
