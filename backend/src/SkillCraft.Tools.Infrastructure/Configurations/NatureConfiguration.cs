using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Tools.Infrastructure.Entities;
using Attribute = SkillCraft.Tools.Core.Attribute;

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

    builder.Property(x => x.UniqueSlug).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.Attribute).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Attribute>());

    builder.HasOne(x => x.Gift).WithMany(x => x.Natures)
      .HasPrincipalKey(x => x.CustomizationId).HasForeignKey(x => x.GiftId)
      .OnDelete(DeleteBehavior.SetNull);
  }
}
