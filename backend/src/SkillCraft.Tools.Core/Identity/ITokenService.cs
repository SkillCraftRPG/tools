using Logitar.Portal.Contracts.Tokens;

namespace SkillCraft.Tools.Core.Identity;

public interface ITokenService
{
  Task<ValidatedTokenModel> ValidateAsync(string token, CancellationToken cancellationToken = default);
  Task<ValidatedTokenModel> ValidateAsync(string token, string? type = null, CancellationToken cancellationToken = default);
  Task<ValidatedTokenModel> ValidateAsync(string token, string? type = null, bool consume = false, CancellationToken cancellationToken = default);
}
