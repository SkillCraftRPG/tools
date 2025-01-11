using Logitar.EventSourcing;
using Microsoft.Extensions.Caching.Memory;
using SkillCraft.Tools.Core.Actors.Models;
using SkillCraft.Tools.Core.Caching;
using SkillCraft.Tools.Infrastructure.Settings;

namespace SkillCraft.Tools.Infrastructure.Caching;

internal class CacheService : ICacheService
{
  private readonly IMemoryCache _cache;
  private readonly CachingSettings _settings;

  public CacheService(IMemoryCache cache, CachingSettings settings)
  {
    _cache = cache;
    _settings = settings;
  }

  public ActorModel? GetActor(ActorId id)
  {
    string key = GetActorKey(id);
    return Get<ActorModel>(key);
  }
  public void RemoveActor(ActorId id)
  {
    string key = GetActorKey(id);
    Remove(key);
  }
  public void SetActor(ActorModel actor)
  {
    ActorId id = new(actor.Id);
    string key = GetActorKey(id);
    Set(key, actor, _settings.ActorLifetime);
  }
  private static string GetActorKey(ActorId id) => $"Actor.Id:{id}";

  private T? Get<T>(object key) => _cache.TryGetValue(key, out object? value) ? (T?)value : default;
  private void Remove(object key) => _cache.Remove(key);
  private void Set<T>(object key, T value, TimeSpan? lifetime = null)
  {
    if (lifetime.HasValue)
    {
      _cache.Set(key, value, lifetime.Value);
    }
    else
    {
      _cache.Set(key, value);
    }
  }
}
