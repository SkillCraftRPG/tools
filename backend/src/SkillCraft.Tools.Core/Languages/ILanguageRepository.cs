﻿namespace SkillCraft.Tools.Core.Languages;

public interface ILanguageRepository
{
  Task<Language?> LoadAsync(LanguageId id, CancellationToken cancellationToken = default);
  Task<Language?> LoadAsync(LanguageId id, long? version, CancellationToken cancellationToken = default);

  Task<IReadOnlyCollection<Language>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<Language>> LoadAsync(IEnumerable<LanguageId> ids, CancellationToken cancellationToken = default);

  Task SaveAsync(Language caste, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<Language> castes, CancellationToken cancellationToken = default);
}
