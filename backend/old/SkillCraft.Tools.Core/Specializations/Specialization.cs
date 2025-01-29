using Logitar;
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
  private readonly HashSet<OtherRequirement> _otherRequirements = [];
  public IReadOnlyCollection<OtherRequirement> OtherRequirements => _otherRequirements.ToList().AsReadOnly();

  private readonly HashSet<TalentId> _optionalTalentIds = [];
  public IReadOnlyCollection<TalentId> OptionalTalentIds => _optionalTalentIds.ToList().AsReadOnly();
  private readonly HashSet<OtherOption> _otherOptions = [];
  public IReadOnlyCollection<OtherOption> OtherOptions => _otherOptions.ToList().AsReadOnly();

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

  public void AddOptionalTalent(Talent talent)
  {
    if (talent.Tier >= Tier)
    {
      throw new NotImplementedException(); // ISSUE #56: https://github.com/SkillCraftRPG/tools/issues/56
    }

    if (_optionalTalentIds.Add(talent.Id))
    {
      _updated.OptionalTalentIds[talent.Id] = true;
    }
  }

  public bool HasOptionalTalent(Talent talent) => HasOptionalTalent(talent.Id);
  public bool HasOptionalTalent(TalentId talentId) => _optionalTalentIds.Contains(talentId);

  public void RemoveOptionalTalent(Talent talent)
  {
    RemoveOptionalTalent(talent.Id);
  }
  public void RemoveOptionalTalent(TalentId talentId)
  {
    if (_optionalTalentIds.Remove(talentId))
    {
      _updated.OptionalTalentIds[talentId] = false;
    }
  }

  public void SetOtherOptions(IEnumerable<OtherOption> otherOptions)
  {
    otherOptions = otherOptions.Distinct();
    if (!_otherOptions.SequenceEqual(otherOptions))
    {
      _otherOptions.Clear();
      _otherOptions.AddRange(otherOptions);

      _updated.OtherOptions = otherOptions.ToList().AsReadOnly();
    }
  }

  public void SetOtherRequirements(IEnumerable<OtherRequirement> otherRequirements)
  {
    otherRequirements = otherRequirements.Distinct();
    if (!_otherRequirements.SequenceEqual(otherRequirements))
    {
      _otherRequirements.Clear();
      _otherRequirements.AddRange(otherRequirements);

      _updated.OtherRequirements = otherRequirements.ToList().AsReadOnly();
    }
  }

  public void SetRequiredTalent(Talent? requiredTalent)
  {
    if (requiredTalent != null && requiredTalent.Tier >= Tier)
    {
      throw new NotImplementedException(); // ISSUE #56: https://github.com/SkillCraftRPG/tools/issues/56
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
    if (@event.OtherRequirements != null)
    {
      _otherRequirements.Clear();
      _otherRequirements.AddRange(@event.OtherRequirements);
    }

    foreach (KeyValuePair<TalentId, bool> optionalTalentId in @event.OptionalTalentIds)
    {
      if (optionalTalentId.Value)
      {
        _optionalTalentIds.Add(optionalTalentId.Key);
      }
      else
      {
        _optionalTalentIds.Remove(optionalTalentId.Key);
      }
    }
    if (@event.OtherOptions != null)
    {
      _otherOptions.Clear();
      _otherOptions.AddRange(@event.OtherOptions);
    }

    if (@event.ReservedTalent != null)
    {
      _reservedTalent = @event.ReservedTalent.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
