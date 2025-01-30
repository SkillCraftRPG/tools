using Logitar.Cms.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Tools.Core.Aspects;
using SkillCraft.Tools.Core.Castes;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Educations;
using SkillCraft.Tools.Core.Languages;
using SkillCraft.Tools.Core.Lineages;
using SkillCraft.Tools.Core.Natures;
using SkillCraft.Tools.Core.Specializations;
using SkillCraft.Tools.Core.Talents;
using SkillCraft.Tools.Infrastructure.Queriers;

namespace SkillCraft.Tools.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftToolsInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarCmsInfrastructure()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddQueriers();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services
      .AddScoped<IAspectQuerier, AspectQuerier>()
      .AddScoped<ICasteQuerier, CasteQuerier>()
      .AddScoped<ICustomizationQuerier, CustomizationQuerier>()
      .AddScoped<IEducationQuerier, EducationQuerier>()
      .AddScoped<ILanguageQuerier, LanguageQuerier>()
      .AddScoped<ILineageQuerier, LineageQuerier>()
      .AddScoped<INatureQuerier, NatureQuerier>()
      .AddScoped<ISpecializationQuerier, SpecializationQuerier>()
      .AddScoped<ITalentQuerier, TalentQuerier>();
  }
}
