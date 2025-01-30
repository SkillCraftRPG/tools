using Logitar.Cms.Core;
using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.Core.Specializations.Models;

public class SpecializationModel : AggregateModel
{
  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public TalentModel? RequiredTalent { get; set; }
  public List<string> OtherRequirements { get; set; } = [];

  public List<TalentModel> OptionalTalents { get; set; } = [];
  public List<string> OtherOptions { get; set; } = [];

  public ReservedTalentModel? ReservedTalent { get; set; }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
