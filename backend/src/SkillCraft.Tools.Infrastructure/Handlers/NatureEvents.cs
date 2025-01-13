using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Natures.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class NatureEvents : INotificationHandler<NatureCreated>, INotificationHandler<NatureUpdated>
{
  private readonly SkillCraftContext _context;

  public NatureEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(NatureCreated @event, CancellationToken cancellationToken)
  {
    NatureEntity? nature = await _context.Natures.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (nature == null)
    {
      nature = new(@event);

      _context.Natures.Add(nature);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(NatureUpdated @event, CancellationToken cancellationToken)
  {
    NatureEntity? nature = await _context.Natures
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (nature != null && nature.Version == (@event.Version - 1))
    {
      CustomizationEntity? gift = null;
      if (@event.GiftId?.Value != null)
      {
        gift = await _context.Customizations
          .SingleOrDefaultAsync(x => x.StreamId == @event.GiftId.Value.Value.Value, cancellationToken)
          ?? throw new InvalidOperationException($"The customization entity 'StreamId={@event.GiftId.Value}' could not be found.");
      }

      nature.Update(gift, @event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
