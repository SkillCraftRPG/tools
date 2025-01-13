using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Educations;

namespace SkillCraft.Tools.Infrastructure.Repositories;

internal class EducationRepository : Repository, IEducationRepository
{
  public EducationRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Education?> LoadAsync(EducationId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Education?> LoadAsync(EducationId id, long? version, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Education>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Education>> LoadAsync(CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Education>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Education>> LoadAsync(IEnumerable<EducationId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await base.LoadAsync<Education>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Education education, CancellationToken cancellationToken)
  {
    await base.SaveAsync(education, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Education> educations, CancellationToken cancellationToken)
  {
    await base.SaveAsync(educations, cancellationToken);
  }
}
