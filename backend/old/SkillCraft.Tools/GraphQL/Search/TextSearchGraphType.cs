﻿using GraphQL.Types;
using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.GraphQL.Search;

internal class TextSearchGraphType : InputObjectGraphType<TextSearch>
{
  public TextSearchGraphType()
  {
    Name = "TextSearch";
    Description = "Represents textual search parameters.";

    Field(x => x.Terms, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<SearchTermGraphType>>>))
      .Description("The terms of the textual search.");
    Field(x => x.Operator, type: typeof(NonNullGraphType<SearchOperatorGraphType>))
      .Description("The operator of the textual search.");
  }
}
