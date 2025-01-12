using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Sessions;
using SkillCraft.Tools.Core.Identity;

namespace SkillCraft.Tools.Infrastructure.Identity;

internal class SessionService : ISessionService
{
  private readonly ISessionClient _sessionClient;

  public SessionService(ISessionClient sessionClient)
  {
    _sessionClient = sessionClient;
  }

  public async Task<SessionModel?> FindAsync(Guid id, CancellationToken cancellationToken)
  {
    RequestContext context = new(cancellationToken);
    SessionModel? session = await _sessionClient.ReadAsync(id, context);
    return session;
  }

  public async Task<SessionModel> RenewAsync(string refreshToken, IEnumerable<CustomAttribute> customAttributes, CancellationToken cancellationToken)
  {
    RenewSessionPayload payload = new(refreshToken, customAttributes);
    RequestContext context = new(cancellationToken);
    SessionModel session = await _sessionClient.RenewAsync(payload, context);
    return session;
  }
}
