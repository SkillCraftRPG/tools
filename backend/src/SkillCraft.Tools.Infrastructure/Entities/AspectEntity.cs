using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Aspects.Events;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class AspectEntity : AggregateEntity
{
  public int AspectId { get; private set; }
  public Guid Id { get; private set; }

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public Ability? MandatoryAttribute1 { get; private set; }
  public Ability? MandatoryAttribute2 { get; private set; }
  public Ability? OptionalAttribute1 { get; private set; }
  public Ability? OptionalAttribute2 { get; private set; }
  public Skill? DiscountedSkill1 { get; private set; }
  public Skill? DiscountedSkill2 { get; private set; }

  public AspectEntity(AspectCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    UniqueSlug = @event.UniqueSlug.Value;
  }

  private AspectEntity() : base()
  {
  }

  public void Update(AspectUpdated @event)
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
      MandatoryAttribute1 = @event.Attributes.Mandatory1;
      MandatoryAttribute2 = @event.Attributes.Mandatory2;
      OptionalAttribute1 = @event.Attributes.Optional1;
      OptionalAttribute2 = @event.Attributes.Optional2;
    }
    if (@event.Skills != null)
    {
      DiscountedSkill1 = @event.Skills.Discounted1;
      DiscountedSkill2 = @event.Skills.Discounted2;
    }
  }

  public AttributeSelectionModel GetAttributeSelection() => new()
  {
    Mandatory1 = MandatoryAttribute1,
    Mandatory2 = MandatoryAttribute2,
    Optional1 = OptionalAttribute1,
    Optional2 = OptionalAttribute2
  };
  public SkillSelectionModel GetSkillSelection() => new()
  {
    Discounted1 = DiscountedSkill1,
    Discounted2 = DiscountedSkill2
  };

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
