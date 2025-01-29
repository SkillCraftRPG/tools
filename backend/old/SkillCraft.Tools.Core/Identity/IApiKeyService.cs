using Logitar.Portal.Contracts.ApiKeys;

namespace SkillCraft.Tools.Core.Identity;

public interface IApiKeyService
{
  Task<ApiKeyModel> AuthenticateAsync(string xApiKey, CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<ApiKeyModel>> FindAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
