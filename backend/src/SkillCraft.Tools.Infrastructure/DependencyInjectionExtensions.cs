using Logitar.Cms.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Tools.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftToolsInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarCmsInfrastructure()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
  }
}
