using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure;

public class SkillCraftContext : DbContext
{
  public SkillCraftContext(DbContextOptions<SkillCraftContext> options) : base(options)
  {
  }

  internal DbSet<AspectEntity> Aspects => Set<AspectEntity>();
  internal DbSet<CasteEntity> Castes => Set<CasteEntity>();
  internal DbSet<CustomizationEntity> Customizations => Set<CustomizationEntity>();
  internal DbSet<EducationEntity> Educations => Set<EducationEntity>();
  internal DbSet<LanguageEntity> Languages => Set<LanguageEntity>();
  internal DbSet<LineageEntity> Lineages => Set<LineageEntity>();
  internal DbSet<LineageLanguageEntity> LineageLanguages => Set<LineageLanguageEntity>();
  internal DbSet<NatureEntity> Natures => Set<NatureEntity>();
  internal DbSet<SpecializationEntity> Specializations => Set<SpecializationEntity>();
  internal DbSet<SpecializationOptionalTalentEntity> SpecializationOptionalTalents => Set<SpecializationOptionalTalentEntity>();
  internal DbSet<TalentEntity> Talents => Set<TalentEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
