using Logitar.Portal.Contracts.Roles;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Tokens;
using Logitar.Portal.Contracts.Users;
using Logitar.Security.Claims;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Identity;
using SkillCraft.Tools.Core.Identity.Models;
using SkillCraft.Tools.Models.Account;
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

  public async Task<TokenResponse> GetTokenResponseAsync(SessionModel session, CancellationToken cancellationToken)
  {
    IdentityModel identity = GetIdentityModel(session);
    CreatedTokenModel access = await _tokenService.CreateAsync(identity, _settings.AccessToken.Lifetime, _settings.AccessToken.TokenType, cancellationToken);

    return new TokenResponse
    {
      AccessToken = access.Token,
      TokenType = Schemes.Bearer,
      ExpiresIn = _settings.AccessToken.Lifetime,
      RefreshToken = session.RefreshToken
    };
  }
  private static IdentityModel GetIdentityModel(SessionModel session)
  {
    UserModel user = session.User;

    IdentityModel identity = new()
    {
      Subject = user.Id.ToString()
    };
    if (user.Email != null)
    {
      identity.Email = new EmailPayload(user.Email.Address, user.Email.IsVerified);
    }

    identity.Claims.Add(new ClaimModel(Rfc7519ClaimNames.Username, user.UniqueName));

    Claim updatedAt = ClaimHelper.Create(Rfc7519ClaimNames.UpdatedAt, user.UpdatedOn);
    identity.Claims.Add(new ClaimModel(updatedAt.Type, updatedAt.Value, updatedAt.ValueType));

    if (user.FullName != null)
    {
      if (user.FirstName != null)
      {
        identity.Claims.Add(new ClaimModel(Rfc7519ClaimNames.FirstName, user.FirstName));
      }
      if (user.MiddleName != null)
      {
        identity.Claims.Add(new ClaimModel(Rfc7519ClaimNames.MiddleName, user.MiddleName));
      }
      if (user.LastName != null)
      {
        identity.Claims.Add(new ClaimModel(Rfc7519ClaimNames.LastName, user.LastName));
      }
      identity.Claims.Add(new ClaimModel(Rfc7519ClaimNames.FullName, user.FullName));
    }

    if (user.Picture != null)
    {
      identity.Claims.Add(new ClaimModel(Rfc7519ClaimNames.Picture, user.Picture));
    }

    if (user.AuthenticatedOn.HasValue)
    {
      Claim authenticatedOn = ClaimHelper.Create(Rfc7519ClaimNames.AuthenticationTime, user.AuthenticatedOn.Value);
      identity.Claims.Add(new ClaimModel(authenticatedOn.Type, authenticatedOn.Value, authenticatedOn.ValueType));
    }

    foreach (RoleModel role in user.Roles)
    {
      identity.Claims.Add(new ClaimModel(Rfc7519ClaimNames.Roles, role.UniqueName));
    }

    identity.Claims.Add(new ClaimModel(Rfc7519ClaimNames.SessionId, session.Id.ToString()));

    return identity;
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
