﻿using Logitar.EventSourcing;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.Worker;

internal class WorkerApplicationContext : IApplicationContext
{
  public ActorId? ActorId => null; // ISSUE: https://github.com/SkillCraftRPG/tools/issues/5
}
