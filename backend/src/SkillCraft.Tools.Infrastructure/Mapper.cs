using Logitar;
using Logitar.EventSourcing;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Actors.Models;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure;

internal class Mapper
{
  private readonly Dictionary<ActorId, ActorModel> _actors = [];
  private readonly ActorModel _system = new();

  public Mapper()
  {
  }

  public Mapper(IEnumerable<ActorModel> actors)
  {
    foreach (ActorModel actor in actors)
    {
      ActorId id = new(actor.Id);
      _actors[id] = actor;
    }
  }

  public TalentModel ToTalent(TalentEntity source)
  {
    TalentModel destination = new()
    {
      Tier = source.Tier,
      UniqueSlug = source.UniqueSlug,
      DisplayName = source.DisplayName,
      Description = source.Description,
      AllowMultiplePurchases = source.AllowMultiplePurchases,
      Skill = source.Skill
    };

    if (source.RequiredTalent != null)
    {
      destination.RequiredTalent = ToTalent(source.RequiredTalent);
    }

    MapAggregate(source, destination);

    return destination;
  }

  private void MapAggregate(AggregateEntity source, AggregateModel destination)
  {
    try
    {
      destination.Id = new StreamId(source.StreamId).ToGuid();
    }
    catch (Exception)
    {
    }
    destination.Version = source.Version;
    destination.CreatedBy = TryFindActor(source.CreatedBy) ?? _system;
    destination.CreatedOn = source.CreatedOn.AsUniversalTime();
    destination.UpdatedBy = TryFindActor(source.UpdatedBy) ?? _system;
    destination.UpdatedOn = source.UpdatedOn.AsUniversalTime();
  }

  private ActorModel? TryFindActor(string? id) => TryFindActor(id == null ? null : new ActorId(id));
  private ActorModel? TryFindActor(ActorId? id) => id.HasValue && _actors.TryGetValue(id.Value, out ActorModel? actor) ? actor : null;
}
