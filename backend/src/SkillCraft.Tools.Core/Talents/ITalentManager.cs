namespace SkillCraft.Tools.Core.Talents;

public interface ITalentManager
{
  Task SaveAsync(Talent talent, CancellationToken cancellationToken = default);
}
