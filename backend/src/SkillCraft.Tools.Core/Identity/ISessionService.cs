using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Sessions;

namespace SkillCraft.Tools.Core.Identity;

public interface ISessionService
{
  Task<SessionModel?> FindAsync(Guid id, CancellationToken cancellationToken = default);
  Task<SessionModel> RenewAsync(string refreshToken, IEnumerable<CustomAttribute> customAttributes, CancellationToken cancellationToken = default);
}
