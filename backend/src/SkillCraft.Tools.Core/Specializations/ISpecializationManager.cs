namespace SkillCraft.Tools.Core.Specializations;

public interface ISpecializationManager
{
  Task SaveAsync(Specialization specialization, CancellationToken cancellationToken = default);
}
