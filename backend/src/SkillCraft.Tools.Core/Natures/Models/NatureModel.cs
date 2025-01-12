using SkillCraft.Tools.Core.Customizations.Models;

namespace SkillCraft.Tools.Core.Natures.Models;

public class NatureModel : AggregateModel
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Ability? Attribute { get; set; }
  public CustomizationModel? Gift { get; set; }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
