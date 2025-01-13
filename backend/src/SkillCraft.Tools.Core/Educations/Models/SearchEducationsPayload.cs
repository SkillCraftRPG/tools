using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Educations.Models;

public record SearchEducationsPayload : SearchPayload
{
  public Skill? Skill { get; set; }

  public new List<EducationSortOption> Sort { get; set; } = [];
}
