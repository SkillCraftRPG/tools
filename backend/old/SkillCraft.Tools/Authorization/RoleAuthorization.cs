using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using SkillCraft.Tools.Extensions;

namespace SkillCraft.Tools.Authorization;

internal record RoleAuthorizationRequirement(string Role) : IAuthorizationRequirement;

internal class RoleAuthorizationHandler : AuthorizationHandler<RoleAuthorizationRequirement>
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public RoleAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAuthorizationRequirement requirement)
  {
    if (_httpContextAccessor.HttpContext != null)
    {
      string role = Normalize(requirement.Role);

      UserModel? user = _httpContextAccessor.HttpContext.GetUser();
      if (user != null)
      {
        if (user.Roles.Any(r => role.Equals(Normalize(r.UniqueName))))
        {
          context.Succeed(requirement);
        }
      }

      ApiKeyModel? apiKey = _httpContextAccessor.HttpContext.GetApiKey();
      if (apiKey != null)
      {
        if (apiKey.Roles.Any(r => role.Equals(Normalize(r.UniqueName))))
        {
          context.Succeed(requirement);
        }
      }
    }

    return Task.CompletedTask;
  }

  private static string Normalize(string role) => role.Trim().ToUpperInvariant();
}
