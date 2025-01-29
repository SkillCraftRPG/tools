using GraphQL;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Portal.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.FeatureManagement;
using Scalar.AspNetCore;
using SkillCraft.Tools.Authentication;
using SkillCraft.Tools.Authorization;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Extensions;
using SkillCraft.Tools.Filters;
using SkillCraft.Tools.GraphQL;
using SkillCraft.Tools.Infrastructure;
using SkillCraft.Tools.Infrastructure.Commands;
using SkillCraft.Tools.Infrastructure.MongoDB;
using SkillCraft.Tools.Infrastructure.PostgreSQL;
using SkillCraft.Tools.Infrastructure.SqlServer;
using SkillCraft.Tools.Middlewares;
using SkillCraft.Tools.Settings;

namespace SkillCraft.Tools;

internal class Startup : StartupBase
{
  private readonly string[] _authenticationSchemes;
  private readonly IConfiguration _configuration;
  private readonly CorsSettings _corsSettings;
  private readonly IHostEnvironment _environment;

  public Startup(IConfiguration configuration, IHostEnvironment environment)
  {
    _authenticationSchemes = Schemes.GetEnabled(configuration);
    _configuration = configuration;
    _corsSettings = configuration.GetSection(CorsSettings.SectionKey).Get<CorsSettings>() ?? new();
    _environment = environment;
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddSingleton(_corsSettings);
    services.AddCors();

    OpenAuthenticationSettings openAuthenticationSettings = _configuration.GetSection(OpenAuthenticationSettings.SectionKey).Get<OpenAuthenticationSettings>() ?? new();
    services.AddSingleton(openAuthenticationSettings);
    services.AddTransient<IOpenAuthenticationService, OpenAuthenticationService>();
    AuthenticationBuilder authenticationBuilder = services.AddAuthentication()
      .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(Schemes.ApiKey, options => { })
      .AddScheme<BearerAuthenticationOptions, BearerAuthenticationHandler>(Schemes.Bearer, options => { })
      .AddScheme<SessionAuthenticationOptions, SessionAuthenticationHandler>(Schemes.Session, options => { });
    if (_authenticationSchemes.Contains(Schemes.Basic))
    {
      authenticationBuilder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(Schemes.Basic, options => { });
    }

    services.AddAuthorizationBuilder()
      .SetDefaultPolicy(new AuthorizationPolicyBuilder(_authenticationSchemes)
        .RequireAuthenticatedUser()
        .Build())
      .AddPolicy(Policies.IsAdmin, new AuthorizationPolicyBuilder(_authenticationSchemes)
        .RequireAuthenticatedUser()
        .AddRequirements(new RoleAuthorizationRequirement("admin"))
        .Build());
    services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();

    CookiesSettings cookiesSettings = _configuration.GetSection(CookiesSettings.SectionKey).Get<CookiesSettings>() ?? new();
    services.AddSingleton(cookiesSettings);
    services.AddSession(options =>
    {
      options.Cookie.SameSite = cookiesSettings.Session.SameSite;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
    services.AddDistributedMemoryCache();

    services.AddControllersWithViews(options => options.Filters.Add<OperationLogging>())
      .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    services.AddGraphQL(builder => builder
      .AddAuthorizationRule()
      .AddSchema<SkillCraftSchema>()
      .AddSystemTextJson()
      .AddGraphTypes(Assembly.GetExecutingAssembly())
      .ConfigureExecutionOptions(options =>
      {
        options.EnableMetrics = !_environment.IsProduction();
        options.ThrowOnUnhandledException = true;
      }));

    services.AddApplicationInsightsTelemetry();
    IHealthChecksBuilder healthChecks = services.AddHealthChecks();

    services.AddOpenApi();

    services.AddSkillCraftToolsCore();
    services.AddSkillCraftToolsInfrastructure();
    services.AddSkillCraftToolsInfrastructureMongoDB(_configuration);
    services.AddSingleton<IApplicationContext, HttpApplicationContext>();

    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.SqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.PostgreSQL:
        services.AddSkillCraftToolsInfrastructurePostgreSQL(_configuration);
        healthChecks.AddDbContextCheck<EventContext>();
        healthChecks.AddDbContextCheck<SkillCraftContext>();
        break;
      case DatabaseProvider.SqlServer:
        services.AddSkillCraftToolsInfrastructureSqlServer(_configuration);
        healthChecks.AddDbContextCheck<EventContext>();
        healthChecks.AddDbContextCheck<SkillCraftContext>();
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    services.AddExceptionHandler<ExceptionHandler>();
    services.AddFeatureManagement();
    services.AddProblemDetails();

    services.AddLogitarPortalClient(_configuration);
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

    if (await featureManager.IsEnabledAsync(Features.UseGraphQLGraphiQL))
    {
      application.UseGraphQLGraphiQL();
    }
    if (await featureManager.IsEnabledAsync(Features.UseGraphQLVoyager))
    {
      application.UseGraphQLVoyager();
    }

    application.UseHttpsRedirection();
    application.UseCors(_corsSettings);
    application.UseStaticFiles();
    application.UseExceptionHandler();
    application.UseSession();
    application.UseMiddleware<Logging>();
    application.UseMiddleware<RenewSession>();
    application.UseMiddleware<RedirectNotFound>();
    application.UseAuthentication();
    application.UseAuthorization();

    application.UseGraphQL<SkillCraftSchema>("/graphql", options => options.AuthenticationSchemes.AddRange(_authenticationSchemes));

    application.MapControllers();

    if (await featureManager.IsEnabledAsync(Features.MigrateDatabase))
    {
      using IServiceScope scope = application.Services.CreateScope();
      IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      await mediator.Send(new MigrateDatabaseCommand());
    }
  }
}
