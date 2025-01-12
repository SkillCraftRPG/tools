using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Tokens;
using Logitar.Security.Claims;
using SkillCraft.Tools.Core.Identity;
using SkillCraft.Tools.Settings;

namespace SkillCraft.Tools.Authentication;

internal class OpenAuthenticationService : IOpenAuthenticationService
{
  private readonly OpenAuthenticationSettings _settings;
  private readonly ISessionService _sessionService;
  private readonly ITokenService _tokenService;

  public OpenAuthenticationService(ISessionService sessionService, OpenAuthenticationSettings settings, ITokenService tokenService)
  {
    _sessionService = sessionService;
    _settings = settings;
    _tokenService = tokenService;
  }

  public async Task<SessionModel> ValidateAsync(string accessToken, CancellationToken cancellationToken)
  {
    ValidatedTokenModel validatedToken = await _tokenService.ValidateAsync(accessToken, _settings.AccessToken.TokenType, cancellationToken);

    ClaimModel[] claims = validatedToken.Claims.Where(x => x.Name == Rfc7519ClaimNames.SessionId).ToArray();
    if (claims.Length != 1)
    {
      StringBuilder message = new();
      message.Append("Exactly 1 '").Append(Rfc7519ClaimNames.SessionId).Append("' claim was expected; ");
      if (claims.Length < 1)
      {
        message.Append("none was");
      }
      else
      {
        message.Append(claims.Length).Append(" were");
      }
      message.AppendLine(" found.").Append("AccessToken: ").AppendLine(accessToken);
      throw new ArgumentException(message.ToString(), nameof(accessToken));
    }

    ClaimModel claim = claims.Single();
    if (!Guid.TryParse(claim.Value, out Guid sessionId))
    {
      StringBuilder message = new();
      message.Append("The value '").Append(claim.Value).Append("' is not a valid '").Append(Rfc7519ClaimNames.SessionId).AppendLine("' claim value.")
        .Append("AccessToken: ").AppendLine(accessToken);
      throw new ArgumentException(message.ToString(), nameof(accessToken));
    }

    SessionModel? session = await _sessionService.FindAsync(sessionId, cancellationToken);
    if (session == null)
    {
      StringBuilder message = new();
      message.Append("The session 'Id=").Append(sessionId).AppendLine("' could not be found.")
        .Append("AccessToken: ").AppendLine(accessToken);
      throw new ArgumentException(message.ToString(), nameof(accessToken));
    }

    return session;
  }
}
