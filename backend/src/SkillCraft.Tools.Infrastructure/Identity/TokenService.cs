using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Tokens;
using SkillCraft.Tools.Core.Identity;

namespace SkillCraft.Tools.Infrastructure.Identity;

internal class TokenService : ITokenService
{
  private readonly ITokenClient _tokenClient;

  public TokenService(ITokenClient tokenClient)
  {
    _tokenClient = tokenClient;
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
