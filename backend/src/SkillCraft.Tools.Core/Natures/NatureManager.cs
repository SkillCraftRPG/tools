using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Natures.Events;

namespace SkillCraft.Tools.Core.Natures;

internal class NatureManager : INatureManager
{
  private readonly INatureQuerier _natureQuerier;
  private readonly INatureRepository _natureRepository;

  public NatureManager(INatureQuerier natureQuerier, INatureRepository natureRepository)
  {
    _natureQuerier = natureQuerier;
    _natureRepository = natureRepository;
  }

  public async Task SaveAsync(Nature nature, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in nature.Changes)
    {
      if (change is NatureCreated created)
      {
        uniqueSlug = created.UniqueSlug;
      }
      else if (change is NatureUpdated updated && updated.UniqueSlug != null)
      {
        uniqueSlug = updated.UniqueSlug;
      }
    }

    if (uniqueSlug != null)
    {
      NatureId? conflictId = await _natureQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(nature.Id))
      {
        throw new UniqueSlugAlreadyUsedException(nature, conflictId.Value);
      }
    }

    await _natureRepository.SaveAsync(nature, cancellationToken);
  }
}
