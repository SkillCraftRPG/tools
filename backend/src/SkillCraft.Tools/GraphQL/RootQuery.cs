﻿using GraphQL.Types;
using SkillCraft.Tools.GraphQL.Customizations;
using SkillCraft.Tools.GraphQL.Educations;
using SkillCraft.Tools.GraphQL.Natures;
using SkillCraft.Tools.GraphQL.Specializations;
using SkillCraft.Tools.GraphQL.Talents;

namespace SkillCraft.Tools.GraphQL;

internal class RootQuery : ObjectGraphType
{
  public RootQuery()
  {
    Name = "RootQuery";

    CustomizationQueries.Register(this);
    EducationQueries.Register(this);
    NatureQueries.Register(this);
    SpecializationQueries.Register(this);
    TalentQueries.Register(this);
  }
}
