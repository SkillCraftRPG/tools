﻿namespace SkillCraft.Tools.Core.Aspects.Models;

public record SkillSelectionModel : ISkillSelection
{
  public Skill? Discounted1 { get; set; }
  public Skill? Discounted2 { get; set; }
}