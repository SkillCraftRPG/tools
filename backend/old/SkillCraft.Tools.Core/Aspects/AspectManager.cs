using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Aspects.Events;

namespace SkillCraft.Tools.Core.Aspects;

internal class AspectManager : IAspectManager
{
  private readonly IAspectQuerier _aspectQuerier;
  private readonly IAspectRepository _aspectRepository;

  public AspectManager(IAspectQuerier aspectQuerier, IAspectRepository aspectRepository)
  {
    _aspectQuerier = aspectQuerier;
    _aspectRepository = aspectRepository;
  }

  public async Task SaveAsync(Aspect aspect, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in aspect.Changes)
    {
      if (change is AspectCreated created)
      {
        uniqueSlug = created.UniqueSlug;
      }
      else if (change is AspectUpdated updated && updated.UniqueSlug != null)
      {
        uniqueSlug = updated.UniqueSlug;
      }
    }

    if (uniqueSlug != null)
    {
      AspectId? conflictId = await _aspectQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(aspect.Id))
      {
        throw new UniqueSlugAlreadyUsedException(aspect, conflictId.Value);
      }
    }

    await _aspectRepository.SaveAsync(aspect, cancellationToken);
  }
}
