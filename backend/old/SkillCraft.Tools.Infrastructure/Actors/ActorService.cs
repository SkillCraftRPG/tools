using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Users;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Caching;
using SkillCraft.Tools.Core.Identity;

namespace SkillCraft.Tools.Infrastructure.Actors;

internal class ActorService : IActorService
{
  private readonly IApiKeyService _apiKeyService;
  private readonly ICacheService _cacheService;
  private readonly IUserService _userService;

  public ActorService(IApiKeyService apiKeyService, ICacheService cacheService, IUserService userService)
  {
    _apiKeyService = apiKeyService;
    _cacheService = cacheService;
    _userService = userService;
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
      IReadOnlyCollection<UserModel> users = await _userService.FindAsync(missingIds, cancellationToken);
      foreach (UserModel user in users)
      {
        ActorModel actor = new(user);
        _cacheService.SetActor(actor);

        ActorId actorId = new(actor.Id);
        actors[actorId] = actor;

        missingIds.Remove(actor.Id);
      }
    }

    if (missingIds.Count > 0)
    {
      IReadOnlyCollection<ApiKeyModel> apiKeys = await _apiKeyService.FindAsync(missingIds, cancellationToken);
      foreach (ApiKeyModel apiKey in apiKeys)
      {
        ActorModel actor = new(apiKey);
        _cacheService.SetActor(actor);

        ActorId actorId = new(actor.Id);
        actors[actorId] = actor;

        missingIds.Remove(actor.Id);
      }
    }

    return actors.Values;
  }
}
