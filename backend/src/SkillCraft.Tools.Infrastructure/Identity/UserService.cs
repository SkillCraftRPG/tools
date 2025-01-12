using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Users;
using SkillCraft.Tools.Core.Identity;

namespace SkillCraft.Tools.Infrastructure.Identity;

internal class UserService : IUserService
{
  private readonly IUserClient _userClient;

  public UserService(IUserClient userClient)
  {
    _userClient = userClient;
  }

  public async Task<UserModel> AuthenticateAsync(string uniqueName, string password, CancellationToken cancellationToken)
  {
    AuthenticateUserPayload payload = new(uniqueName, password);
    RequestContext context = new(cancellationToken);
    UserModel user = await _userClient.AuthenticateAsync(payload, context);
    return user;
  }
}
