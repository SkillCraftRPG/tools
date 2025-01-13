namespace SkillCraft.Tools.DataTransform.Worker;

internal class Program
{
  public static void Main(string[] args)
  {
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddHostedService<DataTransformWorker>();
    builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    IHost host = builder.Build();
    host.Run();
  }
}
