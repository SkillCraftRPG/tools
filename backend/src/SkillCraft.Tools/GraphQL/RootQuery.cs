using GraphQL.Types;
using SkillCraft.Tools.GraphQL.Specializations;

namespace SkillCraft.Tools.GraphQL;

internal class RootQuery : ObjectGraphType
{
  public RootQuery()
  {
    Name = "RootQuery";

    SpecializationQueries.Register(this);
  }
}
