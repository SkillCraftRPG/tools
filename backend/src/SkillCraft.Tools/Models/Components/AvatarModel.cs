using Logitar.Cms.Core.Actors;

namespace SkillCraft.Tools.Models.Components;

public record AvatarModel
{
  public string? DisplayName { get; set; }
  public string? EmailAddress { get; set; }
  public string? Icon { get; set; } = "fas fa-user";
  public int? Size { get; set; } = 32;
  public string? Url { get; set; }
  public BadgeVariant? Variant { get; set; } = BadgeVariant.Secondary;

  public AvatarModel()
  {
  }

  public AvatarModel(ActorModel actor)
  {
    DisplayName = actor.DisplayName;
    EmailAddress = actor.EmailAddress;
    Url = actor.PictureUrl;
  }
}
