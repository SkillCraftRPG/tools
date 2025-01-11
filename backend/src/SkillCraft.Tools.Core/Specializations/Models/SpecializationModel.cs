namespace SkillCraft.Tools.Core.Specializations.Models;

public class SpecializationModel : AggregateModel
{
  public int Tier { get; set; }
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  // TODO(fpion): Requis
  // TODO(fpion): Talent obligatoire
  // TODO(fpion): Talents optionnels
  // TODO(fpion): Talent réservé
  // TODO(fpion): Contenu supplémentaire

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
