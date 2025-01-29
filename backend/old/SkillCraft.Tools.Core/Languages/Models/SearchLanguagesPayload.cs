using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.Core.Languages.Models;

public record SearchLanguagesPayload : SearchPayload
{
  public string? Script { get; set; }

  public new List<LanguageSortOption> Sort { get; set; } = [];
}
