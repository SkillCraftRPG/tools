namespace SkillCraft.Tools.Core.Lineages.Models;

public record SpeedsModel
{
  public int Walk { get; set; }
  public int Climb { get; set; }
  public int Swim { get; set; }
  public int Fly { get; set; }
  public int Hover { get; set; }
  public int Burrow { get; set; }
}
