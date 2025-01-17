using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Lineages.Events;

namespace SkillCraft.Tools.Core.Lineages;

public class Lineage : AggregateRoot
{
  private LineageUpdated _updated = new();

  public new LineageId Id => new(base.Id);

  public LineageId? ParentId { get; private set; }

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

  private AttributeBonuses _attributes = new();
  public AttributeBonuses Attributes
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
  private readonly Dictionary<Guid, Trait> _traits = [];
  public IReadOnlyDictionary<Guid, Trait> Traits => _traits.AsReadOnly();

  private Languages _languages = new();
  public Languages Languages
  {
    get => _languages;
    set
    {
      if (_languages != value)
      {
        _languages = value;
        _updated.Languages = value;
      }
    }
  }
  private Names _names = new();
  public Names Names
  {
    get => _names;
    set
    {
      if (_names != value)
      {
        _names = value;
        _updated.Names = value;
      }
    }
  }

  private Speeds _speeds = new();
  public Speeds Speeds
  {
    get => _speeds;
    set
    {
      if (_speeds != value)
      {
        _speeds = value;
        _updated.Speeds = value;
      }
    }
  }
  private Size _size = new();
  public Size Size
  {
    get => _size;
    set
    {
      if (_size != value)
      {
        _size = value;
        _updated.Size = value;
      }
    }
  }
  private Weight _weight = new();
  public Weight Weight
  {
    get => _weight;
    set
    {
      if (_weight != value)
      {
        _weight = value;
        _updated.Weight = value;
      }
    }
  }
  private Ages _ages = new();
  public Ages Ages
  {
    get => _ages;
    set
    {
      if (_ages != value)
      {
        _ages = value;
        _updated.Ages = value;
      }
    }
  }
  public Lineage() : base()
  {
  }

  public Lineage(Slug uniqueSlug, Lineage? parent = null, ActorId? actorId = null, LineageId? lineageId = null) : base(lineageId?.StreamId)
  {
    if (parent != null && parent.ParentId.HasValue)
    {
      throw new InvalidParentLineageException(parent, "ParentId");
    }

    Raise(new LineageCreated(parent?.Id, uniqueSlug), actorId);
  }
  protected virtual void Handle(LineageCreated @event)
  {
    ParentId = @event.ParentId;

    _uniqueSlug = @event.UniqueSlug;
  }

  public void AddTrait(Trait trait)
  {
    SetTrait(Guid.NewGuid(), trait);
  }
  public void RemoveTrait(Guid id)
  {
    if (_traits.Remove(id))
    {
      _updated.Traits[id] = null;
    }
  }
  public void SetTrait(Guid id, Trait trait)
  {
    if (!_traits.TryGetValue(id, out Trait? existingTrait) || existingTrait != trait)
    {
      _traits[id] = trait;
      _updated.Traits[id] = trait;
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
  protected virtual void Handle(LineageUpdated @event)
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
    foreach (KeyValuePair<Guid, Trait?> trait in @event.Traits)
    {
      if (trait.Value == null)
      {
        _traits.Remove(trait.Key);
      }
      else
      {
        _traits[trait.Key] = trait.Value;
      }
    }

    if (@event.Languages != null)
    {
      _languages = @event.Languages;
    }
    if (@event.Names != null)
    {
      _names = @event.Names;
    }

    if (@event.Speeds != null)
    {
      _speeds = @event.Speeds;
    }
    if (@event.Size != null)
    {
      _size = @event.Size;
    }
    if (@event.Weight != null)
    {
      _weight = @event.Weight;
    }
    if (@event.Ages != null)
    {
      _ages = @event.Ages;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
