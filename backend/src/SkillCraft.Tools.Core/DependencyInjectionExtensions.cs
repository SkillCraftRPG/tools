using Logitar.EventSourcing;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Tools.Core.Aspects;
using SkillCraft.Tools.Core.Castes;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Educations;
using SkillCraft.Tools.Core.Natures;
using SkillCraft.Tools.Core.Talents;

namespace SkillCraft.Tools.Core;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftToolsCore(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcing()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddManagers();
  }

  private static IServiceCollection AddManagers(this IServiceCollection services)
  {
    return services
      .AddTransient<IAspectManager, AspectManager>()
      .AddTransient<ICasteManager, CasteManager>()
      .AddTransient<ICustomizationManager, CustomizationManager>()
      .AddTransient<IEducationManager, EducationManager>()
      .AddTransient<INatureManager, NatureManager>()
      .AddTransient<ITalentManager, TalentManager>();
  }
}
