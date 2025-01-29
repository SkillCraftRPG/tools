using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Specializations;
using SkillCraft.Tools.Core.Specializations.Events;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Core.Talents;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class SpecializationEntity : AggregateEntity
{
  public int SpecializationId { get; private set; }
  public Guid Id { get; private set; }

  public int Tier { get; private set; }

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public TalentEntity? RequiredTalent { get; private set; }
  public int? RequiredTalentId { get; private set; }
  public string? OtherRequirements { get; private set; }

  public string? OtherOptions { get; private set; }

  public string? ReservedTalentName { get; private set; }
  public string? ReservedTalentDescriptions { get; private set; }

  public List<TalentEntity> OptionalTalents { get; private set; } = [];

  public SpecializationEntity(SpecializationCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    Tier = @event.Tier;

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private SpecializationEntity() : base()
  {
  }

  public void Update(Dictionary<Guid, TalentEntity> talents, SpecializationUpdated @event)
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

    if (@event.RequiredTalentId != null)
    {
      TalentEntity? requiredTalent = @event.RequiredTalentId.Value.HasValue
        ? talents[@event.RequiredTalentId.Value.Value.ToGuid()]
        : null;
      RequiredTalent = requiredTalent;
      RequiredTalentId = requiredTalent?.TalentId;
    }
    if (@event.OtherRequirements != null)
    {
      SetOtherRequirements(@event.OtherRequirements);
    }

    foreach (KeyValuePair<TalentId, bool> optionalTalentId in @event.OptionalTalentIds)
    {
      TalentEntity talent = talents[optionalTalentId.Key.ToGuid()];
      if (optionalTalentId.Value)
      {
        OptionalTalents.Add(talent);
      }
      else
      {
        OptionalTalents.Remove(talent);
      }
    }
    if (@event.OtherOptions != null)
    {
      SetOtherOptions(@event.OtherOptions);
    }

    if (@event.ReservedTalent != null)
    {
      SetReservedTalent(@event.ReservedTalent.Value);
    }
  }

  public override IReadOnlyCollection<ActorId> GetActorIds()
  {
    List<ActorId> actorIds = [.. base.GetActorIds()];
    if (RequiredTalent != null)
    {
      actorIds.AddRange(RequiredTalent.GetActorIds());
    }
    foreach (TalentEntity optionalTalent in OptionalTalents)
    {
      actorIds.AddRange(optionalTalent.GetActorIds());
    }
    return actorIds.AsReadOnly();
  }

  public IReadOnlyCollection<string> GetOtherRequirements()
  {
    return (OtherRequirements == null ? null : JsonSerializer.Deserialize<IReadOnlyCollection<string>>(OtherRequirements)) ?? [];
  }
  private void SetOtherRequirements(IReadOnlyCollection<OtherRequirement> otherRequirements)
  {
    OtherRequirements = otherRequirements.Count < 1 ? null : JsonSerializer.Serialize(otherRequirements.Select(x => x.Value));
  }

  public IReadOnlyCollection<string> GetOtherOptions()
  {
    return (OtherOptions == null ? null : JsonSerializer.Deserialize<IReadOnlyCollection<string>>(OtherOptions)) ?? [];
  }
  private void SetOtherOptions(IReadOnlyCollection<OtherOption> otherOptions)
  {
    OtherOptions = otherOptions.Count < 1 ? null : JsonSerializer.Serialize(otherOptions.Select(x => x.Value));
  }

  public ReservedTalentModel? GetReservedTalent()
  {
    if (ReservedTalentName == null)
    {
      return null;
    }

    IEnumerable<string>? descriptions = ReservedTalentDescriptions == null ? null : JsonSerializer.Deserialize<IEnumerable<string>>(ReservedTalentDescriptions);
    return new ReservedTalentModel(ReservedTalentName, descriptions);
  }
  private void SetReservedTalent(ReservedTalent? reservedTalent)
  {
    if (reservedTalent == null)
    {
      ReservedTalentName = null;
      ReservedTalentDescriptions = null;
    }
    else
    {
      ReservedTalentName = reservedTalent.Name.Value;
      ReservedTalentDescriptions = reservedTalent.Descriptions.Count < 1
        ? null
        : JsonSerializer.Serialize(reservedTalent.Descriptions.Select(description => description.Value).Distinct());
    }
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
