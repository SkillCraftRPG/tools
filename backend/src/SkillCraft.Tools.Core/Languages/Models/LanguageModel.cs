﻿using Logitar.Cms.Core;

namespace SkillCraft.Tools.Core.Languages.Models;

public class LanguageModel : AggregateModel
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public List<ScriptModel> Scripts { get; set; } = [];
  public string? TypicalSpeakers { get; set; }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
