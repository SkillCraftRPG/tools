using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveLanguageCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveLanguageCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveLanguageCommandHandler : IRequestHandler<RemoveLanguageCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveLanguageCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveLanguageCommand command, CancellationToken cancellationToken)
  {
    LanguageEntity? language = await _context.Languages
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (language != null)
    {
      _context.Languages.Remove(language);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
