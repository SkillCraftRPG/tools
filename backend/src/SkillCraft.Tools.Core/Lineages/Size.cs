namespace SkillCraft.Tools.Core.Lineages;

public record Size
{
  public SizeCategory Category { get; }
  public Roll? Roll { get; }

  public Size()
  {
  }

  [JsonConstructor]
  public Size(SizeCategory category, Roll? roll)
  {
    if (!Enum.IsDefined(category))
    {
      throw new ArgumentOutOfRangeException(nameof(category));
    }

    Category = category;
    Roll = roll;
  }
}
