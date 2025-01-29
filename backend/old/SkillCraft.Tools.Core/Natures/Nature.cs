using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Natures.Events;

namespace SkillCraft.Tools.Core.Natures;

public class Nature : AggregateRoot
{
  private NatureUpdated _updated = new();

  public new NatureId Id => new(base.Id);

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

  private Ability? _attribute = null;
  public Ability? Attribute
  {
    get => _attribute;
    set
    {
      if (value.HasValue && !Enum.IsDefined(value.Value))
      {
        throw new ArgumentOutOfRangeException(nameof(Attribute));
      }

      if (_attribute != value)
      {
        _attribute = value;
        _updated.Attribute = new Change<Ability?>(value);
      }
    }
  }
  public CustomizationId? GiftId { get; private set; }

  public Nature() : base()
  {
  }

  public Nature(Slug uniqueSlug, ActorId? actorId = null, NatureId? natureId = null) : base(natureId?.StreamId)
  {
    Raise(new NatureCreated(uniqueSlug), actorId);
  }
  protected virtual void Handle(NatureCreated @event)
  {
    _uniqueSlug = @event.UniqueSlug;
  }

  public void SetGift(Customization? gift)
  {
    if (gift != null && gift.Type != CustomizationType.Gift)
    {
      throw new CustomizationIsNotGiftException(gift, nameof(GiftId));
    }

    if (GiftId != gift?.Id)
    {
      GiftId = gift?.Id;
      _updated.GiftId = new Change<CustomizationId?>(gift?.Id);
    }
  }

  public void Update(ActorId? actorId = null)
  {
    if (_updated.HasChanges)
    {
      Raise(_updated, actorId, DateTime.Now);
      _updated = new();
    }
  }
  protected virtual void Handle(NatureUpdated @event)
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

    if (@event.Attribute != null)
    {
      _attribute = @event.Attribute.Value;
    }
    if (@event.GiftId != null)
    {
      GiftId = @event.GiftId.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
