using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class CustomizationConfiguration : AggregateConfiguration<CustomizationEntity>, IEntityTypeConfiguration<CustomizationEntity>
{
  public override void Configure(EntityTypeBuilder<CustomizationEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Customizations.Table.Table ?? string.Empty, SkillCraftDb.Customizations.Table.Schema);
    builder.HasKey(x => x.CustomizationId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.Type);
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Property(x => x.Type).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<CustomizationType>());
    builder.Property(x => x.UniqueSlug).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisplayName).HasMaxLength(byte.MaxValue);
  }
}
