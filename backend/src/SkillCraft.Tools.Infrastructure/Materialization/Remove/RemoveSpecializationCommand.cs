using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveSpecializationCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveSpecializationCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveSpecializationCommandHandler : IRequestHandler<RemoveSpecializationCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveSpecializationCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveSpecializationCommand command, CancellationToken cancellationToken)
  {
    SpecializationEntity? specialization = await _context.Specializations
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (specialization != null)
    {
      _context.Specializations.Remove(specialization);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
