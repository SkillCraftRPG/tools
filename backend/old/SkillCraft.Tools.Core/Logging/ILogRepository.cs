namespace SkillCraft.Tools.Core.Logging;

public interface ILogRepository
{
  Task SaveAsync(Log log, CancellationToken cancellationToken = default);
}
