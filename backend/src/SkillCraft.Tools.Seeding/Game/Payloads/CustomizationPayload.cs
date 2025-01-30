using SkillCraft.Tools.Core.Customizations;

namespace SkillCraft.Tools.Seeding.Game.Payloads;

public record CustomizationPayload
{
  public Guid Id { get; set; }

  public CustomizationType Type { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }
}
