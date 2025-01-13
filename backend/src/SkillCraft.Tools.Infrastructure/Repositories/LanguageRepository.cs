﻿using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Languages;

namespace SkillCraft.Tools.Infrastructure.Repositories;

internal class LanguageRepository : Repository, ILanguageRepository
{
  public LanguageRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Language?> LoadAsync(LanguageId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Language?> LoadAsync(LanguageId id, long? version, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Language>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Language>> LoadAsync(CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Language>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Language>> LoadAsync(IEnumerable<LanguageId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await base.LoadAsync<Language>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Language language, CancellationToken cancellationToken)
  {
    await base.SaveAsync(language, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Language> languages, CancellationToken cancellationToken)
  {
    await base.SaveAsync(languages, cancellationToken);
  }
}
