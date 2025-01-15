using SkillCraft.Tools.Core.Specializations;
using SkillCraft.Tools.Core.Specializations.Events;
using SkillCraft.Tools.Core.Specializations.Models;
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

  // TODO(fpion): RequiredTalentId
  // TODO(fpion): OtherRequirements
  // TODO(fpion): OptionalTalentIds
  // TODO(fpion): OtherOptions
  public string? ReservedTalentName { get; private set; }
  public string? ReservedTalentDescriptions { get; private set; }

  public SpecializationEntity(SpecializationCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    Tier = @event.Tier;

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private SpecializationEntity() : base()
  {
  }

  public void Update(SpecializationUpdated @event)
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

    // TODO(fpion): RequiredTalentId
    // TODO(fpion): OtherRequirements
    // TODO(fpion): OptionalTalentIds
    // TODO(fpion): OtherOptions
    if (@event.ReservedTalent != null)
    {
      SetReservedTalent(@event.ReservedTalent.Value);
    }
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
