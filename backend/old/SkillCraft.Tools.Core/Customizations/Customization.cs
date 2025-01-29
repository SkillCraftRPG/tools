using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Customizations.Events;

namespace SkillCraft.Tools.Core.Customizations;

public class Customization : AggregateRoot
{
  private CustomizationUpdated _updated = new();

  public new CustomizationId Id => new(base.Id);

  public CustomizationType Type { get; private set; }

  private Slug? _uniqueSlug = null;
  public Slug UniqueSlug
  {
    get => _uniqueSlug ?? throw new InvalidOperationException($"The {nameof(UniqueSlug)} has not been initialized yet.");
    set
    {
      if (_uniqueSlug != value)
      {
        _uniqueSlug = value;
        _updated.UniqueSlug = value;
      }
    }
  }
  private DisplayName? _displayName = null;
  public DisplayName? DisplayName
  {
    get => _displayName;
    set
    {
      if (_displayName != value)
      {
        _displayName = value;
        _updated.DisplayName = new Change<DisplayName>(value);
      }
    }
  }
  private Description? _description = null;
  public Description? Description
  {
    get => _description;
    set
    {
      if (_description != value)
      {
        _description = value;
        _updated.Description = new Change<Description>(value);
      }
    }
  }

  public Customization() : base()
  {
  }

  public Customization(CustomizationType type, Slug uniqueSlug, ActorId? actorId = null, CustomizationId? talentId = null) : base(talentId?.StreamId)
  {
    if (!Enum.IsDefined(type))
    {
      throw new ArgumentOutOfRangeException(nameof(type));
    }

    Raise(new CustomizationCreated(type, uniqueSlug), actorId);
  }
  protected virtual void Handle(CustomizationCreated @event)
  {
    Type = @event.Type;

    _uniqueSlug = @event.UniqueSlug;
  }

  public void Update(ActorId? actorId = null)
  {
    if (_updated.HasChanges)
    {
      Raise(_updated, actorId, DateTime.Now);
      _updated = new();
    }
  }
  protected virtual void Handle(CustomizationUpdated @event)
  {
    if (@event.UniqueSlug != null)
    {
      _uniqueSlug = @event.UniqueSlug;
    }
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName.Value;
    }
    if (@event.Description != null)
    {
      _description = @event.Description.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
