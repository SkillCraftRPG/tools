using CsvHelper;
using Logitar;
using MediatR;
using SkillCraft.Tools.DataTransform.Worker.Inputs;
using SkillCraft.Tools.DataTransform.Worker.Payloads;

namespace SkillCraft.Tools.DataTransform.Worker.Commands;

internal record ConvertCustomizationsCommand(Encoding Encoding, JsonSerializerOptions SerializerOptions) : IRequest;

internal class ConvertCustomizationsCommandHandler : IRequestHandler<ConvertCustomizationsCommand>
{
  private readonly ILogger<ConvertCustomizationsCommandHandler> _logger;

  public ConvertCustomizationsCommandHandler(ILogger<ConvertCustomizationsCommandHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(ConvertCustomizationsCommand command, CancellationToken cancellationToken)
  {
    // Extract
    using StreamReader reader = new StreamReader("data/customizations.csv", command.Encoding);
    using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
    IAsyncEnumerable<CustomizationInput> inputs = csv.GetRecordsAsync<CustomizationInput>(cancellationToken);

    // Transform
    List<CustomizationPayload> customizations = [];
    await foreach (CustomizationInput input in inputs)
    {
      CustomizationPayload customization = new()
      {
        Id = input.Id,
        Type = input.Type,
        UniqueSlug = input.UniqueSlug.Trim(),
        DisplayName = input.DisplayName?.CleanTrim(),
        Description = input.Description?.CleanTrim()
      };
      customizations.Add(customization);
    }
    _logger.LogInformation("Extracted {Count} customizations from CSV file.", customizations.Count);

    // Load
    string json = JsonSerializer.Serialize(customizations, command.SerializerOptions);
    await File.WriteAllTextAsync("output/customizations.json", json, command.Encoding, cancellationToken);
    _logger.LogInformation("Loaded {Count} customizations to JSON file.", customizations.Count);
  }
}
