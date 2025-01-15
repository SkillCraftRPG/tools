using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Specializations;

namespace SkillCraft.Tools.Infrastructure.Repositories;

internal class SpecializationRepository : Repository, ISpecializationRepository
{
  public SpecializationRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Specialization?> LoadAsync(SpecializationId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Specialization?> LoadAsync(SpecializationId id, long? version, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Specialization>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Specialization>> LoadAsync(CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Specialization>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Specialization>> LoadAsync(IEnumerable<SpecializationId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await base.LoadAsync<Specialization>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Specialization specialization, CancellationToken cancellationToken)
  {
    await base.SaveAsync(specialization, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Specialization> specializations, CancellationToken cancellationToken)
  {
    await base.SaveAsync(specializations, cancellationToken);
  }
}
