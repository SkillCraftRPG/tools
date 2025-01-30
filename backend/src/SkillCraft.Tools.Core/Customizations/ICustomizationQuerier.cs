using Logitar.Cms.Core.Search;
using SkillCraft.Tools.Core.Customizations.Models;

namespace SkillCraft.Tools.Core.Customizations;

public interface ICustomizationQuerier
{
  Task<CustomizationModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<CustomizationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<CustomizationModel>> SearchAsync(SearchCustomizationsPayload payload, CancellationToken cancellationToken = default);
}
