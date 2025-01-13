namespace SkillCraft.Tools.Core.Languages;

public interface ILanguageManager
{
  Task SaveAsync(Language caste, CancellationToken cancellationToken = default);
}
