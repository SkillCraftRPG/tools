using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Customizations;

public interface ICustomizationQuerier
{
  Task<CustomizationId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken = default);

  Task<CustomizationModel> ReadAsync(Customization customization, CancellationToken cancellationToken = default);
  Task<CustomizationModel?> ReadAsync(CustomizationId id, CancellationToken cancellationToken = default);
  Task<CustomizationModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<CustomizationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<CustomizationModel>> SearchAsync(SearchCustomizationsPayload payload, CancellationToken cancellationToken = default);
}
