using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Aspects;
using SkillCraft.Tools.Core.Caching;
using SkillCraft.Tools.Core.Castes;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Educations;
using SkillCraft.Tools.Core.Identity;
using SkillCraft.Tools.Core.Natures;
using SkillCraft.Tools.Core.Specializations;
using SkillCraft.Tools.Core.Talents;
using SkillCraft.Tools.Infrastructure.Actors;
using SkillCraft.Tools.Infrastructure.Caching;
using SkillCraft.Tools.Infrastructure.Identity;
using SkillCraft.Tools.Infrastructure.Queriers;
using SkillCraft.Tools.Infrastructure.Repositories;
using SkillCraft.Tools.Infrastructure.Settings;

namespace SkillCraft.Tools.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftToolsInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingWithEntityFrameworkCoreRelational()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddMemoryCache()
      .AddIdentityServices()
      .AddQueriers()
      .AddRepositories()
      .AddSingleton(InitializeCachingSettings)
      .AddSingleton<ICacheService, CacheService>()
      .AddSingleton<IEventSerializer, EventSerializer>()
      .AddScoped<IActorService, ActorService>()
      .AddScoped<IEventBus, EventBus>();
  }

  private static IServiceCollection AddIdentityServices(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyService, ApiKeyService>()
      .AddTransient<ISessionService, SessionService>()
      .AddTransient<ITokenService, TokenService>()
      .AddTransient<IUserService, UserService>();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services
      .AddScoped<IAspectQuerier, AspectQuerier>()
      .AddScoped<ICasteQuerier, CasteQuerier>()
      .AddScoped<ICustomizationQuerier, CustomizationQuerier>()
      .AddScoped<IEducationQuerier, EducationQuerier>()
      .AddScoped<INatureQuerier, NatureQuerier>()
      .AddScoped<ISpecializationQuerier, SpecializationQuerier>()
      .AddScoped<ITalentQuerier, TalentQuerier>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddScoped<IAspectRepository, AspectRepository>()
      .AddScoped<ICasteRepository, CasteRepository>()
      .AddScoped<ICustomizationRepository, CustomizationRepository>()
      .AddScoped<IEducationRepository, EducationRepository>()
      .AddScoped<INatureRepository, NatureRepository>()
      .AddScoped<ITalentRepository, TalentRepository>();
  }

  private static CachingSettings InitializeCachingSettings(IServiceProvider serviceProvider)
  {
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return configuration.GetSection(CachingSettings.SectionKey).Get<CachingSettings>() ?? new();
  }
}
