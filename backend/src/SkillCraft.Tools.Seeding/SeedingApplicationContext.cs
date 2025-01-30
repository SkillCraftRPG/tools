using Logitar.Cms.Core;
using Logitar.EventSourcing;

namespace SkillCraft.Tools.Seeding;

internal class SeedingApplicationContext : IApplicationContext
{
  public ActorId? ActorId { get; set; }
}
