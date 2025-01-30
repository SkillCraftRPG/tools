using Logitar.Cms.Core;
using Logitar.Cms.Core.Localization;
using Logitar.Cms.Core.Localization.Models;
using Logitar.Cms.Core.Users;
using Logitar.Cms.Core.Users.Models;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Seeding.Cms;
using SkillCraft.Tools.Seeding.Cms.Tasks;
using SkillCraft.Tools.Seeding.Game.Tasks;

namespace SkillCraft.Tools.Seeding;

internal class SeedingWorker : BackgroundService
{
  private const string DefaultUniqueName = "admin";
  private const string GenericErrorMessage = "An unhanded exception occurred.";

  private readonly IApplicationContext _applicationContext;
  private readonly IConfiguration _configuration;
  private readonly IHostApplicationLifetime _hostApplicationLifetime;
  private readonly ILogger<SeedingWorker> _logger;
  private readonly IServiceProvider _serviceProvider;

  private IPublisher? _publisher = null;
  private IPublisher Publisher => _publisher ?? throw new InvalidOperationException($"The {nameof(Publisher)} has not been initialized yet.");

  private LogLevel _result = LogLevel.Information; // NOTE(fpion): "Information" means success.

  public SeedingWorker(
    IApplicationContext applicationContext,
    IConfiguration configuration,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<SeedingWorker> logger,
    IServiceProvider serviceProvider)
  {
    _applicationContext = applicationContext;
    _configuration = configuration;
    _hostApplicationLifetime = hostApplicationLifetime;
    _logger = logger;
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    Stopwatch chrono = Stopwatch.StartNew();
    _logger.LogInformation("Worker executing at {Timestamp}.", DateTimeOffset.Now);

    using IServiceScope scope = _serviceProvider.CreateScope();
    _publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

    try
    {
      IUserQuerier userQuerier = scope.ServiceProvider.GetRequiredService<IUserQuerier>();
      string uniqueName = _configuration.GetValue<string>("CMS_USERNAME") ?? DefaultUniqueName;
      UserModel user = await userQuerier.ReadAsync(uniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The user '{uniqueName}' could not be found.");
      if (_applicationContext is SeedingApplicationContext applicationContext)
      {
        applicationContext.ActorId = new ActorId(user.Id);
      }

      ILanguageQuerier languageQuerier = scope.ServiceProvider.GetRequiredService<ILanguageQuerier>();
      LanguageModel language = await languageQuerier.ReadDefaultAsync(cancellationToken);

      // NOTE(fpion): the order of these tasks matter.
      await ExecuteAsync(new SeedContentTypesTask(), cancellationToken);
      await ExecuteAsync(new SeedFieldTypesTask(), cancellationToken);
      await ExecuteAsync(new SeedFieldDefinitionsTask(), cancellationToken);

      PublicationAction publicationAction = PublicationAction.Publish;
      await ExecuteAsync(new SeedAspectsTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedCustomizationsTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedScriptsTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedLanguagesTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedLineagesTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedFeaturesTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedCastesTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedEducationsTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedNaturesTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedTalentsTask(language, publicationAction), cancellationToken);
      await ExecuteAsync(new SeedSpecializationsTask(language, publicationAction), cancellationToken);
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
