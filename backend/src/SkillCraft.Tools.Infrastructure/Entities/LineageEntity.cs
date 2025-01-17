using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Lineages;
using SkillCraft.Tools.Core.Lineages.Events;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class LineageEntity : AggregateEntity
{
  public int LineageId { get; private set; }
  public Guid Id { get; private set; }

  public LineageEntity? Parent { get; private set; }
  public int? ParentId { get; private set; }
  public List<LineageEntity> Children { get; private set; } = [];

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public int Agility { get; private set; }
  public int Coordination { get; private set; }
  public int Intellect { get; private set; }
  public int Presence { get; private set; }
  public int Sensitivity { get; private set; }
  public int Spirit { get; private set; }
  public int Vigor { get; private set; }
  public int ExtraAttributes { get; private set; }
  public string? Traits { get; private set; }

  public List<LanguageEntity> Languages { get; private set; } = [];
  public int ExtraLanguages { get; private set; }
  public string? LanguagesText { get; private set; }

  public string? NamesText { get; private set; }
  public string? FamilyNames { get; private set; }
  public string? FemaleNames { get; private set; }
  public string? MaleNames { get; private set; }
  public string? UnisexNames { get; private set; }
  public string? CustomNames { get; private set; }

  public int WalkSpeed { get; private set; }
  public int ClimbSpeed { get; private set; }
  public int SwimSpeed { get; private set; }
  public int FlySpeed { get; private set; }
  public int HoverSpeed { get; private set; }
  public int BurrowSpeed { get; private set; }

  public SizeCategory SizeCategory { get; private set; }
  public string? SizeRoll { get; private set; }

  public string? StarvedRoll { get; private set; }
  public string? SkinnyRoll { get; private set; }
  public string? NormalRoll { get; private set; }
  public string? OverweightRoll { get; private set; }
  public string? ObeseRoll { get; private set; }

  public int? AdolescentAge { get; private set; }
  public int? AdultAge { get; private set; }
  public int? MatureAge { get; private set; }
  public int? VenerableAge { get; private set; }

  public LineageEntity(LineageEntity? parent, LineageCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    Parent = parent;
    ParentId = parent?.ParentId;

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private LineageEntity() : base()
  {
  }

  public void Update(IReadOnlyCollection<LanguageEntity> languages, LineageUpdated @event)
  {
    base.Update(@event);

    if (@event.UniqueSlug != null)
    {
      UniqueSlug = @event.UniqueSlug.Value;
    }
    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value?.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }

    if (@event.Attributes != null)
    {
      SetAttributes(@event.Attributes);
    }
    Dictionary<Guid, TraitModel> traits = GetTraits();
    foreach (KeyValuePair<Guid, Trait?> trait in @event.Traits)
    {
      if (trait.Value == null)
      {
        traits.Remove(trait.Key);
      }
      else
      {
        traits[trait.Key] = new TraitModel
        {
          Id = trait.Key,
          Name = trait.Value.Name.Value,
          Description = trait.Value.Description?.Value
        };
      }
    }
    SetTraits(traits);

    if (@event.Languages != null)
    {
      SetLanguages(languages, @event.Languages);
    }
    if (@event.Names != null)
    {
      SetNames(@event.Names);
    }

    if (@event.Speeds != null)
    {
      SetSpeeds(@event.Speeds);
    }
    if (@event.Size != null)
    {
      SetSize(@event.Size);
    }
    if (@event.Weight != null)
    {
      SetWeight(@event.Weight);
    }
    if (@event.Ages != null)
    {
      SetAges(@event.Ages);
    }
  }

  public AgesModel GetAges() => new()
  {
    Adolescent = AdolescentAge,
    Adult = AdultAge,
    Mature = MatureAge,
    Venerable = VenerableAge
  };
  private void SetAges(Ages ages)
  {
    AdolescentAge = ages.Adolescent;
    AdultAge = ages.Adult;
    MatureAge = ages.Mature;
    VenerableAge = ages.Venerable;
  }

  public AttributeBonusesModel GetAttributes() => new()
  {
    Agility = Agility,
    Coordination = Coordination,
    Intellect = Intellect,
    Presence = Presence,
    Sensitivity = Sensitivity,
    Spirit = Spirit,
    Vigor = Vigor,
    Extra = ExtraAttributes
  };
  private void SetAttributes(AttributeBonuses attributes)
  {
    Agility = attributes.Agility;
    Coordination = attributes.Coordination;
    Intellect = attributes.Intellect;
    Presence = attributes.Presence;
    Sensitivity = attributes.Sensitivity;
    Spirit = attributes.Spirit;
    Vigor = attributes.Vigor;
    ExtraAttributes = attributes.Extra;
  }

  public LanguagesModel GetLanguages() => new()
  {
    Extra = ExtraLanguages,
    Text = LanguagesText
  };
  private void SetLanguages(IReadOnlyCollection<LanguageEntity> entities, Core.Lineages.Languages languages)
  {
    Languages.Clear();
    Languages.AddRange(entities);

    ExtraLanguages = languages.Extra;
    LanguagesText = languages.Text;
  }

  public NamesModel GetNames()
  {
    NamesModel names = new()
    {
      Text = NamesText
    };
    if (FamilyNames != null)
    {
      names.Family.AddRange(JsonSerializer.Deserialize<IEnumerable<string>>(FamilyNames) ?? []);
    }
    if (FemaleNames != null)
    {
      names.Female.AddRange(JsonSerializer.Deserialize<IEnumerable<string>>(FemaleNames) ?? []);
    }
    if (MaleNames != null)
    {
      names.Male.AddRange(JsonSerializer.Deserialize<IEnumerable<string>>(MaleNames) ?? []);
    }
    if (UnisexNames != null)
    {
      names.Unisex.AddRange(JsonSerializer.Deserialize<IEnumerable<string>>(UnisexNames) ?? []);
    }
    if (CustomNames != null)
    {
      Dictionary<string, IEnumerable<string>>? custom = JsonSerializer.Deserialize<Dictionary<string, IEnumerable<string>>>(CustomNames);
      if (custom != null)
      {
        foreach (KeyValuePair<string, IEnumerable<string>> category in custom)
        {
          names.Custom.Add(new NameCategory(category));
        }
      }
    }
    return names;
  }
  private void SetNames(Names names)
  {
    NamesText = names.Text;
    FamilyNames = names.Family.Count == 0 ? null : JsonSerializer.Serialize(names.Family);
    FemaleNames = names.Female.Count == 0 ? null : JsonSerializer.Serialize(names.Female);
    MaleNames = names.Male.Count == 0 ? null : JsonSerializer.Serialize(names.Male);
    UnisexNames = names.Unisex.Count == 0 ? null : JsonSerializer.Serialize(names.Unisex);
    CustomNames = names.Custom.Count == 0 ? null : JsonSerializer.Serialize(names.Custom);
  }

  public SizeModel GetSize() => new()
  {
    Category = SizeCategory,
    Roll = SizeRoll
  };
  private void SetSize(Size size)
  {
    SizeCategory = size.Category;
    SizeRoll = size.Roll?.Value;
  }

  public SpeedsModel GetSpeeds() => new()
  {
    Walk = WalkSpeed,
    Climb = ClimbSpeed,
    Swim = SwimSpeed,
    Fly = FlySpeed,
    Hover = HoverSpeed,
    Burrow = BurrowSpeed
  };
  private void SetSpeeds(Speeds speeds)
  {
    WalkSpeed = speeds.Walk;
    ClimbSpeed = speeds.Climb;
    SwimSpeed = speeds.Swim;
    FlySpeed = speeds.Fly;
    HoverSpeed = speeds.Hover;
    BurrowSpeed = speeds.Burrow;
  }

  public WeightModel GetWeight() => new()
  {
    Starved = StarvedRoll,
    Skinny = SkinnyRoll,
    Normal = NormalRoll,
    Overweight = OverweightRoll,
    Obese = ObeseRoll
  };
  private void SetWeight(Weight weight)
  {
    StarvedRoll = weight.Starved?.Value;
    SkinnyRoll = weight.Skinny?.Value;
    NormalRoll = weight.Normal?.Value;
    OverweightRoll = weight.Overweight?.Value;
    ObeseRoll = weight.Obese?.Value;
  }

  public Dictionary<Guid, TraitModel> GetTraits()
  {
    return (Traits == null ? null : JsonSerializer.Deserialize<Dictionary<Guid, TraitModel>>(Traits)) ?? [];
  }
  private void SetTraits(Dictionary<Guid, TraitModel> traits)
  {
    Traits = traits.Count < 1 ? null : JsonSerializer.Serialize(traits);
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
