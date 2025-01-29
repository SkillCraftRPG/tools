using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Castes.Events;

namespace SkillCraft.Tools.Core.Castes;

internal class CasteManager : ICasteManager
{
  private readonly ICasteQuerier _casteQuerier;
  private readonly ICasteRepository _casteRepository;

  public CasteManager(ICasteQuerier casteQuerier, ICasteRepository casteRepository)
  {
    _casteQuerier = casteQuerier;
    _casteRepository = casteRepository;
  }

  public async Task SaveAsync(Caste caste, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in caste.Changes)
    {
      if (change is CasteCreated created)
      {
        uniqueSlug = created.UniqueSlug;
      }
      else if (change is CasteUpdated updated && updated.UniqueSlug != null)
      {
        uniqueSlug = updated.UniqueSlug;
      }
    }

    if (uniqueSlug != null)
    {
      CasteId? conflictId = await _casteQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(caste.Id))
      {
        throw new UniqueSlugAlreadyUsedException(caste, conflictId.Value);
      }
    }

    await _casteRepository.SaveAsync(caste, cancellationToken);
  }
}
