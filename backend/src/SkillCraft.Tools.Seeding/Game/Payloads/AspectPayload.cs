namespace SkillCraft.Tools.Seeding.Game.Payloads;

public record AspectPayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public AttributeSelection Attributes { get; set; } = new();
  public SkillSelection Skills { get; set; } = new();
}

public record AttributeSelection
{
  public GameAttribute? Mandatory1 { get; set; }
  public GameAttribute? Mandatory2 { get; set; }
  public GameAttribute? Optional1 { get; set; }
  public GameAttribute? Optional2 { get; set; }
}

public record SkillSelection
{
  public Skill? Discounted1 { get; set; }
  public Skill? Discounted2 { get; set; }
}
