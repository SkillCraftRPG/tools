using Logitar.Portal.Contracts;

namespace SkillCraft.Tools.Core.Customizations.Models;

public class CustomizationModel : AggregateModel
{
  public CustomizationType Type { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
