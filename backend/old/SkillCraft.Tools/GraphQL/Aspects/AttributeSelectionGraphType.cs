using GraphQL.Types;
using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.GraphQL.Aspects;

internal class AttributeSelectionGraphType : ObjectGraphType<AttributeSelectionModel>
{
  public AttributeSelectionGraphType()
  {
    Name = "AttributeSelection";
    Description = "Represents the attribute selection of an aspect.";

    Field(x => x.Mandatory1, type: typeof(AttributeGraphType))
      .Description("The first mandatory attribute of the aspect.");
    Field(x => x.Mandatory2, type: typeof(AttributeGraphType))
      .Description("The second mandatory attribute of the aspect.");
    Field(x => x.Optional1, type: typeof(AttributeGraphType))
      .Description("The first optional attribute of the aspect.");
    Field(x => x.Optional2, type: typeof(AttributeGraphType))
      .Description("The second optional attribute of the aspect.");
  }
}
