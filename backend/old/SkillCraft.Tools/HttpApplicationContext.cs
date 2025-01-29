using Logitar.EventSourcing;
using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Users;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Extensions;

namespace SkillCraft.Tools;

internal class HttpApplicationContext : IApplicationContext
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public HttpApplicationContext(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public ActorId? ActorId
  {
    get
    {
      if (_httpContextAccessor.HttpContext != null)
      {
        UserModel? user = _httpContextAccessor.HttpContext.GetUser();
        if (user != null)
        {
          return new ActorId(user.Id);
        }

        ApiKeyModel? apiKey = _httpContextAccessor.HttpContext.GetApiKey();
        if (apiKey != null)
        {
          return new ActorId(apiKey.Id);
        }
      }

      return null;
    }
  }
}
