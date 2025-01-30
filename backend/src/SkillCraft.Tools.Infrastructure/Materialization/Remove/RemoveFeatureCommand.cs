using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveFeatureCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveFeatureCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveFeatureCommandHandler : IRequestHandler<RemoveFeatureCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveFeatureCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveFeatureCommand command, CancellationToken cancellationToken)
  {
    FeatureEntity? feature = await _context.Features
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (feature != null)
    {
      _context.Features.Remove(feature);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
