using FluentValidation;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Sessions;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Authentication;
using SkillCraft.Tools.Core.Identity;
using SkillCraft.Tools.Extensions;
using SkillCraft.Tools.Models.Account;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Route("api")]
public class AccountController : ControllerBase
{
  private readonly IOpenAuthenticationService _openAuthenticationService;
  private readonly ISessionService _sessionService;

  public AccountController(IOpenAuthenticationService openAuthenticationService, ISessionService sessionService)
  {
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

    TokenResponse tokenResponse = await _openAuthenticationService.GetTokenResponseAsync(session, cancellationToken);
    return Ok(tokenResponse);
  }
}
