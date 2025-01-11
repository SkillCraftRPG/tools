namespace SkillCraft.Tools;

internal class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddControllers();
  }

  public override void Configure(IApplicationBuilder builder)
  {
    Configure((WebApplication)builder);
  }
  public void Configure(WebApplication application)
  {
    application.UseHttpsRedirection();

    application.MapControllers();
  }
}
