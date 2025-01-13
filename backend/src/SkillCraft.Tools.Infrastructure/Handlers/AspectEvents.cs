using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Aspects.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class AspectEvents : INotificationHandler<AspectCreated>, INotificationHandler<AspectUpdated>
{
  private readonly SkillCraftContext _context;

  public AspectEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(AspectCreated @event, CancellationToken cancellationToken)
  {
    AspectEntity? aspect = await _context.Aspects.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (aspect == null)
    {
      aspect = new(@event);

      _context.Aspects.Add(aspect);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(AspectUpdated @event, CancellationToken cancellationToken)
  {
    AspectEntity? aspect = await _context.Aspects
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (aspect != null && aspect.Version == (@event.Version - 1))
    {
      aspect.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
