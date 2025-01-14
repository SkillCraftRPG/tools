using Logitar.Portal.Contracts.Search;
using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.Core.Talents;

public interface ITalentQuerier
{
  Task<TalentId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken = default);

  Task<TalentModel> ReadAsync(Talent talent, CancellationToken cancellationToken = default);
  Task<TalentModel?> ReadAsync(TalentId id, CancellationToken cancellationToken = default);
  Task<TalentModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<TalentModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<TalentModel>> SearchAsync(SearchTalentsPayload payload, CancellationToken cancellationToken = default);
}
