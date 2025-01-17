namespace SkillCraft.Tools.Core.Lineages.Models;

public record CreateOrReplaceLineagePayload
{
  public Guid? ParentId { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public AttributeBonusesModel Attributes { get; set; } = new();
  public List<TraitPayload> Traits { get; set; } = [];

  public LanguagesPayload Languages { get; set; } = new();
  public NamesModel Names { get; set; } = new();

  public SpeedsModel Speeds { get; set; } = new();
  public SizeModel Size { get; set; } = new();
  public WeightModel Weight { get; set; } = new();
  public AgesModel Ages { get; set; } = new();
}
