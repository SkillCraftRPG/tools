using SkillCraft.Tools.Core;
using Attribute = SkillCraft.Tools.Core.Attribute;

namespace SkillCraft.Tools.Seeding.Game.Payloads;

internal record AspectPayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public AttributeSelection Attributes { get; set; } = new();
  public SkillSelection Skills { get; set; } = new();
}

internal record AttributeSelection
{
  public Attribute? Mandatory1 { get; set; }
  public Attribute? Mandatory2 { get; set; }
  public Attribute? Optional1 { get; set; }
  public Attribute? Optional2 { get; set; }
}

internal record SkillSelection
{
  public Skill? Discounted1 { get; set; }
  public Skill? Discounted2 { get; set; }
}
