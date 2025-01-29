using Logitar.Portal.Contracts.ApiKeys;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Identity;
using SkillCraft.Tools.Extensions;

namespace SkillCraft.Tools.Authentication;

internal class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions;

internal class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
  private readonly IApiKeyService _apiKeyService;

  public ApiKeyAuthenticationHandler(IApiKeyService apiKeyService, IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : base(options, logger, encoder)
  {
    _apiKeyService = apiKeyService;
  }

  protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    if (Context.Request.Headers.TryGetValue(Headers.ApiKey, out StringValues values))
    {
      string? value = values.Single();
      if (!string.IsNullOrWhiteSpace(value))
      {
        try
        {
          ApiKeyModel apiKey = await _apiKeyService.AuthenticateAsync(value);

          Context.SetApiKey(apiKey);

          ClaimsPrincipal principal = new(apiKey.CreateClaimsIdentity(Scheme.Name));
          AuthenticationTicket ticket = new(principal, Scheme.Name);

          return AuthenticateResult.Success(ticket);
        }
        catch (Exception exception)
        {
          return AuthenticateResult.Fail(exception);
        }
      }
    }

    return AuthenticateResult.NoResult();
  }
}
