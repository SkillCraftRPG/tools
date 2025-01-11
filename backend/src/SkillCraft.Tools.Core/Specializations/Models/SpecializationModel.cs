using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.Core.Specializations.Models;

public class SpecializationModel : AggregateModel
{
  public int Tier { get; set; }
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  // TODO(fpion): Autres requis
  public TalentModel? RequiredTalent { get; set; }
  public List<TalentModel> OptionalTalents { get; set; } = [];
  // TODO(fpion): Autres options
  // TODO(fpion): Talent réservé

  // TODO(fpion): Arbres de spécialisations
  // TODO(fpion): Contenu supplémentaire (annexes)

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
