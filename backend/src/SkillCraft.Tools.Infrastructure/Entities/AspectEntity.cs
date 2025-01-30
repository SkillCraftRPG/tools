using Logitar.Cms.Core.Contents.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using SkillCraft.Tools.Core;
using Attribute = SkillCraft.Tools.Core.Attribute;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class AspectEntity : AggregateEntity
{
  public int AspectId { get; private set; }
  public Guid Id { get; private set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Attribute? MandatoryAttribute1 { get; set; }
  public Attribute? MandatoryAttribute2 { get; set; }
  public Attribute? OptionalAttribute1 { get; set; }
  public Attribute? OptionalAttribute2 { get; set; }
  public Skill? DiscountedSkill1 { get; set; }
  public Skill? DiscountedSkill2 { get; set; }

  public AspectEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();
  }

  private AspectEntity() : base()
  {
  }

  public void Update(ContentLocalePublished @event)
  {
    base.Update(@event);
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
