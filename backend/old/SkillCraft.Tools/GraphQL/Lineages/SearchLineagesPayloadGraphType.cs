using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.GraphQL.Search;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class SearchLineagesPayloadGraphType : SearchPayloadInputGraphType<SearchLineagesPayload>
{
  public SearchLineagesPayloadGraphType() : base()
  {
    Field(x => x.Attribute, type: typeof(AttributeGraphType))
      .Description("When specified, only lineages granting a bonus to this attribute will match.");
    Field(x => x.LanguageId)
      .Description("When specified, only lineages learning this language will match.");
    Field(x => x.ParentId)
      .Description("When specified, only children of this lineage will match.");
    Field(x => x.SizeCategory, type: typeof(SizeCategoryGraphType))
      .Description("When specified, only lineages in this size category will match.");

    Field(x => x.Sort, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<LineageSortOptionGraphType>>>))
      .DefaultValue([])
      .Description("The sort parameters of the search.");
  }
}
