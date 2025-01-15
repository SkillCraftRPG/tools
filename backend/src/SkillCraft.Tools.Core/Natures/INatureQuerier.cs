using Logitar.Portal.Contracts.Search;
using SkillCraft.Tools.Core.Natures.Models;

namespace SkillCraft.Tools.Core.Natures;

public interface INatureQuerier
{
  Task<NatureId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken = default);

  Task<NatureModel> ReadAsync(Nature nature, CancellationToken cancellationToken = default);
  Task<NatureModel?> ReadAsync(NatureId id, CancellationToken cancellationToken = default);
  Task<NatureModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<NatureModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<NatureModel>> SearchAsync(SearchNaturesPayload payload, CancellationToken cancellationToken = default);
}
