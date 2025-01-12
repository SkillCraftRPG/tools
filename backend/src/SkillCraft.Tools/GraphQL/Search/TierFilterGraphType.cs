using GraphQL.Types;
using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.GraphQL.Search;

internal class TierFilterGraphType : InputObjectGraphType<TierFilter>
{
  public TierFilterGraphType()
  {
    Name = "TierFilter";
    Description = "Represents a filter to match data on game tiers.";

    Field(x => x.Values, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<IntGraphType>>>))
      .Description("The values compared to the tier of the entities. It should only contain values ranging from 0 to 3, each value should only be included once, and only one value should be included when not using the IN or NOT IN operator.");
    Field(x => x.Operator)
      .DefaultValue("eq")
      .Description("The comparison operator (case-insensitive). Expected operators are: eq (equal to) (DEFAULT), ne (not equal to), gt (greater than), gte (greater than or equal to), lt (less than), lte (less than or equal to), in (in Values), and nin (not in Values).");
  }
}
