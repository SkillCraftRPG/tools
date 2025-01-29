using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Search;
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

  public async Task<IReadOnlyCollection<ApiKeyModel>> FindAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
  {
    SearchApiKeysPayload payload = new();
    payload.Ids.AddRange(ids);
    RequestContext context = new(cancellationToken);
    SearchResults<ApiKeyModel> apiKeys = await _apiKeyClient.SearchAsync(payload, context);
    return apiKeys.Items.AsReadOnly();
  }
}
