﻿using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Languages;

public interface ILanguageQuerier
{
  Task<LanguageId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken = default);

  Task<LanguageModel> ReadAsync(Language caste, CancellationToken cancellationToken = default);
  Task<LanguageModel?> ReadAsync(LanguageId id, CancellationToken cancellationToken = default);
  Task<LanguageModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<LanguageModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<LanguageModel>> SearchAsync(SearchLanguagesPayload payload, CancellationToken cancellationToken = default);
}
