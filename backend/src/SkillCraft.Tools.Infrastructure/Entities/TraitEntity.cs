using Logitar.Cms.Core.Contents.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class TraitEntity : AggregateEntity
{
  public int TraitId { get; private set; }
  public Guid Id { get; private set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public List<LineageEntity> Lineages { get; private set; } = [];

  public TraitEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();
  }

  private TraitEntity() : base()
  {
  }

  public void Update(ContentLocalePublished @event)
  {
    base.Update(@event);
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
