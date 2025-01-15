using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.Core.Customizations.Models;

public record SearchCustomizationsPayload : SearchPayload
{
  public CustomizationType? Type { get; set; }

  public new List<CustomizationSortOption> Sort { get; set; } = [];
}
