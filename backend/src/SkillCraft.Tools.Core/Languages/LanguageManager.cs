using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Languages.Events;

namespace SkillCraft.Tools.Core.Languages;

internal class LanguageManager : ILanguageManager
{
  private readonly ILanguageQuerier _casteQuerier;
  private readonly ILanguageRepository _casteRepository;

  public LanguageManager(ILanguageQuerier casteQuerier, ILanguageRepository casteRepository)
  {
    _casteQuerier = casteQuerier;
    _casteRepository = casteRepository;
  }

  public async Task SaveAsync(Language caste, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in caste.Changes)
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
      LanguageId? conflictId = await _casteQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(caste.Id))
      {
        throw new UniqueSlugAlreadyUsedException(caste, conflictId.Value);
      }
    }

    await _casteRepository.SaveAsync(caste, cancellationToken);
  }
}
