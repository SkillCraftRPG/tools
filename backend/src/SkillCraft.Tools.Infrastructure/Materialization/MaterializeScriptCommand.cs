using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization;

public record MaterializeScriptCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeScriptCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeScriptCommandHandler : IRequestHandler<MaterializeScriptCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeScriptCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeScriptCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    ScriptEntity? script = await _context.Scripts
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (script == null)
    {
      script = new(command.Event);
      _context.Scripts.Add(script);
    }
    else
    {
      script.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale != null)
    {
      script.UniqueSlug = locale.UniqueName.Value;
      script.DisplayName = locale.DisplayName?.Value;
      script.Description = locale.Description?.Value;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
