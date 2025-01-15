using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Specializations.Events;

namespace SkillCraft.Tools.Core.Specializations;

public class Specialization : AggregateRoot
{
  private SpecializationUpdated _updated = new();

  public new SpecializationId Id => new(base.Id);

  public int Tier { get; private set; }

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

  // TODO(fpion): RequiredTalentId
  // TODO(fpion): OtherRequirements
  // TODO(fpion): OptionalTalentIds
  // TODO(fpion): OtherOptions
  // TODO(fpion): ReservedTalent

  public Specialization() : base()
  {
  }

  public Specialization(int tier, Slug uniqueSlug, ActorId? actorId = null, SpecializationId? specializationId = null) : base(specializationId?.StreamId)
  {
    if (tier < 0 || tier > 3)
    {
      throw new ArgumentOutOfRangeException(nameof(tier));
    }

    Raise(new SpecializationCreated(tier, uniqueSlug), actorId);
  }
  protected virtual void Handle(SpecializationCreated @event)
  {
    Tier = @event.Tier;

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
  protected virtual void Handle(SpecializationUpdated @event)
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

    // TODO(fpion): RequiredTalentId
    // TODO(fpion): OtherRequirements
    // TODO(fpion): OptionalTalentIds
    // TODO(fpion): OtherOptions
    // TODO(fpion): ReservedTalent
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
