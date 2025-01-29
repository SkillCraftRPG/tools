using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SkillCraft.Tools.Core.Logging;

namespace SkillCraft.Tools.Filters;

internal class OperationLogging : ActionFilterAttribute
{
  private readonly ILoggingService _loggingService;

  public OperationLogging(ILoggingService loggingService)
  {
    _loggingService = loggingService;
  }

  public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
    {
      Operation operation = new(descriptor.ControllerName, descriptor.ActionName);
      _loggingService.SetOperation(operation);
    }

    await base.OnActionExecutionAsync(context, next);
  }
}
