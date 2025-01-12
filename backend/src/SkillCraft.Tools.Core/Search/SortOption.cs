﻿namespace SkillCraft.Tools.Core.Search;

public record SortOption
{
  public string Field { get; set; } = string.Empty;
  public bool IsDescending { get; set; }

  public SortOption()
  {
  }

  public SortOption(string field, bool isDescending = false)
  {
    Field = field;
  }
}
