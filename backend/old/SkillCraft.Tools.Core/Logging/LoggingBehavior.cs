using MediatR;

namespace SkillCraft.Tools.Core.Logging;

internal class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
  private readonly ILoggingService _loggingService;

  public LoggingBehavior(ILoggingService loggingService)
  {
    _loggingService = loggingService;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    if (request is IActivity activity)
    {
      _loggingService.SetActivity(activity);
    }

    return await next();
  }
}
