﻿namespace SkillCraft.Tools.Seeding.Game.Payloads;

internal record ScriptPayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }
}
