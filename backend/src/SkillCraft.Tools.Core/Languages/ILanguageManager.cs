namespace SkillCraft.Tools.Core.Languages;

public interface ILanguageManager
{
  Task SaveAsync(Language language, CancellationToken cancellationToken = default);
}
