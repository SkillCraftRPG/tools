using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Customizations.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class CustomizationEvents : INotificationHandler<CustomizationCreated>, INotificationHandler<CustomizationUpdated>
{
  private readonly SkillCraftContext _context;

  public CustomizationEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(CustomizationCreated @event, CancellationToken cancellationToken)
  {
    CustomizationEntity? customization = await _context.Customizations.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (customization == null)
    {
      customization = new(@event);

      _context.Customizations.Add(customization);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(CustomizationUpdated @event, CancellationToken cancellationToken)
  {
    CustomizationEntity? customization = await _context.Customizations
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (customization != null && customization.Version == (@event.Version - 1))
    {
      customization.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
