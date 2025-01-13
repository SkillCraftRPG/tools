using FluentValidation;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Sessions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Authentication;
using SkillCraft.Tools.Core.Identity;
using SkillCraft.Tools.Core.Identity.Events;
using SkillCraft.Tools.Extensions;
using SkillCraft.Tools.Models.Account;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Route("api")]
public class AccountController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IOpenAuthenticationService _openAuthenticationService;
  private readonly ISessionService _sessionService;

  public AccountController(IMediator mediator, IOpenAuthenticationService openAuthenticationService, ISessionService sessionService)
  {
    _mediator = mediator;
    _openAuthenticationService = openAuthenticationService;
    _sessionService = sessionService;
  }

  [HttpPost("auth/token")]
  public async Task<ActionResult<TokenResponse>> GetTokenAsync([FromBody] GetTokenPayload payload, CancellationToken cancellationToken)
  {
    new GetTokenValidator().ValidateAndThrow(payload);

    IReadOnlyCollection<CustomAttribute> customAttributes = HttpContext.GetSessionCustomAttributes();

    SessionModel session;
    if (!string.IsNullOrWhiteSpace(payload.RefreshToken))
    {
      session = await _sessionService.RenewAsync(payload.RefreshToken, customAttributes, cancellationToken);
    }
    else if (payload.Credentials != null)
    {
      session = await _sessionService.SignInAsync(payload.Credentials.UniqueName, payload.Credentials.Password, isPersistent: true, customAttributes, cancellationToken);
    }
    else
    {
      throw new ArgumentException("The payload validation succeeded, but the payload is not valid.", nameof(payload));
    }
    await _mediator.Publish(new UserAuthenticated(session.User));

    TokenResponse tokenResponse = await _openAuthenticationService.GetTokenResponseAsync(session, cancellationToken);
    return Ok(tokenResponse);
  }
}
