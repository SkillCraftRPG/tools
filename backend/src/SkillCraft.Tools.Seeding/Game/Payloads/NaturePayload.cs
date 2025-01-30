using Attribute = SkillCraft.Tools.Core.Attribute;

namespace SkillCraft.Tools.Seeding.Game.Payloads;

public record NaturePayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Attribute? Attribute { get; set; }
  public string? Gift { get; set; }
}
