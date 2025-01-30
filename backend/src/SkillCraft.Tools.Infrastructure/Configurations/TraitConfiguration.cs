using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class TraitConfiguration : AggregateConfiguration<TraitEntity>, IEntityTypeConfiguration<TraitEntity>
{
  public override void Configure(EntityTypeBuilder<TraitEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Traits.Table.Table ?? string.Empty, SkillCraftDb.Traits.Table.Schema);
    builder.HasKey(x => x.TraitId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Property(x => x.UniqueSlug).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
  }
}
