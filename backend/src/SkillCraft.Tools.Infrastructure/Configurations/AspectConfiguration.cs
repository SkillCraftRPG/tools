using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class AspectConfiguration : AggregateConfiguration<AspectEntity>, IEntityTypeConfiguration<AspectEntity>
{
  public override void Configure(EntityTypeBuilder<AspectEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Aspects.Table.Table ?? string.Empty, SkillCraftDb.Aspects.Table.Schema);
    builder.HasKey(x => x.AspectId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    // TODO(fpion): Attributes
    // TODO(fpion): Skills

    builder.Property(x => x.UniqueSlug).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisplayName).HasMaxLength(byte.MaxValue);
    // TODO(fpion): Attributes
    // TODO(fpion): Skills
  }
}
