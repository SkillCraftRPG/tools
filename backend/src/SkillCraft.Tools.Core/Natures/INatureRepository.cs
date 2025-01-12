namespace SkillCraft.Tools.Core.Natures;

public interface INatureRepository
{
  Task<Nature?> LoadAsync(NatureId id, CancellationToken cancellationToken = default);
  Task<Nature?> LoadAsync(NatureId id, long? version, CancellationToken cancellationToken = default);

  Task<IReadOnlyCollection<Nature>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<Nature>> LoadAsync(IEnumerable<NatureId> ids, CancellationToken cancellationToken = default);

  Task SaveAsync(Nature nature, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<Nature> natures, CancellationToken cancellationToken = default);
}
