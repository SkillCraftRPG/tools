using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Lineages;
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

    builder.Property(x => x.UniqueSlug).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisplayName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.LanguagesText).HasMaxLength(Languages.MaximumLength);
    builder.Property(x => x.NamesText).HasMaxLength(Names.MaximumLength);
    builder.Property(x => x.SizeCategory).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<SizeCategory>());
    builder.Property(x => x.SizeRoll).HasMaxLength(Roll.MaximumLength);
    builder.Property(x => x.StarvedRoll).HasMaxLength(Roll.MaximumLength);
    builder.Property(x => x.SkinnyRoll).HasMaxLength(Roll.MaximumLength);
    builder.Property(x => x.NormalRoll).HasMaxLength(Roll.MaximumLength);
    builder.Property(x => x.OverweightRoll).HasMaxLength(Roll.MaximumLength);
    builder.Property(x => x.ObeseRoll).HasMaxLength(Roll.MaximumLength);

    builder.HasOne(x => x.Parent).WithMany(x => x.Children)
      .HasPrincipalKey(x => x.LineageId).HasForeignKey(x => x.ParentId)
      .OnDelete(DeleteBehavior.Restrict);
    builder.HasMany(x => x.Languages).WithMany(x => x.Lineages)
      .UsingEntity<LineageLanguageEntity>(builder =>
      {
        builder.ToTable(SkillCraftDb.LineageLanguages.Table.Table ?? string.Empty, SkillCraftDb.LineageLanguages.Table.Schema);
        builder.HasKey(x => new { x.LineageId, x.LanguageId });
      });
  }
}
