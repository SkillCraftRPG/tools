using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Tools.Core;
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
    builder.HasIndex(x => x.MandatoryAttribute1);
    builder.HasIndex(x => x.MandatoryAttribute2);
    builder.HasIndex(x => x.OptionalAttribute1);
    builder.HasIndex(x => x.OptionalAttribute2);
    builder.HasIndex(x => x.DiscountedSkill1);
    builder.HasIndex(x => x.DiscountedSkill2);

    builder.Property(x => x.UniqueSlug).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisplayName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.MandatoryAttribute1).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Ability>());
    builder.Property(x => x.MandatoryAttribute2).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Ability>());
    builder.Property(x => x.OptionalAttribute1).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Ability>());
    builder.Property(x => x.OptionalAttribute2).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Ability>());
    builder.Property(x => x.DiscountedSkill1).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Skill>());
    builder.Property(x => x.DiscountedSkill2).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Skill>());
  }
}
