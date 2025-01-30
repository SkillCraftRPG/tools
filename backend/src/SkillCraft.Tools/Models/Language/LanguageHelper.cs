using SkillCraft.Tools.Core.Languages.Models;

namespace SkillCraft.Tools.Models.Language;

internal static class LanguageHelper
{
  public static string FormatScripts(LanguageModel language)
  {
    return language.Scripts.Count < 1 ? "—" : string.Join(", ", language.Scripts.Select(script => script.Name).OrderBy(name => name));
  }
}
