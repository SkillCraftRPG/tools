using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Castes;

namespace SkillCraft.Tools.Infrastructure.Repositories;

internal class CasteRepository : Repository, ICasteRepository
{
  public CasteRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Caste?> LoadAsync(CasteId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Caste?> LoadAsync(CasteId id, long? version, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Caste>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Caste>> LoadAsync(CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Caste>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Caste>> LoadAsync(IEnumerable<CasteId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await base.LoadAsync<Caste>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Caste caste, CancellationToken cancellationToken)
  {
    await base.SaveAsync(caste, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Caste> castes, CancellationToken cancellationToken)
  {
    await base.SaveAsync(castes, cancellationToken);
  }
}
