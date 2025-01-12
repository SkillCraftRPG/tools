using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Talents.Events;

namespace SkillCraft.Tools.Core.Talents;

public class Talent : AggregateRoot
{
  private TalentUpdated _updated = new();

  public new TalentId Id => new(base.Id);

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

  private bool _allowMultiplePurchases = false;
  public bool AllowMultiplePurchases
  {
    get => _allowMultiplePurchases;
    set
    {
      if (_allowMultiplePurchases != value)
      {
        _allowMultiplePurchases = value;
        _updated.AllowMultiplePurchases = value;
      }
    }
  }
  public TalentId? RequiredTalentId { get; private set; }
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

  public Talent() : base()
  {
  }

  public Talent(int tier, Slug uniqueSlug, ActorId? actorId = null, TalentId? talentId = null) : base(talentId?.StreamId)
  {
    if (tier < 0 || tier > 3)
    {
      throw new ArgumentOutOfRangeException(nameof(tier));
    }

    Raise(new TalentCreated(tier, uniqueSlug), actorId);
  }
  protected virtual void Handle(TalentCreated @event)
  {
    Tier = @event.Tier;

    _uniqueSlug = @event.UniqueSlug;
  }

  public void SetRequiredTalent(Talent? requiredTalent)
  {
    if (requiredTalent != null)
    {
      if (requiredTalent.Tier > Tier)
      {
        throw new RequiredTalentTierCannotExceedRequiringTalentTierException(this, requiredTalent, nameof(RequiredTalentId));
      }
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
  protected virtual void Handle(TalentUpdated @event)
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

    if (@event.AllowMultiplePurchases.HasValue)
    {
      _allowMultiplePurchases = @event.AllowMultiplePurchases.Value;
    }
    if (@event.RequiredTalentId != null)
    {
      RequiredTalentId = @event.RequiredTalentId.Value;
    }
    if (@event.Skill != null)
    {
      _skill = @event.Skill.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
