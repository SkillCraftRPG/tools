using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Tools.Infrastructure.SqlServer;

public static class DependencyInjectionExtensions
{
  private const string ConfigurationKey = "SQLCONNSTR_SkillCraft";

  public static IServiceCollection AddSkillCraftToolsInfrastructureSqlServer(this IServiceCollection services, IConfiguration configuration)
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
    return services.AddSkillCraftToolsInfrastructureSqlServer(connectionString.Trim());
  }
  public static IServiceCollection AddSkillCraftToolsInfrastructureSqlServer(this IServiceCollection services, string connectionString)
  {
    return services.AddDbContext<SkillCraftContext>(options => options.UseSqlServer(connectionString,
      options => options.MigrationsAssembly("SkillCraft.Tools.Infrastructure.SqlServer")));
  }
}
