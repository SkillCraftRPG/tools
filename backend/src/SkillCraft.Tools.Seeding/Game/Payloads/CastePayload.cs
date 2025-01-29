namespace SkillCraft.Tools.Seeding.Game.Payloads;

public record CastePayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Skill? Skill { get; set; }
  public string? WealthRoll { get; set; }
  public string? Features { get; set; }
}
