namespace SkillCraft.Tools.Core.Lineages;

public record Weight
{
  public Roll? Starved { get; }
  public Roll? Skinny { get; }
  public Roll? Normal { get; }
  public Roll? Overweight { get; }
  public Roll? Obese { get; }

  public Weight()
  {
  }

  [JsonConstructor]
  public Weight(Roll? starved, Roll? skinny, Roll? normal, Roll? overweight, Roll? obese)
  {
    Starved = starved;
    Skinny = skinny;
    Normal = normal;
    Overweight = overweight;
    Obese = obese;
  }
}
