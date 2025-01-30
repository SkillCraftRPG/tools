using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class LanguageConfiguration : AggregateConfiguration<LanguageEntity>, IEntityTypeConfiguration<LanguageEntity>
{
  public override void Configure(EntityTypeBuilder<LanguageEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Languages.Table.Table ?? string.Empty, SkillCraftDb.Languages.Table.Schema);
    builder.HasKey(x => x.LanguageId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Property(x => x.UniqueSlug).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);

    builder.HasMany(x => x.Scripts).WithMany(x => x.Languages)
      .UsingEntity<LanguageScriptEntity>(builder =>
      {
        builder.ToTable(SkillCraftDb.LanguageScripts.Table.Table ?? string.Empty, SkillCraftDb.LanguageScripts.Table.Schema);
        builder.HasKey(x => new { x.LanguageId, x.ScriptId });
      });
  }
}
