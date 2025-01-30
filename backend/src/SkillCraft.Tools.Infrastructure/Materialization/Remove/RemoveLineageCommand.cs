using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveLineageCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveLineageCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveLineageCommandHandler : IRequestHandler<RemoveLineageCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveLineageCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveLineageCommand command, CancellationToken cancellationToken)
  {
    LineageEntity? lineage = await _context.Lineages
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (lineage != null)
    {
      _context.Lineages.Remove(lineage);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
