using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class LineageConfiguration : AggregateConfiguration<LineageEntity>, IEntityTypeConfiguration<LineageEntity>
{
  public override void Configure(EntityTypeBuilder<LineageEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Lineages.Table.Table ?? string.Empty, SkillCraftDb.Lineages.Table.Schema);
    builder.HasKey(x => x.LineageId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Property(x => x.UniqueSlug).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.SizeCategory).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<SizeCategory>());
    builder.Property(x => x.SizeRoll).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.StarvedRoll).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.SkinnyRoll).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.NormalRoll).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.OverweightRoll).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.ObeseRoll).HasMaxLength(byte.MaxValue);

    builder.HasOne(x => x.Parent).WithMany(x => x.Children)
      .HasPrincipalKey(x => x.LineageId).HasForeignKey(x => x.ParentId)
      .OnDelete(DeleteBehavior.Restrict);
    builder.HasMany(x => x.Traits).WithMany(x => x.Lineages)
      .UsingEntity<LineageTraitEntity>(builder =>
      {
        builder.ToTable(SkillCraftDb.LineageTraits.Table.Table ?? string.Empty, SkillCraftDb.LineageTraits.Table.Schema);
        builder.HasKey(x => new { x.LineageId, x.TraitId });
      });
    builder.HasMany(x => x.Languages).WithMany(x => x.Lineages)
      .UsingEntity<LineageLanguageEntity>(builder =>
      {
        builder.ToTable(SkillCraftDb.LineageLanguages.Table.Table ?? string.Empty, SkillCraftDb.LineageLanguages.Table.Schema);
        builder.HasKey(x => new { x.LineageId, x.LanguageId });
      });
  }
}
