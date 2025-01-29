namespace SkillCraft.Tools.Core.Specializations.Models;

public record ReservedTalentModel
{
  public string Name { get; set; } = string.Empty;
  public List<string> Descriptions { get; set; } = [];

  public ReservedTalentModel()
  {
  }

  public ReservedTalentModel(string name, IEnumerable<string>? descriptions = null)
  {
    Name = name;

    if (descriptions != null)
    {
      Descriptions.AddRange(descriptions);
    }
  }
}
