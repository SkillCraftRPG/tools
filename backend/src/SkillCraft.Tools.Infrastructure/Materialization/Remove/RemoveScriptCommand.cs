using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveScriptCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveScriptCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveScriptCommandHandler : IRequestHandler<RemoveScriptCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveScriptCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveScriptCommand command, CancellationToken cancellationToken)
  {
    ScriptEntity? script = await _context.Scripts
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (script != null)
    {
      _context.Scripts.Remove(script);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
