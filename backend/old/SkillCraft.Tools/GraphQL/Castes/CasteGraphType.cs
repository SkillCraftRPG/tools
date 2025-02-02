﻿using GraphQL.Types;
using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.GraphQL.Castes;

internal class CasteGraphType : AggregateGraphType<CasteModel>
{
  public CasteGraphType() : base("Represents a character caste.")
  {
    Field(x => x.UniqueSlug)
      .Description("The unique slug of the caste.");
    Field(x => x.DisplayName)
      .Description("The display name of the caste.");
    Field(x => x.Description)
      .Description("A textual description of the caste. It may contain Markdown and HTML.");

    Field(x => x.Skill, type: typeof(SkillGraphType))
      .Description("The skill talent required by this caste.");
    Field(x => x.WealthRoll)
      .Description("The starting wealth roll of characters in this caste.");

    Field(x => x.Features, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<FeatureGraphType>>>))
      .Description("The features granted to characters in this caste.");
  }
}
