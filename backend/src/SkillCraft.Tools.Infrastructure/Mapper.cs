using Logitar;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Actors;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure;

internal class Mapper
{
  private readonly Dictionary<ActorId, ActorModel> _actors = [];
  private readonly ActorModel _system = ActorModel.System;

  public Mapper()
  {
  }

  public Mapper(IEnumerable<ActorModel> actors)
  {
    foreach (ActorModel actor in actors)
    {
      ActorId id = new(actor.Id);
      _actors[id] = actor;
    }
  }

  public AspectModel ToAspect(AspectEntity source)
  {
    AspectModel destination = new()
    {
      UniqueSlug = source.UniqueSlug,
      DisplayName = source.DisplayName,
      Description = source.Description,
      Attributes = source.GetAttributeSelection(),
      Skills = source.GetSkillSelection()
    };

    MapAggregate(source, destination);

    return destination;
  }

  public CasteModel ToCaste(CasteEntity source)
  {
    CasteModel destination = new()
    {
      UniqueSlug = source.UniqueSlug,
      DisplayName = source.DisplayName,
      Description = source.Description,
      Skill = source.Skill,
      WealthRoll = source.WealthRoll
    };

    foreach (KeyValuePair<Guid, FeatureModel> feature in source.GetFeatures())
    {
      destination.Features.Add(feature.Value);
    }

    MapAggregate(source, destination);

    return destination;
  }

  public CustomizationModel ToCustomization(CustomizationEntity source)
  {
    CustomizationModel destination = new()
    {
      Type = source.Type,
      UniqueSlug = source.UniqueSlug,
      DisplayName = source.DisplayName,
      Description = source.Description
    };

    MapAggregate(source, destination);

    return destination;
  }

  public EducationModel ToEducation(EducationEntity source)
  {
    EducationModel destination = new()
    {
      UniqueSlug = source.UniqueSlug,
      DisplayName = source.DisplayName,
      Description = source.Description,
      Skill = source.Skill,
      WealthMultiplier = source.WealthMultiplier
    };

    MapAggregate(source, destination);

    return destination;
  }

  public LanguageModel ToLanguage(LanguageEntity source)
  {
    LanguageModel destination = new()
    {
      UniqueSlug = source.UniqueSlug,
      DisplayName = source.DisplayName,
      Description = source.Description,
      Script = source.Script,
      TypicalSpeakers = source.TypicalSpeakers
    };

    MapAggregate(source, destination);

    return destination;
  }

  public NatureModel ToNature(NatureEntity source)
  {
    NatureModel destination = new()
    {
      UniqueSlug = source.UniqueSlug,
      DisplayName = source.DisplayName,
      Description = source.Description,
      Attribute = source.Attribute
    };

    if (source.Gift != null)
    {
      destination.Gift = ToCustomization(source.Gift);
    }

    MapAggregate(source, destination);

    return destination;
  }

  public SpecializationModel ToSpecialization(SpecializationEntity source)
  {
    SpecializationModel destination = new()
    {
      Tier = source.Tier,
      UniqueSlug = source.UniqueSlug,
      DisplayName = source.DisplayName,
      Description = source.Description,
      // TODO(fpion): OtherRequirements
      // TODO(fpion): OptionalTalentIds
      // TODO(fpion): OtherOptions
      ReservedTalent = source.GetReservedTalent()
    };

    if (source.RequiredTalent != null)
    {
      destination.RequiredTalent = ToTalent(source.RequiredTalent);
    }

    MapAggregate(source, destination);

    return destination;
  }

  public TalentModel ToTalent(TalentEntity source)
  {
    TalentModel destination = new()
    {
      Tier = source.Tier,
      UniqueSlug = source.UniqueSlug,
      DisplayName = source.DisplayName,
      Description = source.Description,
      AllowMultiplePurchases = source.AllowMultiplePurchases,
      Skill = source.Skill
    };

    if (source.RequiredTalent != null)
    {
      destination.RequiredTalent = ToTalent(source.RequiredTalent);
    }

    MapAggregate(source, destination);

    return destination;
  }

  private void MapAggregate(AggregateEntity source, AggregateModel destination)
  {
    try
    {
      destination.Id = new StreamId(source.StreamId).ToGuid();
    }
    catch (Exception)
    {
    }
    destination.Version = source.Version;
    destination.CreatedBy = TryFindActor(source.CreatedBy) ?? _system;
    destination.CreatedOn = source.CreatedOn.AsUniversalTime();
    destination.UpdatedBy = TryFindActor(source.UpdatedBy) ?? _system;
    destination.UpdatedOn = source.UpdatedOn.AsUniversalTime();
  }

  private ActorModel? TryFindActor(string? id) => TryFindActor(id == null ? null : new ActorId(id));
  private ActorModel? TryFindActor(ActorId? id) => id.HasValue && _actors.TryGetValue(id.Value, out ActorModel? actor) ? actor : null;
}
