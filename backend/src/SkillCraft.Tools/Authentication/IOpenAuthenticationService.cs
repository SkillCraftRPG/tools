using Logitar.Portal.Contracts.Sessions;

namespace SkillCraft.Tools.Authentication;

internal interface IOpenAuthenticationService
{
  Task<SessionModel> ValidateAsync(string accessToken, CancellationToken cancellationToken = default);
}
