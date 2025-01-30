﻿using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization;

public record MaterializeLanguageCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeLanguageCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeLanguageCommandHandler : IRequestHandler<MaterializeLanguageCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeLanguageCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeLanguageCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    LanguageEntity? language = await _context.Languages
      .Include(x => x.Scripts)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (language == null)
    {
      language = new(command.Event);
      _context.Languages.Add(language);
    }
    else
    {
      language.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale == null)
    {
      language.Scripts.Clear();
      if (command.FieldValues.TryGetValue(Language.Scripts, out string? scriptsValue))
      {
        IEnumerable<Guid> scriptIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(scriptsValue) ?? [];
        ScriptEntity[] scripts = scriptIds.Any()
          ? await _context.Scripts.Where(script => scriptIds.Contains(script.Id)).ToArrayAsync(cancellationToken)
          : [];
        language.Scripts.AddRange(scripts);
      }
    }
    else
    {
      language.UniqueSlug = locale.UniqueName.Value;
      language.DisplayName = locale.DisplayName?.Value;
      language.Description = locale.Description?.Value;

      language.TypicalSpeakers = command.FieldValues.TryGetValue(Language.TypicalSpeakers, out string? typicalSpeakers) ? typicalSpeakers : null;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
