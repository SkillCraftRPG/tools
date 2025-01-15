using SkillCraft.Tools.Core.Logging;

namespace SkillCraft.Tools.Seeding.Worker;

internal class SeedingLogRepository : ILogRepository
{
  public Task SaveAsync(Log log, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
