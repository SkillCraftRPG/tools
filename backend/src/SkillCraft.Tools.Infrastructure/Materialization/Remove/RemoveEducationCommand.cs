using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveEducationCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveEducationCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveEducationCommandHandler : IRequestHandler<RemoveEducationCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveEducationCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveEducationCommand command, CancellationToken cancellationToken)
  {
    EducationEntity? education = await _context.Educations
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (education != null)
    {
      _context.Educations.Remove(education);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
