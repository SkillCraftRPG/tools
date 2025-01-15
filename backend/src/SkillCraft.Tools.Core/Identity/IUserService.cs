using Logitar.Portal.Contracts.Users;

namespace SkillCraft.Tools.Core.Identity;

public interface IUserService
{
  Task<UserModel> AuthenticateAsync(string uniqueName, string password, CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<UserModel>> FindAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
