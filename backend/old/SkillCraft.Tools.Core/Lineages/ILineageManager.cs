namespace SkillCraft.Tools.Core.Lineages;

public interface ILineageManager
{
  Task SaveAsync(Lineage lineage, CancellationToken cancellationToken = default);
}
