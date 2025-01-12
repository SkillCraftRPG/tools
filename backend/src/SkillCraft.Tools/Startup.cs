using GraphQL;
using GraphQL.Execution;
using Microsoft.FeatureManagement;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.GraphQL;
using SkillCraft.Tools.Infrastructure;
using SkillCraft.Tools.Infrastructure.SqlServer;
using SkillCraft.Tools.Settings;

namespace SkillCraft.Tools;

internal class Startup : StartupBase
{
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddSkillCraftToolsCore();
    services.AddSkillCraftToolsInfrastructure();

    services.AddControllers()
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

    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.SqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.SqlServer:
        services.AddSkillCraftToolsInfrastructureSqlServer(_configuration);
        healthChecks.AddDbContextCheck<SkillCraftContext>();
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    services.AddFeatureManagement();
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (builder is WebApplication application)
    {
      this.ConfigureAsync(application).Wait();
    }
  }
  public async Task ConfigureAsync(WebApplication application)
  {
    IFeatureManager featureManager = application.Services.GetRequiredService<IFeatureManager>();

    if (await featureManager.IsEnabledAsync(FeatureFlags.UseGraphQLGraphiQL))
    {
      application.UseGraphQLGraphiQL();
    }
    if (await featureManager.IsEnabledAsync(FeatureFlags.UseGraphQLVoyager))
    {
      application.UseGraphQLVoyager();
    }

    application.UseHttpsRedirection();

    application.UseGraphQL<SkillCraftSchema>("/graphql"/*, options => options.AuthenticationSchemes.AddRange(_authenticationSchemes)*/); // ISSUE: https://github.com/SkillCraftRPG/tools/issues/5

    application.MapControllers();
  }
}
