using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Talents;

namespace SkillCraft.Tools.Infrastructure.Repositories;

internal class TalentRepository : Repository, ITalentRepository
{
  public TalentRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Talent?> LoadAsync(TalentId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Talent?> LoadAsync(TalentId id, long? version, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Talent>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Talent>> LoadAsync(CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Talent>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Talent>> LoadAsync(IEnumerable<TalentId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await base.LoadAsync<Talent>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Talent talent, CancellationToken cancellationToken)
  {
    await base.SaveAsync(talent, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Talent> talents, CancellationToken cancellationToken)
  {
    await base.SaveAsync(talents, cancellationToken);
  }
}
