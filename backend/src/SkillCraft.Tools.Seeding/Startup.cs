using Logitar.Cms.Core;
using Logitar.Cms.Infrastructure;
using Logitar.Cms.Infrastructure.PostgreSQL;
using Logitar.Cms.Infrastructure.SqlServer;
using System.Reflection;

namespace SkillCraft.Tools.Seeding;

internal class Startup
{
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddHostedService<SeedingWorker>();
    services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    services.AddLogitarCmsCore();
    services.AddLogitarCmsInfrastructure();

    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.SqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.PostgreSQL:
        services.AddLogitarCmsWithPostgreSQL(_configuration);
        break;
      case DatabaseProvider.SqlServer:
        services.AddLogitarCmsWithSqlServer(_configuration);
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    services.AddSingleton<IApplicationContext, SeedingApplicationContext>();
  }
}
