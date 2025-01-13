using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Languages.Events;

namespace SkillCraft.Tools.Core.Languages;

internal class LanguageManager : ILanguageManager
{
  private readonly ILanguageQuerier _languageQuerier;
  private readonly ILanguageRepository _languageRepository;

  public LanguageManager(ILanguageQuerier languageQuerier, ILanguageRepository languageRepository)
  {
    _languageQuerier = languageQuerier;
    _languageRepository = languageRepository;
  }

  public async Task SaveAsync(Language language, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in language.Changes)
    {
      if (change is LanguageCreated created)
      {
        uniqueSlug = created.UniqueSlug;
      }
      else if (change is LanguageUpdated updated && updated.UniqueSlug != null)
      {
        uniqueSlug = updated.UniqueSlug;
      }
    }

    if (uniqueSlug != null)
    {
      LanguageId? conflictId = await _languageQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(language.Id))
      {
        throw new UniqueSlugAlreadyUsedException(language, conflictId.Value);
      }
    }

    await _languageRepository.SaveAsync(language, cancellationToken);
  }
}
