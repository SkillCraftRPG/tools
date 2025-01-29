using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Tools.Infrastructure.PostgreSQL;

public static class DependencyInjectionExtensions
{
  private const string ConfigurationKey = "POSTGRESQLCONNSTR_SkillCraft";

  public static IServiceCollection AddSkillCraftToolsInfrastructurePostgreSQL(this IServiceCollection services, IConfiguration configuration)
  {
    string? connectionString = Environment.GetEnvironmentVariable(ConfigurationKey);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      connectionString = configuration.GetValue<string>(ConfigurationKey);
    }
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      throw new ArgumentException($"The configuration '{ConfigurationKey}' could not be found.", nameof(configuration));
    }
    return services.AddSkillCraftToolsInfrastructurePostgreSQL(connectionString.Trim());
  }
  public static IServiceCollection AddSkillCraftToolsInfrastructurePostgreSQL(this IServiceCollection services, string connectionString)
  {
    return services
      .AddLogitarEventSourcingWithEntityFrameworkCorePostgreSQL(connectionString)
      .AddDbContext<SkillCraftContext>(options => options.UseNpgsql(connectionString,
        options => options.MigrationsAssembly("SkillCraft.Tools.Infrastructure.PostgreSQL")))
      .AddSingleton<ISqlHelper, PostgresHelper>();
  }
}
