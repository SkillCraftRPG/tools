using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Aspects;

namespace SkillCraft.Tools.Infrastructure.Repositories;

internal class AspectRepository : Repository, IAspectRepository
{
  public AspectRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Aspect?> LoadAsync(AspectId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Aspect?> LoadAsync(AspectId id, long? version, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Aspect>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Aspect>> LoadAsync(CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Aspect>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Aspect>> LoadAsync(IEnumerable<AspectId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await base.LoadAsync<Aspect>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Aspect aspect, CancellationToken cancellationToken)
  {
    await base.SaveAsync(aspect, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Aspect> aspects, CancellationToken cancellationToken)
  {
    await base.SaveAsync(aspects, cancellationToken);
  }
}
