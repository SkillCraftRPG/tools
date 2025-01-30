using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveNatureCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveNatureCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveNatureCommandHandler : IRequestHandler<RemoveNatureCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveNatureCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveNatureCommand command, CancellationToken cancellationToken)
  {
    NatureEntity? nature = await _context.Natures
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (nature != null)
    {
      _context.Natures.Remove(nature);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
