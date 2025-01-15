using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.Core.Natures.Models;

public record SearchNaturesPayload : SearchPayload
{
  public Ability? Attribute { get; set; }
  public Guid? GiftId { get; set; }

  public new List<NatureSortOption> Sort { get; set; } = [];
}
