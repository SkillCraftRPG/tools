using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Tools.Core.Aspects;
using SkillCraft.Tools.Core.Castes;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Educations;
using SkillCraft.Tools.Core.Languages;
using SkillCraft.Tools.Core.Logging;
using SkillCraft.Tools.Core.Natures;
using SkillCraft.Tools.Core.Specializations;
using SkillCraft.Tools.Core.Talents;

namespace SkillCraft.Tools.Core;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftToolsCore(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcing()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
      .AddSingleton<ILoggingSettings>(InitializeLoggingSettings)
      .AddScoped<ILoggingService, LoggingService>()
      .AddManagers();
  }

  private static IServiceCollection AddManagers(this IServiceCollection services)
  {
    return services
      .AddTransient<IAspectManager, AspectManager>()
      .AddTransient<ICasteManager, CasteManager>()
      .AddTransient<ICustomizationManager, CustomizationManager>()
      .AddTransient<IEducationManager, EducationManager>()
      .AddTransient<ILanguageManager, LanguageManager>()
      .AddTransient<INatureManager, NatureManager>()
      .AddTransient<ISpecializationManager, SpecializationManager>()
      .AddTransient<ITalentManager, TalentManager>();
  }

  private static LoggingSettings InitializeLoggingSettings(IServiceProvider serviceProvider)
  {
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return configuration.GetSection("ApplicationLogging").Get<LoggingSettings>() ?? new();
  }
}
