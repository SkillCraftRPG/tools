using Logitar.Portal.Contracts;

namespace SkillCraft.Tools.Core.Lineages.Models;

public class LineageModel : AggregateModel
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public AttributeBonusesModel Attributes { get; set; } = new();
  public List<TraitModel> Traits { get; set; } = [];

  public LanguagesModel Languages { get; set; } = new();
  public NamesModel Names { get; set; } = new();

  public SpeedsModel Speeds { get; set; } = new();
  public SizeModel Size { get; set; } = new();
  public WeightModel Weight { get; set; } = new();
  public AgesModel Ages { get; set; } = new();

  public LineageModel? Parent { get; set; }
  public List<LineageModel> Children { get; set; } = [];

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
