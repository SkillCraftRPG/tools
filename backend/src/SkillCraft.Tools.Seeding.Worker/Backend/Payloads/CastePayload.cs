﻿using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

internal record CastePayload : CreateOrReplaceCastePayload
{
  public Guid Id { get; set; }
}
