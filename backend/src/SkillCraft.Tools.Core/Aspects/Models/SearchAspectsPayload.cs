using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Aspects.Models;

public record SearchAspectsPayload : SearchPayload
{
  public Ability? Attribute { get; set; }
  public Skill? Skill { get; set; }

  public new List<AspectSortOption> Sort { get; set; } = [];
}
