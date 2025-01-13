using CsvHelper;
using Logitar;
using MediatR;
using SkillCraft.Tools.DataTransform.Worker.Inputs;
using SkillCraft.Tools.DataTransform.Worker.Payloads;

namespace SkillCraft.Tools.DataTransform.Worker.Commands;

internal record ConvertLanguagesCommand(Encoding Encoding, JsonSerializerOptions SerializerOptions) : IRequest;

internal class ConvertLanguagesCommandHandler : IRequestHandler<ConvertLanguagesCommand>
{
  private readonly ILogger<ConvertLanguagesCommandHandler> _logger;

  public ConvertLanguagesCommandHandler(ILogger<ConvertLanguagesCommandHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(ConvertLanguagesCommand command, CancellationToken cancellationToken)
  {
    // Extract
    using StreamReader reader = new StreamReader("data/languages.csv", command.Encoding);
    using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
    IAsyncEnumerable<LanguageInput> inputs = csv.GetRecordsAsync<LanguageInput>(cancellationToken);

    // Transform
    List<LanguagePayload> languages = [];
    await foreach (LanguageInput input in inputs)
    {
      LanguagePayload language = new()
      {
        Id = input.Id,
        UniqueSlug = input.UniqueSlug.Trim(),
        DisplayName = input.DisplayName?.CleanTrim(),
        Description = input.Description?.CleanTrim(),
        Script = input.Script?.CleanTrim(),
        TypicalSpeakers = input.TypicalSpeakers?.CleanTrim()
      };
      languages.Add(language);
    }
    _logger.LogInformation("Extracted {Count} languages from CSV file.", languages.Count);

    // Load
    string json = JsonSerializer.Serialize(languages, command.SerializerOptions);
    await File.WriteAllTextAsync("output/languages.json", json, command.Encoding, cancellationToken);
    _logger.LogInformation("Loaded {Count} languages to JSON file.", languages.Count);
  }
}
