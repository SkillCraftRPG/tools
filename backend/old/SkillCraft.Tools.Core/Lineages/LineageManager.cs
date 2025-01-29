using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Lineages.Events;

namespace SkillCraft.Tools.Core.Lineages;

internal class LineageManager : ILineageManager
{
  private readonly ILineageQuerier _lineageQuerier;
  private readonly ILineageRepository _lineageRepository;

  public LineageManager(ILineageQuerier lineageQuerier, ILineageRepository lineageRepository)
  {
    _lineageQuerier = lineageQuerier;
    _lineageRepository = lineageRepository;
  }

  public async Task SaveAsync(Lineage lineage, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in lineage.Changes)
    {
      if (change is LineageCreated created)
      {
        uniqueSlug = created.UniqueSlug;
      }
      else if (change is LineageUpdated updated && updated.UniqueSlug != null)
      {
        uniqueSlug = updated.UniqueSlug;
      }
    }

    if (uniqueSlug != null)
    {
      LineageId? conflictId = await _lineageQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(lineage.Id))
      {
        throw new UniqueSlugAlreadyUsedException(lineage, conflictId.Value);
      }
    }

    await _lineageRepository.SaveAsync(lineage, cancellationToken);
  }
}
