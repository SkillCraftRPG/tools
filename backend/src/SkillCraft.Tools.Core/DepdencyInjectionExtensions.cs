using Logitar.Cms.Core;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Tools.Core;

public static class DepdencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftToolsCore(this IServiceCollection services)
  {
    return services
      .AddLogitarCmsCore()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
  }
}
