using Logitar.Cms.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Tools.Infrastructure.SqlServer;

public static class DependencyInjectionExtensions
{
  private const string ConfigurationKey = "SQLCONNSTR_SkillCraft";

  public static IServiceCollection AddSkillCraftToolsWithSqlServer(this IServiceCollection services, IConfiguration configuration)
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
    return services.AddSkillCraftToolsWithSqlServer(connectionString.Trim());
  }
  public static IServiceCollection AddSkillCraftToolsWithSqlServer(this IServiceCollection services, string connectionString)
  {
    return services
      .AddLogitarCmsWithSqlServer(connectionString)
      .AddDbContext<SkillCraftContext>(options => options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("SkillCraft.Tools.Infrastructure.SqlServer")));
  }
}
