namespace SkillCraft.Tools.Seeding.Game.Payloads;

internal record SpecializationPayload
{
  public Guid Id { get; set; }

  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? RequiredTalent { get; set; }
  public List<string> OtherRequirements { get; set; } = [];
  public List<string> OptionalTalents { get; set; } = [];
  public List<string> OtherOptions { get; set; } = [];

  public ReservedTalent? ReservedTalent { get; set; }
}

public record ReservedTalent
{
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
}
