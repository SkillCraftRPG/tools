using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Tools.Core;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftToolsCore(this IServiceCollection services)
  {
    return services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
  }
}
