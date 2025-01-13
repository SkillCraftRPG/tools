using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Languages.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class LanguageEvents : INotificationHandler<LanguageCreated>, INotificationHandler<LanguageUpdated>
{
  private readonly SkillCraftContext _context;

  public LanguageEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(LanguageCreated @event, CancellationToken cancellationToken)
  {
    LanguageEntity? language = await _context.Languages.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (language == null)
    {
      language = new(@event);

      _context.Languages.Add(language);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(LanguageUpdated @event, CancellationToken cancellationToken)
  {
    LanguageEntity? language = await _context.Languages
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (language != null && language.Version == (@event.Version - 1))
    {
      language.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
