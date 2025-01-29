using Logitar.EventSourcing;
using Logitar.EventSourcing.Infrastructure;
using MediatR;
using SkillCraft.Tools.Core.Logging;

namespace SkillCraft.Tools.Infrastructure;

internal class EventBus : IEventBus
{
  private readonly ILoggingService _loggingService;
  private readonly IMediator _mediator;

  public EventBus(ILoggingService loggingService, IMediator mediator)
  {
    _loggingService = loggingService;
    _mediator = mediator;
  }

  public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
  {
    if (@event is IIdentifiableEvent identifiable)
    {
      _loggingService.Report(identifiable);
    }

    await _mediator.Publish(@event, cancellationToken);
  }
}
