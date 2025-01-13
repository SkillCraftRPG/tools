namespace SkillCraft.Tools.Core.Castes;

public interface ICasteRepository
{
  Task<Caste?> LoadAsync(CasteId id, CancellationToken cancellationToken = default);
  Task<Caste?> LoadAsync(CasteId id, long? version, CancellationToken cancellationToken = default);

  Task<IReadOnlyCollection<Caste>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<Caste>> LoadAsync(IEnumerable<CasteId> ids, CancellationToken cancellationToken = default);

  Task SaveAsync(Caste caste, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<Caste> castes, CancellationToken cancellationToken = default);
}
