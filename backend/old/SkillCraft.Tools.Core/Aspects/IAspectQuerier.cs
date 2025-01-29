using Logitar.Portal.Contracts.Search;
using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.Core.Aspects;

public interface IAspectQuerier
{
  Task<AspectId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken = default);

  Task<AspectModel> ReadAsync(Aspect aspect, CancellationToken cancellationToken = default);
  Task<AspectModel?> ReadAsync(AspectId id, CancellationToken cancellationToken = default);
  Task<AspectModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<AspectModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<AspectModel>> SearchAsync(SearchAspectsPayload payload, CancellationToken cancellationToken = default);
}
