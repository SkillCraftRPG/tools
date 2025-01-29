using GraphQL.Types;
using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.GraphQL.Castes;

internal class FeatureGraphType : ObjectGraphType<FeatureModel>
{
  public FeatureGraphType()
  {
    Name = "Feature";
    Description = "Represents a feature granted by a caste.";

    Field(x => x.Id)
      .Description("The unique identifier of the feature.");

    Field(x => x.Name)
      .Description("The name of the feature.");
    Field(x => x.Description)
      .Description("The description of the feature.");
  }
}
