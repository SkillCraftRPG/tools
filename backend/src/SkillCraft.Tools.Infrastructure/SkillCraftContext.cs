using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure;

public class SkillCraftContext : DbContext
{
  public SkillCraftContext(DbContextOptions<SkillCraftContext> options) : base(options)
  {
  }

  internal DbSet<CustomizationEntity> Customizations => Set<CustomizationEntity>();
  internal DbSet<TalentEntity> Talents => Set<TalentEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
