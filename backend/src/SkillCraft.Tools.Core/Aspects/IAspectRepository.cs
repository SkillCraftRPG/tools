namespace SkillCraft.Tools.Core.Aspects;

public interface IAspectRepository
{
  Task<Aspect?> LoadAsync(AspectId id, CancellationToken cancellationToken = default);
  Task<Aspect?> LoadAsync(AspectId id, long? version, CancellationToken cancellationToken = default);

  Task<IReadOnlyCollection<Aspect>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<Aspect>> LoadAsync(IEnumerable<AspectId> ids, CancellationToken cancellationToken = default);

  Task SaveAsync(Aspect aspect, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<Aspect> aspects, CancellationToken cancellationToken = default);
}
