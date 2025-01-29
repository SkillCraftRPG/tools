using Logitar.Portal.Contracts.Tokens;
using SkillCraft.Tools.Core.Identity.Models;

namespace SkillCraft.Tools.Core.Identity;

public interface ITokenService
{
  Task<CreatedTokenModel> CreateAsync(IdentityModel identity, CancellationToken cancellationToken = default);
  Task<CreatedTokenModel> CreateAsync(IdentityModel identity, int? lifetimeSeconds, CancellationToken cancellationToken = default);
  Task<CreatedTokenModel> CreateAsync(IdentityModel identity, int? lifetimeSeconds, string? type, CancellationToken cancellationToken = default);
  Task<CreatedTokenModel> CreateAsync(IdentityModel identity, int? lifetimeSeconds, string? type, bool isConsumable, CancellationToken cancellationToken = default);

  Task<ValidatedTokenModel> ValidateAsync(string token, CancellationToken cancellationToken = default);
  Task<ValidatedTokenModel> ValidateAsync(string token, string? type = null, CancellationToken cancellationToken = default);
  Task<ValidatedTokenModel> ValidateAsync(string token, string? type = null, bool consume = false, CancellationToken cancellationToken = default);
}
