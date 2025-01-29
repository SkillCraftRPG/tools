using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;

namespace SkillCraft.Tools.Core.Actors;

public interface IActorService
{
  Task<IReadOnlyCollection<ActorModel>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken = default);
}
