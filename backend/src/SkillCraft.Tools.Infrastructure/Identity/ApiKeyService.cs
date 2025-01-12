using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.ApiKeys;
using SkillCraft.Tools.Core.Identity;

namespace SkillCraft.Tools.Infrastructure.Identity;

internal class ApiKeyService : IApiKeyService
{
  private readonly IApiKeyClient _apiKeyClient;

  public ApiKeyService(IApiKeyClient apiKeyClient)
  {
    _apiKeyClient = apiKeyClient;
  }

  public async Task<ApiKeyModel> AuthenticateAsync(string xApiKey, CancellationToken cancellationToken)
  {
    AuthenticateApiKeyPayload payload = new(xApiKey);
    RequestContext context = new(cancellationToken);
    ApiKeyModel apiKey = await _apiKeyClient.AuthenticateAsync(payload, context);
    return apiKey;
  }
}
