namespace SkillCraft.Tools.Core.Aspects;

public interface IAspectManager
{
  Task SaveAsync(Aspect aspect, CancellationToken cancellationToken = default);
}
