namespace SkillCraft.Tools.Models.Components;

public record ImageModel
{
  public string? Alt { get; set; }
  public int Height { get; set; }
  public bool IsCircle { get; set; }
  public bool IsFluid { get; set; }
  public bool IsRounded { get; set; }
  public bool IsThumbnail { get; set; }
  public string? Src { get; set; }
  public int Width { get; set; }
}
