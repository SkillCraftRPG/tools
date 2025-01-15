using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.Core.Castes.Models;

public record SearchCastesPayload : SearchPayload
{
  public Skill? Skill { get; set; }

  public new List<CasteSortOption> Sort { get; set; } = [];
}
