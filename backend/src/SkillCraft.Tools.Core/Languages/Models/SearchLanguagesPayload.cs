using Logitar.Cms.Core.Search;

namespace SkillCraft.Tools.Core.Languages.Models;

public record SearchLanguagesPayload : SearchPayload
{
  public string? Script { get; set; }

  public new List<LanguageSortOption> Sort { get; set; } = [];
}
