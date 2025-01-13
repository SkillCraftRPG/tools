using Logitar.EventSourcing;
using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Caching;
using SkillCraft.Tools.Core.Identity.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class IdentityEvents : INotificationHandler<ApiKeyAuthenticated>, INotificationHandler<UserAuthenticated>
{
  private readonly ICacheService _cacheService;
  private readonly SkillCraftContext _context;

  public IdentityEvents(ICacheService cacheService, SkillCraftContext context)
  {
    _cacheService = cacheService;
    _context = context;
  }

  public async Task Handle(ApiKeyAuthenticated @event, CancellationToken cancellationToken)
  {
    ApiKeyModel apiKey = @event.ApiKey;

    ActorEntity? actor = await _context.Actors.SingleOrDefaultAsync(x => x.Id == apiKey.Id, cancellationToken);
    if (actor == null)
    {
      actor = new(apiKey);

      _context.Actors.Add(actor);
    }
    else
    {
      actor.Update(apiKey);
    }

    await _context.SaveChangesAsync(cancellationToken);

    ActorId actorId = new(actor.Id);
    _cacheService.RemoveActor(actorId);
  }

  public async Task Handle(UserAuthenticated @event, CancellationToken cancellationToken)
  {
    UserModel user = @event.User;

    ActorEntity? actor = await _context.Actors.SingleOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
    if (actor == null)
    {
      actor = new(user);

      _context.Actors.Add(actor);
    }
    else
    {
      actor.Update(user);
    }

    await _context.SaveChangesAsync(cancellationToken);

    ActorId actorId = new(actor.Id);
    _cacheService.RemoveActor(actorId);
  }
}
