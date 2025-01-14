﻿using GraphQL.Types;
using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.GraphQL.Search;

internal class SearchTermGraphType : InputObjectGraphType<SearchTerm>
{
  public SearchTermGraphType()
  {
    Name = "SearchTerm";
    Description = "Represents a search term.";

    Field(x => x.Value)
      .Description("The textual value of the search term.");
  }
}
