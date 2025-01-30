using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveTalentCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveTalentCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveTalentCommandHandler : IRequestHandler<RemoveTalentCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveTalentCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveTalentCommand command, CancellationToken cancellationToken)
  {
    TalentEntity? talent = await _context.Talents
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (talent != null)
    {
      _context.Talents.Remove(talent);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
