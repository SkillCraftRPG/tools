using Logitar.Cms.Core.Search;
using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.Core.Castes;

public interface ICasteQuerier
{
  Task<CasteModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<CasteModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<CasteModel>> SearchAsync(SearchCastesPayload payload, CancellationToken cancellationToken = default);
}
