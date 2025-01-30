using Logitar.Cms.Core.Search;
using SkillCraft.Tools.Core.Languages.Models;

namespace SkillCraft.Tools.Core.Languages;

public interface ILanguageQuerier
{
  Task<LanguageModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<LanguageModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<LanguageModel>> SearchAsync(SearchLanguagesPayload payload, CancellationToken cancellationToken = default);
}
