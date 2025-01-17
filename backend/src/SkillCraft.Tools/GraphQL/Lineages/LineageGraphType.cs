using GraphQL.Types;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.GraphQL.Lineages;

internal class LineageGraphType : AggregateGraphType<LineageModel>
{
  public LineageGraphType() : base("Represents a character lineage.")
  {
    Field(x => x.UniqueSlug)
      .Description("The unique slug of the lineage.");
    Field(x => x.DisplayName)
      .Description("The display name of the lineage.");
    Field(x => x.Description)
      .Description("A textual description of the lineage. It may contain Markdown and HTML.");

    Field(x => x.Attributes, type: typeof(NonNullGraphType<AttributeBonusesGraphType>))
      .Description("The attribute bonuses granted by this lineage.");
    Field(x => x.Traits, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<TraitGraphType>>>))
      .Description("The traits granted to characters in this lineage.");

    Field(x => x.Languages, type: typeof(NonNullGraphType<LanguagesGraphType>))
      .Description("The languages spoken by this lineage.");
    Field(x => x.Names, type: typeof(NonNullGraphType<NamesGraphType>))
      .Description("The names given to individuals in this lineage.");

    Field(x => x.Speeds, type: typeof(NonNullGraphType<SpeedsGraphType>))
      .Description("The movement speeds of the individuals in this lineage.");
    Field(x => x.Size, type: typeof(NonNullGraphType<SizeGraphType>))
      .Description("The size parameters of this lineage.");
    Field(x => x.Weight, type: typeof(NonNullGraphType<WeightGraphType>))
      .Description("The weight parameters of this lineage.");
    Field(x => x.Ages, type: typeof(NonNullGraphType<AgesGraphType>))
      .Description("The age parameters of this lineage.");

    Field(x => x.Parent, type: typeof(LineageGraphType))
      .Description("The parent lineage of this lineage.");
    Field(x => x.Children, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<LineageGraphType>>>))
      .Description("The child lineages of this lineage.");
  }
}
