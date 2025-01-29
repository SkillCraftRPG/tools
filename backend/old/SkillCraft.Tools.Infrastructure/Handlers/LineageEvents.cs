using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Lineages.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class LineageEvents : INotificationHandler<LineageCreated>, INotificationHandler<LineageUpdated>
{
  private readonly SkillCraftContext _context;

  public LineageEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(LineageCreated @event, CancellationToken cancellationToken)
  {
    LineageEntity? lineage = await _context.Lineages.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (lineage == null)
    {
      LineageEntity? parent = null;
      if (@event.ParentId.HasValue)
      {
        parent = await _context.Lineages
          .SingleOrDefaultAsync(x => x.StreamId == @event.ParentId.Value.Value, cancellationToken)
          ?? throw new InvalidOperationException($"The lineage entity 'StreamId={@event.ParentId}' could not be found.");
      }

      lineage = new(parent, @event);

      _context.Lineages.Add(lineage);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(LineageUpdated @event, CancellationToken cancellationToken)
  {
    LineageEntity? lineage = await _context.Lineages
      .Include(x => x.Languages)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (lineage != null && lineage.Version == (@event.Version - 1))
    {
      IReadOnlyCollection<LanguageEntity> languages = [];
      if (@event.Languages != null)
      {
        HashSet<Guid> languageIds = @event.Languages.Ids.Select(id => id.ToGuid()).ToHashSet();
        languages = (await _context.Languages
          .Where(x => languageIds.Contains(x.Id))
          .ToListAsync(cancellationToken)).AsReadOnly();

        IEnumerable<Guid> missingLanguages = languageIds.Except(languages.Select(language => language.Id));
        if (missingLanguages.Any())
        {
          StringBuilder message = new();
          message.AppendLine("The specified language entities could not be found.");
          foreach (Guid id in missingLanguages)
          {
            message.Append(" - Id: ").Append(id).AppendLine();
          }
          throw new InvalidOperationException(message.ToString());
        }
      }

      lineage.Update(languages, @event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
