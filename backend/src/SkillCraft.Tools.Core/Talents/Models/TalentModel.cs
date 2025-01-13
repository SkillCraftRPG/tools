using Logitar.Portal.Contracts;

namespace SkillCraft.Tools.Core.Talents.Models;

public class TalentModel : AggregateModel
{
  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public bool AllowMultiplePurchases { get; set; }
  public Skill? Skill { get; set; }

  public TalentModel? RequiredTalent { get; set; }
  public List<TalentModel> RequiringTalents { get; set; } = [];

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
