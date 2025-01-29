using Logitar.Cms.Core;
using Logitar.Cms.Infrastructure;
using Logitar.Cms.Infrastructure.PostgreSQL;
using Logitar.Cms.Infrastructure.SqlServer;
using Logitar.Cms.Web;
using Logitar.Cms.Web.Authentication;
using Logitar.Cms.Web.Constants;
using Logitar.Cms.Web.Extensions;
using Logitar.Cms.Web.Middlewares;
using Logitar.Cms.Web.Settings;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.FeatureManagement;
using Scalar.AspNetCore;
using SkillCraft.Tools.Constants;

namespace SkillCraft.Tools;

internal class Startup : StartupBase
{
  private readonly string[] _authenticationSchemes;
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _authenticationSchemes = Schemes.GetEnabled(configuration);
    _configuration = configuration;
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddLogitarCmsCore();
    services.AddLogitarCmsInfrastructure();
    services.AddLogitarCmsWeb(_configuration);

    services.AddCors();

    AuthenticationBuilder authenticationBuilder = services.AddAuthentication()
      //.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(Schemes.ApiKey, options => { }) // TODO(fpion): X-API-Key
      .AddScheme<BearerAuthenticationOptions, BearerAuthenticationHandler>(Schemes.Bearer, options => { })
      .AddScheme<SessionAuthenticationOptions, SessionAuthenticationHandler>(Schemes.Session, options => { });
    if (_authenticationSchemes.Contains(Schemes.Basic))
    {
      authenticationBuilder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(Schemes.Basic, options => { });
    }

    services.AddAuthorizationBuilder()
      .SetDefaultPolicy(new AuthorizationPolicyBuilder(_authenticationSchemes).RequireAuthenticatedUser().Build());
    //  .AddPolicy(Policies.IsAdmin, new AuthorizationPolicyBuilder(_authenticationSchemes)
    //    .RequireAuthenticatedUser()
    //    .AddRequirements(new RoleAuthorizationRequirement("admin"))
    //    .Build());
    //services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>(); // TODO(fpion): RBAC

    CookiesSettings cookiesSettings = _configuration.GetSection(CookiesSettings.SectionKey).Get<CookiesSettings>() ?? new();
    services.AddSession(options =>
    {
      options.Cookie.SameSite = cookiesSettings.Session.SameSite;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

    services.AddApplicationInsightsTelemetry();
    IHealthChecksBuilder healthChecks = services.AddHealthChecks();

    services.AddOpenApi();

    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.SqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.PostgreSQL:
        services.AddLogitarCmsWithPostgreSQL(_configuration);
        healthChecks.AddDbContextCheck<EventContext>();
        healthChecks.AddDbContextCheck<IdentityContext>();
        healthChecks.AddDbContextCheck<CmsContext>();
        break;
      case DatabaseProvider.SqlServer:
        services.AddLogitarCmsWithSqlServer(_configuration);
        healthChecks.AddDbContextCheck<EventContext>();
        healthChecks.AddDbContextCheck<IdentityContext>();
        healthChecks.AddDbContextCheck<CmsContext>();
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    services.AddDistributedMemoryCache();
    services.AddExceptionHandler<ExceptionHandler>();
    services.AddFeatureManagement();
    services.AddProblemDetails();
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (builder is WebApplication application)
    {
      ConfigureAsync(application).Wait();
    }
  }
  public async Task ConfigureAsync(WebApplication application)
  {
    IFeatureManager featureManager = application.Services.GetRequiredService<IFeatureManager>();

    if (await featureManager.IsEnabledAsync(Features.UseScalarUI))
    {
      application.MapOpenApi();
      application.MapScalarApiReference();
    }

    //if (await featureManager.IsEnabledAsync(Features.UseGraphQLGraphiQL))
    //{
    //  application.UseGraphQLGraphiQL();
    //}
    //if (await featureManager.IsEnabledAsync(Features.UseGraphQLVoyager))
    //{
    //  application.UseGraphQLVoyager();
    //}

    application.UseHttpsRedirection();
    application.UseCors(application.Services.GetRequiredService<CorsSettings>());
    application.UseStaticFiles();
    application.UseExceptionHandler();
    application.UseSession();
    //application.UseMiddleware<Logging>(); // TODO(fpion): Logging
    application.UseMiddleware<RenewSession>();
    application.UseMiddleware<RedirectNotFound>();
    application.UseAuthentication();
    application.UseAuthorization();

    //application.UseGraphQL<SkillCraftSchema>("/graphql", options => options.AuthenticationSchemes.AddRange(_authenticationSchemes)); // TODO(fpion): GraphQL

    application.MapControllers();
    application.MapHealthChecks("/health");
  }
}
