namespace SkillCraft.Tools.Core.Natures;

public interface INatureManager
{
  Task SaveAsync(Nature nature, CancellationToken cancellationToken = default);
}
