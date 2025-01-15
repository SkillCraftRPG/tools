namespace SkillCraft.Tools;

internal class Program
{
  public static async Task Main(string[] args)
  {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    Startup startup = new(builder.Configuration, builder.Environment);
    startup.ConfigureServices(builder.Services);

    WebApplication application = builder.Build();

    await startup.ConfigureAsync(application);

    application.Run();
  }
}
