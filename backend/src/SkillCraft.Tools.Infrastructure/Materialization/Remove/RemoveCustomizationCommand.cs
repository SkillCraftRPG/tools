using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Remove;

public record RemoveCustomizationCommand : IRequest
{
  public ContentLocaleUnpublished Event { get; }

  public RemoveCustomizationCommand(ContentLocaleUnpublished @event)
  {
    Event = @event;
  }
}

internal class RemoveCustomizationCommandHandler : IRequestHandler<RemoveCustomizationCommand>
{
  private readonly SkillCraftContext _context;

  public RemoveCustomizationCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(RemoveCustomizationCommand command, CancellationToken cancellationToken)
  {
    CustomizationEntity? customization = await _context.Customizations
      .SingleOrDefaultAsync(x => x.StreamId == command.Event.StreamId.Value, cancellationToken);
    if (customization != null)
    {
      _context.Customizations.Remove(customization);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
