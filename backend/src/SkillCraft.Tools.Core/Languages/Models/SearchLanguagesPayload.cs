using Logitar.Cms.Core.Search;

namespace SkillCraft.Tools.Core.Languages.Models;

public record SearchLanguagesPayload : SearchPayload
{
  public Guid? ScriptId { get; set; }

  public new List<LanguageSortOption> Sort { get; set; } = [];
}
