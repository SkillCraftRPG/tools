using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class FeatureConfiguration : AggregateConfiguration<FeatureEntity>, IEntityTypeConfiguration<FeatureEntity>
{
  public override void Configure(EntityTypeBuilder<FeatureEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Features.Table.Table ?? string.Empty, SkillCraftDb.Features.Table.Schema);
    builder.HasKey(x => x.FeatureId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Property(x => x.UniqueSlug).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
  }
}
