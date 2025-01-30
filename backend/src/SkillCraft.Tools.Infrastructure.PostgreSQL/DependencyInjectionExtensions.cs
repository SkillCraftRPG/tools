using Logitar.Cms.Infrastructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Tools.Infrastructure.PostgreSQL;

public static class DependencyInjectionExtensions
{
  private const string ConfigurationKey = "POSTGRESQLCONNSTR_SkillCraft";

  public static IServiceCollection AddSkillCraftToolsWithPostgreSQL(this IServiceCollection services, IConfiguration configuration)
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
    return services.AddSkillCraftToolsWithPostgreSQL(connectionString.Trim());
  }
  public static IServiceCollection AddSkillCraftToolsWithPostgreSQL(this IServiceCollection services, string connectionString)
  {
    return services
      .AddLogitarCmsWithPostgreSQL(connectionString)
      .AddDbContext<SkillCraftContext>(options => options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly("SkillCraft.Tools.Infrastructure.PostgreSQL")));
  }
}
