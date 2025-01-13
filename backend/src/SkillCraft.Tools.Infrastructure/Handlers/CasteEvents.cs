using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Castes.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class CasteEvents : INotificationHandler<CasteCreated>, INotificationHandler<CasteUpdated>
{
  private readonly SkillCraftContext _context;

  public CasteEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(CasteCreated @event, CancellationToken cancellationToken)
  {
    CasteEntity? caste = await _context.Castes.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (caste == null)
    {
      caste = new(@event);

      _context.Castes.Add(caste);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(CasteUpdated @event, CancellationToken cancellationToken)
  {
    CasteEntity? caste = await _context.Castes
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (caste != null && caste.Version == (@event.Version - 1))
    {
      caste.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
