using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Natures;

namespace SkillCraft.Tools.Infrastructure.Repositories;

internal class NatureRepository : Repository, INatureRepository
{
  public NatureRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Nature?> LoadAsync(NatureId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Nature?> LoadAsync(NatureId id, long? version, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Nature>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Nature>> LoadAsync(CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Nature>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Nature>> LoadAsync(IEnumerable<NatureId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await base.LoadAsync<Nature>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Nature nature, CancellationToken cancellationToken)
  {
    await base.SaveAsync(nature, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Nature> natures, CancellationToken cancellationToken)
  {
    await base.SaveAsync(natures, cancellationToken);
  }
}
