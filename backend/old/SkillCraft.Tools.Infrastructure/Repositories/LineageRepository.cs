using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Lineages;

namespace SkillCraft.Tools.Infrastructure.Repositories;

internal class LineageRepository : Repository, ILineageRepository
{
  public LineageRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Lineage?> LoadAsync(LineageId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Lineage?> LoadAsync(LineageId id, long? version, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Lineage>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Lineage>> LoadAsync(CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Lineage>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Lineage>> LoadAsync(IEnumerable<LineageId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await base.LoadAsync<Lineage>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Lineage lineage, CancellationToken cancellationToken)
  {
    await base.SaveAsync(lineage, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Lineage> lineages, CancellationToken cancellationToken)
  {
    await base.SaveAsync(lineages, cancellationToken);
  }
}
