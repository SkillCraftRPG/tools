using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Talents.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class TalentEvents : INotificationHandler<TalentCreated>, INotificationHandler<TalentUpdated>
{
  private readonly SkillCraftContext _context;

  public TalentEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(TalentCreated @event, CancellationToken cancellationToken)
  {
    TalentEntity? talent = await _context.Talents.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (talent == null)
    {
      talent = new(@event);

      _context.Talents.Add(talent);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(TalentUpdated @event, CancellationToken cancellationToken)
  {
    TalentEntity? talent = await _context.Talents
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (talent != null && talent.Version == (@event.Version - 1))
    {
      TalentEntity? requiredTalent = null;
      if (@event.RequiredTalentId?.Value != null)
      {
        requiredTalent = await _context.Talents
          .SingleOrDefaultAsync(x => x.StreamId == @event.RequiredTalentId.Value.Value.Value, cancellationToken)
          ?? throw new InvalidOperationException($"The talent entity 'StreamId={@event.RequiredTalentId.Value}' could not be found.");
      }

      talent.Update(requiredTalent, @event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
