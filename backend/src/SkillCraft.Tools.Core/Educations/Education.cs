using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Educations.Events;

namespace SkillCraft.Tools.Core.Educations;

public class Education : AggregateRoot
{
  private EducationUpdated _updated = new();

  public new EducationId Id => new(base.Id);

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

  private Skill? _skill = null;
  public Skill? Skill
  {
    get => _skill;
    set
    {
      if (value.HasValue && !Enum.IsDefined(value.Value))
      {
        throw new ArgumentOutOfRangeException(nameof(Skill));
      }

      if (_skill != value)
      {
        _skill = value;
        _updated.Skill = new Change<Skill?>(value);
      }
    }
  }
  // TODO(fpion): WealthMultiplier

  public Education() : base()
  {
  }

  public Education(Slug uniqueSlug, ActorId? actorId = null, EducationId? educationId = null) : base(educationId?.StreamId)
  {
    Raise(new EducationCreated(uniqueSlug), actorId);
  }
  protected virtual void Handle(EducationCreated @event)
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
  protected virtual void Handle(EducationUpdated @event)
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

    if (@event.Skill != null)
    {
      _skill = @event.Skill.Value;
    }
    // TODO(fpion): WealthMultiplier
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
