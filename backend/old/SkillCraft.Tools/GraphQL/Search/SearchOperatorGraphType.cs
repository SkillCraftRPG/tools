using GraphQL.Types;
using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.GraphQL.Search;

internal class SearchOperatorGraphType : EnumerationGraphType<SearchOperator>
{
  public SearchOperatorGraphType()
  {
    Name = "SearchOperator";
    Description = "Represents the available operators of a textual search.";

    AddValue(SearchOperator.And, "All terms must be found for the result to match the search.");
    AddValue(SearchOperator.Or, "At least one term may be found for the result to match the search.");
  }
  private void AddValue(SearchOperator value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
