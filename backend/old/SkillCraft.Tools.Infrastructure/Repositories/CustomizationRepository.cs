using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Customizations;

namespace SkillCraft.Tools.Infrastructure.Repositories;

internal class CustomizationRepository : Repository, ICustomizationRepository
{
  public CustomizationRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Customization?> LoadAsync(CustomizationId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Customization?> LoadAsync(CustomizationId id, long? version, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Customization>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Customization>> LoadAsync(CancellationToken cancellationToken)
  {
    return await base.LoadAsync<Customization>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Customization>> LoadAsync(IEnumerable<CustomizationId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await base.LoadAsync<Customization>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Customization customization, CancellationToken cancellationToken)
  {
    await base.SaveAsync(customization, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Customization> customizations, CancellationToken cancellationToken)
  {
    await base.SaveAsync(customizations, cancellationToken);
  }
}
