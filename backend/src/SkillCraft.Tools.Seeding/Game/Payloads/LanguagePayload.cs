namespace SkillCraft.Tools.Seeding.Game.Payloads;

internal record LanguagePayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Script { get; set; }
  public string? TypicalSpeakers { get; set; }
}
