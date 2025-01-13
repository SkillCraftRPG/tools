using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Educations.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class EducationEvents : INotificationHandler<EducationCreated>, INotificationHandler<EducationUpdated>
{
  private readonly SkillCraftContext _context;

  public EducationEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(EducationCreated @event, CancellationToken cancellationToken)
  {
    EducationEntity? education = await _context.Educations.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (education == null)
    {
      education = new(@event);

      _context.Educations.Add(education);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(EducationUpdated @event, CancellationToken cancellationToken)
  {
    EducationEntity? education = await _context.Educations
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (education != null && education.Version == (@event.Version - 1))
    {
      education.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
