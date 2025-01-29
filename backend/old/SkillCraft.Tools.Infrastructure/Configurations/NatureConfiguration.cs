using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class NatureConfiguration : AggregateConfiguration<NatureEntity>, IEntityTypeConfiguration<NatureEntity>
{
  public override void Configure(EntityTypeBuilder<NatureEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Natures.Table.Table ?? string.Empty, SkillCraftDb.Natures.Table.Schema);
    builder.HasKey(x => x.NatureId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.Attribute);
    builder.HasIndex(x => x.GiftId);

    builder.Property(x => x.UniqueSlug).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisplayName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Attribute).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Ability>());

    builder.HasOne(x => x.Gift).WithMany(x => x.Natures)
      .HasPrincipalKey(x => x.CustomizationId).HasForeignKey(x => x.GiftId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
