using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Actors.Models;

namespace SkillCraft.Tools.Core.Caching;

public interface ICacheService
{
  ActorModel? GetActor(ActorId id);
  void RemoveActor(ActorId id);
  void SetActor(ActorModel actor);
}
