using Logitar.Portal.Contracts.Sessions;
using SkillCraft.Tools.Models.Account;

namespace SkillCraft.Tools.Authentication;

public interface IOpenAuthenticationService
{
  Task<TokenResponse> GetTokenResponseAsync(SessionModel session, CancellationToken cancellationToken = default);
  Task<SessionModel> ValidateAsync(string accessToken, CancellationToken cancellationToken = default);
}
