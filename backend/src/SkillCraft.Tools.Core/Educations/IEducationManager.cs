namespace SkillCraft.Tools.Core.Educations;

public interface IEducationManager
{
  Task SaveAsync(Education education, CancellationToken cancellationToken = default);
}
