using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveAspectCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveAspectCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveAspectCommandHandler : IRequestHandler<RemoveAspectCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveAspectCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveAspectCommand command, CancellationToken cancellationToken)
  {
    AspectEntity? aspect = await _context.Aspects
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (aspect != null)
    {
      _context.Aspects.Remove(aspect);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
