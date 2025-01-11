using GraphQL.Types;

namespace SkillCraft.Tools.GraphQL;

internal class SkillCraftSchema : Schema
{
  public SkillCraftSchema(IServiceProvider serviceProvider) : base(serviceProvider)
  {
    Query = serviceProvider.GetRequiredService<RootQuery>();
  }
}
