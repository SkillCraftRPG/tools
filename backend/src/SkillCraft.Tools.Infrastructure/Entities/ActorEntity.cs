using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Users;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class ActorEntity
{
  public int ActorId { get; private set; }
  public Guid Id { get; private set; }
  public string Key { get; private set; } = string.Empty;

  public ActorType Type { get; private set; }
  public bool IsDeleted { get; private set; }

  public string DisplayName { get; private set; } = string.Empty;
  public string? EmailAddress { get; private set; }
  public string? PictureUrl { get; private set; }

  public ActorEntity(ApiKeyModel apiKey)
  {
    Id = apiKey.Id;
    Key = new ActorId(apiKey.Id).Value;

    Type = ActorType.ApiKey;

    Update(apiKey);
  }
  public ActorEntity(UserModel user)
  {
    Id = user.Id;
    Key = new ActorId(user.Id).Value;

    Type = ActorType.User;

    Update(user);
  }

  private ActorEntity()
  {
  }

  public void Update(ApiKeyModel apiKey)
  {
    DisplayName = apiKey.DisplayName;
  }
  public void Update(UserModel user)
  {
    DisplayName = user.FullName ?? user.UniqueName;
    EmailAddress = user.Email?.Address;
    PictureUrl = user.Picture;
  }

  public override bool Equals(object? obj) => obj is ActorEntity actor && actor.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString()
  {
    StringBuilder actor = new();
    actor.Append(DisplayName);
    if (EmailAddress != null)
    {
      actor.Append(" <").Append(EmailAddress).Append('>');
    }
    actor.Append(" (").Append(Type).Append(".Id=").Append(Id).Append(')');
    return actor.ToString();
  }
}
