using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Specializations.Events;
using SkillCraft.Tools.Core.Talents;

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

  public TalentId? RequiredTalentId { get; private set; }
  // TODO(fpion): OtherRequirements
  // TODO(fpion): OptionalTalentIds
  // TODO(fpion): OtherOptions
  private ReservedTalent? _reservedTalent = null;
  public ReservedTalent? ReservedTalent
  {
    get => _reservedTalent;
    set
    {
      if (_reservedTalent != value)
      {
        _reservedTalent = value;
        _updated.ReservedTalent = new Change<ReservedTalent>(value);
      }
    }
  }

  public Specialization() : base()
  {
  }

  public Specialization(int tier, Slug uniqueSlug, ActorId? actorId = null, SpecializationId? specializationId = null) : base(specializationId?.StreamId)
  {
    if (tier < 1 || tier > 3)
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

  public void SetRequiredTalent(Talent? requiredTalent)
  {
    if (requiredTalent != null && requiredTalent.Tier >= Tier)
    {
      throw new NotImplementedException(); // TODO(fpion): typed exception
    }

    if (RequiredTalentId != requiredTalent?.Id)
    {
      RequiredTalentId = requiredTalent?.Id;
      _updated.RequiredTalentId = new Change<TalentId?>(requiredTalent?.Id);
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

    if (@event.RequiredTalentId != null)
    {
      RequiredTalentId = @event.RequiredTalentId.Value;
    }
    // TODO(fpion): OtherRequirements
    // TODO(fpion): OptionalTalentIds
    // TODO(fpion): OtherOptions
    if (@event.ReservedTalent != null)
    {
      _reservedTalent = @event.ReservedTalent.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
