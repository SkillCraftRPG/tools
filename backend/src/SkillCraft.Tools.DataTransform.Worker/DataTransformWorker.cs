using MediatR;
using SkillCraft.Tools.DataTransform.Worker.Commands;
using System.Text.Json.Serialization;

namespace SkillCraft.Tools.DataTransform.Worker;

public class DataTransformWorker : BackgroundService
{
  private readonly IConfiguration _configuration;
  private readonly IHostApplicationLifetime _hostApplicationLifetime;
  private readonly ILogger<DataTransformWorker> _logger;
  private readonly IServiceProvider _serviceProvider;

  public DataTransformWorker(
    IConfiguration configuration,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<DataTransformWorker> logger,
    IServiceProvider serviceProvider)
  {
    _configuration = configuration;
    _hostApplicationLifetime = hostApplicationLifetime;
    _logger = logger;
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Worker executing at {Timestamp}.", DateTime.Now);

    Stopwatch chrono = Stopwatch.StartNew();
    bool hasFailed = false;
    try
    {
      string? encodingName = _configuration.GetValue<string>("Encoding");
      Encoding encoding = string.IsNullOrWhiteSpace(encodingName) ? Encoding.Default : Encoding.GetEncoding(encodingName.Trim());

      JsonSerializerOptions serializerOptions = new()
      {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
      };
      serializerOptions.Converters.Add(new JsonStringEnumConverter());

      using IServiceScope scope = _serviceProvider.CreateScope();
      IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

      Directory.CreateDirectory("output");

      ConvertAspectsCommand aspects = new(encoding, serializerOptions);
      await mediator.Send(aspects, cancellationToken);

      ConvertCastesCommand castes = new(encoding, serializerOptions);
      await mediator.Send(castes, cancellationToken);

      ConvertCustomizationsCommand customizations = new(encoding, serializerOptions);
      await mediator.Send(customizations, cancellationToken);

      ConvertEducationsCommand educations = new(encoding, serializerOptions);
      await mediator.Send(educations, cancellationToken);

      ConvertNaturesCommand natures = new(encoding, serializerOptions);
      await mediator.Send(natures, cancellationToken);
    }
    catch (Exception exception)
    {
      Environment.ExitCode = exception.HResult;

      hasFailed = true;
      _logger.LogError(exception, "An unhandled exception occurred.");
    }
    finally
    {
      chrono.Stop();

      if (hasFailed)
      {
        _logger.LogError("The operation failed with errors after {Milliseconds}ms.", chrono.ElapsedMilliseconds);
      }
      else
      {
        _logger.LogInformation("The operation completed successfully in {Milliseconds}ms.", chrono.ElapsedMilliseconds);
      }

      _hostApplicationLifetime.StopApplication();
    }
  }
}
