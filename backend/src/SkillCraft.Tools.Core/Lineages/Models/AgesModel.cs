namespace SkillCraft.Tools.Core.Lineages.Models;

public record AgesModel : IAges
{
  public int? Adolescent { get; set; }
  public int? Adult { get; set; }
  public int? Mature { get; set; }
  public int? Venerable { get; set; }
}
