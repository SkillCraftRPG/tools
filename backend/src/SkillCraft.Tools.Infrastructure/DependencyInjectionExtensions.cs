using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Caching;
using SkillCraft.Tools.Core.Specializations;
using SkillCraft.Tools.Core.Talents;
using SkillCraft.Tools.Infrastructure.Actors;
using SkillCraft.Tools.Infrastructure.Caching;
using SkillCraft.Tools.Infrastructure.Queriers;
using SkillCraft.Tools.Infrastructure.Settings;

namespace SkillCraft.Tools.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftToolsInfrastructure(this IServiceCollection services)
  {
    return services
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddMemoryCache()
      .AddQueriers()
      .AddSingleton(InitializeCachingSettings)
      .AddSingleton<ICacheService, CacheService>()
      .AddScoped<IActorService, ActorService>();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services
      .AddScoped<ISpecializationQuerier, SpecializationQuerier>()
      .AddScoped<ITalentQuerier, TalentQuerier>();
  }

  private static CachingSettings InitializeCachingSettings(IServiceProvider serviceProvider)
  {
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return configuration.GetSection(CachingSettings.SectionKey).Get<CachingSettings>() ?? new();
  }
}
