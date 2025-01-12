using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Talents;

internal class TalentSearchResultsGraphType : SearchResultsGraphType<TalentGraphType, TalentModel>
{
  public TalentSearchResultsGraphType() : base("TalentSearchResults", "Represents the results of a talent search.")
  {
  }
}
