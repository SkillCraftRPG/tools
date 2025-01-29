namespace SkillCraft.Tools.Seeding.Game.Payloads;

public record FeaturePayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }
}
