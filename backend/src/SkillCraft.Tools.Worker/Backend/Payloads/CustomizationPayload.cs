﻿using SkillCraft.Tools.Core.Customizations.Models;

namespace SkillCraft.Tools.Worker.Backend.Payloads;

internal record CustomizationPayload : CreateOrReplaceCustomizationPayload
{
  public Guid Id { get; set; }
}
