using GraphQL;
using GraphQL.Execution;
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
using SkillCraft.Tools.GraphQL;
using SkillCraft.Tools.Infrastructure;
using SkillCraft.Tools.Infrastructure.Commands;
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

  public Startup(IConfiguration configuration)
  {
    _authenticationSchemes = Schemes.GetEnabled(configuration);
    _configuration = configuration;
    _corsSettings = configuration.GetSection(CorsSettings.SectionKey).Get<CorsSettings>() ?? new();
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddSkillCraftToolsCore();
    services.AddSkillCraftToolsInfrastructure();
    services.AddSingleton<IApplicationContext, HttpApplicationContext>();

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

    services.AddControllersWithViews()
      .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    GraphQLSettings graphQLSettings = _configuration.GetSection(GraphQLSettings.SectionKey).Get<GraphQLSettings>() ?? new();
    services.AddSingleton(graphQLSettings);
    services.AddGraphQL(builder => builder
      .AddAuthorizationRule()
      .AddSchema<SkillCraftSchema>()
      .AddSystemTextJson()
      .AddErrorInfoProvider(new ErrorInfoProvider(options => options.ExposeExceptionDetails = graphQLSettings.ExposeExceptionDetails))
      .AddGraphTypes(Assembly.GetExecutingAssembly())
      .ConfigureExecutionOptions(options => options.EnableMetrics = graphQLSettings.EnableMetrics));

    services.AddApplicationInsightsTelemetry();
    IHealthChecksBuilder healthChecks = services.AddHealthChecks();

    services.AddOpenApi();

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
    application.UseMiddleware<RenewSession>();
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
