using Logitar.Portal.Client;
using MediatR;
using SkillCraft.Tools.Seeding.Worker.Backend.Tasks;
using SkillCraft.Tools.Seeding.Worker.Portal.Tasks;

namespace SkillCraft.Tools.Seeding.Worker;

internal class SeedingWorker : BackgroundService
{
  private const string GenericErrorMessage = "An unhanded exception occurred.";
  private const int MaximumAttempts = 24;
  private const int MillisecondsDelay = 5000;

  private readonly IHostApplicationLifetime _hostApplicationLifetime;
  private readonly ILogger<SeedingWorker> _logger;
  private readonly IPortalSettings _portalSettings;
  private readonly IServiceProvider _serviceProvider;

  private IPublisher? _publisher = null;
  private IPublisher Publisher => _publisher ?? throw new InvalidOperationException($"The {nameof(Publisher)} has not been initialized yet.");

  private LogLevel _result = LogLevel.Information; // NOTE(fpion): "Information" means success.

  public SeedingWorker(
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<SeedingWorker> logger,
    IPortalSettings portalSettings,
    IServiceProvider serviceProvider)
  {
    _hostApplicationLifetime = hostApplicationLifetime;
    _logger = logger;
    _portalSettings = portalSettings;
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    Stopwatch chrono = Stopwatch.StartNew();
    _logger.LogInformation("Worker executing at {Timestamp}.", DateTimeOffset.Now);

    await WaitForStartupAsync(cancellationToken);

    using IServiceScope scope = _serviceProvider.CreateScope();
    _publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

    try
    {
      // NOTE(fpion): the order of these tasks matter.
      await ExecuteAsync(new SeedRealmTask(), cancellationToken);
      await ExecuteAsync(new SeedRolesTask(), cancellationToken);
      await ExecuteAsync(new SeedUsersTask(), cancellationToken);
      await ExecuteAsync(new MigrateDatabaseTask(), cancellationToken);
      await ExecuteAsync(new SeedAspectsTask(), cancellationToken);
      await ExecuteAsync(new SeedCustomizationsTask(), cancellationToken);
      await ExecuteAsync(new SeedCastesTask(), cancellationToken);
      await ExecuteAsync(new SeedEducationsTask(), cancellationToken);
      await ExecuteAsync(new SeedLanguagesTask(), cancellationToken);
      await ExecuteAsync(new SeedNaturesTask(), cancellationToken);
      await ExecuteAsync(new SeedTalentsTask(), cancellationToken);
      await ExecuteAsync(new SeedSpecializationsTask(), cancellationToken);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, GenericErrorMessage);
      _result = LogLevel.Error;

      Environment.ExitCode = exception.HResult;
    }
    finally
    {
      chrono.Stop();

      long seconds = chrono.ElapsedMilliseconds / 1000;
      string secondText = seconds <= 1 ? "second" : "seconds";
      switch (_result)
      {
        case LogLevel.Error:
          _logger.LogError("Seeding failed after {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
          break;
        case LogLevel.Warning:
          _logger.LogWarning("Seeding completed with warnings in {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
          break;
        default:
          _logger.LogInformation("Seeding succeeded in {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
          break;
      }

      _hostApplicationLifetime.StopApplication();
    }
  }

  private async Task WaitForStartupAsync(CancellationToken cancellationToken)
  {
    using HttpClient client = new();
    if (_portalSettings.BaseUrl != null)
    {
      client.BaseAddress = new Uri(_portalSettings.BaseUrl, UriKind.Absolute);
    }

    for (int attempts = 0; attempts < MaximumAttempts; attempts++)
    {
      try
      {
        using HttpRequestMessage request = new(HttpMethod.Get, new Uri("/", UriKind.Relative));
        _ = await client.SendAsync(request, cancellationToken);
        _logger.LogInformation("Attempt {Attempt} of {Maximum} - Portal is now up and running!", attempts + 1, MaximumAttempts);

        return;
      }
      catch (Exception exception)
      {
        _logger.LogWarning("Attempt {Attempt} of {Maximum} - A '{ExceptionType}' occurred. Retrying in {Milliseconds}ms.",
          attempts + 1, MaximumAttempts, exception.GetType().Name, MillisecondsDelay);

        await Task.Delay(MillisecondsDelay, cancellationToken);
      }
    }

    throw new InvalidOperationException("The Portal could not be reached.");
  }

  private async Task ExecuteAsync(SeedingTask task, CancellationToken cancellationToken)
  {
    await ExecuteAsync(task, continueOnError: false, cancellationToken);
  }
  private async Task ExecuteAsync(SeedingTask task, bool continueOnError, CancellationToken cancellationToken)
  {
    bool hasFailed = false;
    try
    {
      await Publisher.Publish(task, cancellationToken);
    }
    catch (Exception exception)
    {
      if (continueOnError)
      {
        _logger.LogWarning(exception, GenericErrorMessage);
        hasFailed = true;
      }
      else
      {
        throw;
      }
    }
    finally
    {
      task.Complete();

      LogLevel result = LogLevel.Information;
      if (hasFailed)
      {
        _result = LogLevel.Warning;
        result = LogLevel.Warning;
      }

      int milliseconds = task.Duration?.Milliseconds ?? 0;
      int seconds = milliseconds / 1000;
      string secondText = seconds <= 1 ? "second" : "seconds";
      _logger.Log(result, "Task '{Name}' succeeded in {Elapsed}ms ({Seconds} {SecondText}).", task.Name, milliseconds, seconds, secondText);
    }
  }
}
