using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Actors.Models;
using SkillCraft.Tools.Core.Caching;

namespace SkillCraft.Tools.Infrastructure.Actors;

internal class ActorService : IActorService
{
  private readonly ICacheService _cacheService;
  private readonly SkillCraftContext _context;

  public ActorService(ICacheService cacheService, SkillCraftContext context)
  {
    _cacheService = cacheService;
    _context = context;
  }

  public Task<IReadOnlyCollection<ActorModel>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken)
  {
    int capacity = ids.Count();
    Dictionary<ActorId, ActorModel> actors = new(capacity);
    HashSet<Guid> missingIds = new(capacity);

    foreach (ActorId id in ids)
    {
      if (id != default)
      {
        ActorModel? actor = _cacheService.GetActor(id);
        if (actor == null)
        {
          missingIds.Add(id.ToGuid());
        }
        else
        {
          actors[id] = actor;
          _cacheService.SetActor(actor);
        }
      }
    }

    //if (missingIds.Count > 0)
    //{
    //  UserEntity[] users = await _context.Users.AsNoTracking()
    //    .Where(a => missingIds.Contains(a.Id))
    //    .ToArrayAsync(cancellationToken);

    //  foreach (UserEntity user in users)
    //  {
    //    ActorModel actor = Mapper.ToActor(user);
    //    ActorId id = new(user.Id);

    //    actors[id] = actor;
    //    _cacheService.SetActor(actor);
    //  }
    //} // ISSUE: https://github.com/SkillCraftRPG/tools/issues/5

    return Task.FromResult<IReadOnlyCollection<ActorModel>>(actors.Values);
  }
}
