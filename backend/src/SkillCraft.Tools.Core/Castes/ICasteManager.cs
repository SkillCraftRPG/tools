namespace SkillCraft.Tools.Core.Castes;

public interface ICasteManager
{
  Task SaveAsync(Caste caste, CancellationToken cancellationToken = default);
}
