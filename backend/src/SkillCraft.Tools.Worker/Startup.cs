using Logitar.Portal.Client;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Infrastructure;
using SkillCraft.Tools.Infrastructure.SqlServer;
using SkillCraft.Tools.Worker.Portal;

namespace SkillCraft.Tools.Worker;

internal class Startup
{
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public void ConfigureServices(IServiceCollection services)
  {
    IPortalSettings portalSettings = _configuration.GetSection(PortalSettings.SectionKey).Get<PortalSettings>() ?? new();
    portalSettings = WorkerPortalSettings.Initialize(portalSettings);
    services.AddLogitarPortalClient(portalSettings);

    services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    services.AddHostedService<SeedingWorker>();

    services.AddSkillCraftToolsCore();
    services.AddSkillCraftToolsInfrastructure();
    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.SqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.SqlServer:
        services.AddSkillCraftToolsInfrastructureSqlServer(_configuration);
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }
  }
}
