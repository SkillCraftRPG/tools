using GraphQL.Types;
using SkillCraft.Tools.Core.Actors.Models;

namespace SkillCraft.Tools.GraphQL.Actors;

internal class ActorGraphType : ObjectGraphType<ActorModel>
{
  public ActorGraphType()
  {
    Name = "Actor";
    Description = "Represents an actor in the SkillCraft system.";

    Field(x => x.Id)
      .Description("The unique identifier of the actor.");
    Field(x => x.Type, type: typeof(NonNullGraphType<ActorTypeGraphType>))
      .Description("The type of the actor.");
    Field(x => x.IsDeleted)
      .Description("A value indicating whether or not the actor is deleted.");

    Field(x => x.DisplayName)
      .Description("The display name of the actor.");
    Field(x => x.EmailAddress)
      .Description("The email address of the actor.");
    Field(x => x.PictureUrl)
      .Description("The URL to the actor's picture.");
  }
}
