using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Specializations.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class SpecializationEvents : INotificationHandler<SpecializationCreated>, INotificationHandler<SpecializationUpdated>
{
  private readonly SkillCraftContext _context;

  public SpecializationEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(SpecializationCreated @event, CancellationToken cancellationToken)
  {
    SpecializationEntity? specialization = await _context.Specializations.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (specialization == null)
    {
      specialization = new(@event);

      _context.Specializations.Add(specialization);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(SpecializationUpdated @event, CancellationToken cancellationToken)
  {
    SpecializationEntity? specialization = await _context.Specializations
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (specialization != null && specialization.Version == (@event.Version - 1))
    {
      specialization.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
