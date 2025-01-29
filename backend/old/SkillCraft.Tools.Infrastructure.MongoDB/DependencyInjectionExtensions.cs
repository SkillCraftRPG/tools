using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SkillCraft.Tools.Core.Logging;
using SkillCraft.Tools.Infrastructure.MongoDB.Repositories;
using SkillCraft.Tools.Infrastructure.MongoDB.Settings;

namespace SkillCraft.Tools.Infrastructure.MongoDB;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftToolsInfrastructureMongoDB(this IServiceCollection services, IConfiguration configuration)
  {
    MongoDBSettings settings = configuration.GetSection(MongoDBSettings.SectionKey).Get<MongoDBSettings>() ?? new();
    services.AddSingleton(settings);
    if (!string.IsNullOrWhiteSpace(settings.ConnectionString) && !string.IsNullOrWhiteSpace(settings.DatabaseName))
    {
      MongoClient client = new(settings.ConnectionString.Trim());
      IMongoDatabase database = client.GetDatabase(settings.DatabaseName.Trim());
      services.AddSingleton(database).AddTransient<ILogRepository, LogRepository>();
    }

    return services;
  }
}

