using Logitar.Cms.Core.Search;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Core.Lineages;

public interface ILineageQuerier
{
  Task<LineageModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<LineageModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<LineageModel>> SearchAsync(SearchLineagesPayload payload, CancellationToken cancellationToken = default);
}
