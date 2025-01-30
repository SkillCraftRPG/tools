using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.Seeding.Game.Payloads;

public record EducationPayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Skill? Skill { get; set; }
  public double? WealthMultiplier { get; set; }
}
