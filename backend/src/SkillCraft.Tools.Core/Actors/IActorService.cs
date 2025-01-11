using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Actors.Models;

namespace SkillCraft.Tools.Core.Actors;

public interface IActorService
{
  Task<IReadOnlyCollection<ActorModel>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken = default);
}
