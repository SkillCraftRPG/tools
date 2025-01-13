using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Actors.Models;
using SkillCraft.Tools.Core.Caching;
using SkillCraft.Tools.Infrastructure.Entities;

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

  public async Task<IReadOnlyCollection<ActorModel>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken)
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

    if (missingIds.Count > 0)
    {
      ActorEntity[] entities = await _context.Actors.AsNoTracking()
        .Where(a => missingIds.Contains(a.Id))
        .ToArrayAsync(cancellationToken);

      foreach (ActorEntity entity in entities)
      {
        ActorModel actor = Mapper.ToActor(entity);
        ActorId id = new(entity.Id);

        actors[id] = actor;
        _cacheService.SetActor(actor);
      }
    }

    return actors.Values;
  }
}
