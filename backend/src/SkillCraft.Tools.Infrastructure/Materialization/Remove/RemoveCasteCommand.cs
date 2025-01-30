using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveCasteCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveCasteCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveCasteCommandHandler : IRequestHandler<RemoveCasteCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveCasteCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveCasteCommand command, CancellationToken cancellationToken)
  {
    CasteEntity? caste = await _context.Castes
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (caste != null)
    {
      _context.Castes.Remove(caste);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
