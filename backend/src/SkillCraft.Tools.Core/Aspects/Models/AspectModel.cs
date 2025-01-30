using Logitar.Cms.Core;

namespace SkillCraft.Tools.Core.Aspects.Models;

public class AspectModel : AggregateModel
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public AttributeSelectionModel Attributes { get; set; } = new();
  public SkillSelectionModel Skills { get; set; } = new();

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
