using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;

namespace SkillCraft.Tools.Core.Caching;

public interface ICacheService
{
  ActorModel? GetActor(ActorId id);
  void RemoveActor(ActorId id);
  void SetActor(ActorModel actor);
}
