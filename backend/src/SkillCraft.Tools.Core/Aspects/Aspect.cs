using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Aspects.Events;

namespace SkillCraft.Tools.Core.Aspects;

public class Aspect : AggregateRoot
{
  private AspectUpdated _updated = new();

  public new AspectId Id => new(base.Id);

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

  private AttributeSelection _attributes = new();
  public AttributeSelection Attributes
  {
    get => _attributes;
    set
    {
      if (_attributes != value)
      {
        _attributes = value;
        _updated.Attributes = value;
      }
    }
  }
  private SkillSelection _skills = new();
  public SkillSelection Skills
  {
    get => _skills;
    set
    {
      if (_skills != value)
      {
        _skills = value;
        _updated.Skills = value;
      }
    }
  }

  public Aspect() : base()
  {
  }

  public Aspect(Slug uniqueSlug, ActorId? actorId = null, AspectId? aspectId = null) : base(aspectId?.StreamId)
  {
    Raise(new AspectCreated(uniqueSlug), actorId);
  }
  protected virtual void Handle(AspectCreated @event)
  {
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
  protected virtual void Handle(AspectUpdated @event)
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

    if (@event.Attributes != null)
    {
      _attributes = @event.Attributes;
    }
    if (@event.Skills != null)
    {
      _skills = @event.Skills;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
