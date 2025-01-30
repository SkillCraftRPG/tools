using Logitar.Cms.Core;
using Logitar.Cms.Infrastructure;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Infrastructure;
using SkillCraft.Tools.Infrastructure.PostgreSQL;
using SkillCraft.Tools.Infrastructure.SqlServer;

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

    services.AddSkillCraftToolsCore();
    services.AddSkillCraftToolsInfrastructure();

    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.SqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.PostgreSQL:
        services.AddSkillCraftToolsWithPostgreSQL(_configuration);
        break;
      case DatabaseProvider.SqlServer:
        services.AddSkillCraftToolsWithSqlServer(_configuration);
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    services.AddSingleton<IApplicationContext, SeedingApplicationContext>();
  }
}
