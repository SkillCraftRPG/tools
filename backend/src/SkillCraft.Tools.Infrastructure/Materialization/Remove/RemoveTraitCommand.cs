using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveTraitCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveTraitCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveTraitCommandHandler : IRequestHandler<RemoveTraitCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveTraitCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveTraitCommand command, CancellationToken cancellationToken)
  {
    TraitEntity? trait = await _context.Traits
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (trait != null)
    {
      _context.Traits.Remove(trait);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
