using Logitar.EventSourcing;

namespace SkillCraft.Tools.Core;

public interface IApplicationContext
{
  ActorId? ActorId { get; }
}
