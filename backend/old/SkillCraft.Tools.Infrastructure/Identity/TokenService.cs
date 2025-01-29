using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Tokens;
using SkillCraft.Tools.Core.Identity;
using SkillCraft.Tools.Core.Identity.Models;

namespace SkillCraft.Tools.Infrastructure.Identity;

internal class TokenService : ITokenService
{
  private readonly ITokenClient _tokenClient;

  public TokenService(ITokenClient tokenClient)
  {
    _tokenClient = tokenClient;
  }

  public async Task<CreatedTokenModel> CreateAsync(IdentityModel identity, CancellationToken cancellationToken)
  {
    return await CreateAsync(identity, lifetimeSeconds: null, cancellationToken);
  }
  public async Task<CreatedTokenModel> CreateAsync(IdentityModel identity, int? lifetimeSeconds, CancellationToken cancellationToken)
  {
    return await CreateAsync(identity, lifetimeSeconds, type: null, cancellationToken);
  }
  public async Task<CreatedTokenModel> CreateAsync(IdentityModel identity, int? lifetimeSeconds, string? type, CancellationToken cancellationToken)
  {
    return await CreateAsync(identity, lifetimeSeconds, type, isConsumable: false, cancellationToken);
  }
  public async Task<CreatedTokenModel> CreateAsync(IdentityModel identity, int? lifetimeSeconds, string? type, bool isConsumable, CancellationToken cancellationToken)
  {
    CreateTokenPayload payload = new()
    {
      IsConsumable = isConsumable,
      LifetimeSeconds = lifetimeSeconds,
      Type = type,
      Subject = identity.Subject,
      Email = identity.Email
    };
    payload.Claims.AddRange(identity.Claims);
    RequestContext context = new(cancellationToken);
    CreatedTokenModel createdToken = await _tokenClient.CreateAsync(payload, context);
    return createdToken;
  }

  public async Task<ValidatedTokenModel> ValidateAsync(string token, CancellationToken cancellationToken)
  {
    return await ValidateAsync(token, type: null, cancellationToken);
  }
  public async Task<ValidatedTokenModel> ValidateAsync(string token, string? type, CancellationToken cancellationToken)
  {
    return await ValidateAsync(token, type, consume: false, cancellationToken);
  }
  public async Task<ValidatedTokenModel> ValidateAsync(string token, string? type, bool consume, CancellationToken cancellationToken)
  {
    ValidateTokenPayload payload = new(token)
    {
      Consume = consume,
      Type = type
    };
    RequestContext context = new(cancellationToken);
    ValidatedTokenModel validatedToken = await _tokenClient.ValidateAsync(payload, context);
    return validatedToken;
  }
}
