using CsvHelper;
using Logitar;
using MediatR;
using SkillCraft.Tools.DataTransform.Worker.Inputs;
using SkillCraft.Tools.DataTransform.Worker.Payloads;

namespace SkillCraft.Tools.DataTransform.Worker.Commands;

internal record ConvertNaturesCommand(Encoding Encoding, JsonSerializerOptions SerializerOptions) : IRequest;

internal class ConvertNaturesCommandHandler : IRequestHandler<ConvertNaturesCommand>
{
  private readonly ILogger<ConvertNaturesCommandHandler> _logger;

  public ConvertNaturesCommandHandler(ILogger<ConvertNaturesCommandHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(ConvertNaturesCommand command, CancellationToken cancellationToken)
  {
    // Extract
    using StreamReader reader = new StreamReader("data/natures.csv", command.Encoding);
    using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
    IAsyncEnumerable<NatureInput> inputs = csv.GetRecordsAsync<NatureInput>(cancellationToken);

    // Transform
    List<NaturePayload> natures = [];
    await foreach (NatureInput input in inputs)
    {
      NaturePayload nature = new()
      {
        Id = input.Id,
        UniqueSlug = input.UniqueSlug.Trim(),
        DisplayName = input.DisplayName?.CleanTrim(),
        Description = input.Description?.CleanTrim(),
        Attribute = input.Attribute,
        GiftId = input.GiftId
      };
      natures.Add(nature);
    }
    _logger.LogInformation("Extracted {Count} natures from CSV file.", natures.Count);

    // Load
    string json = JsonSerializer.Serialize(natures, command.SerializerOptions);
    await File.WriteAllTextAsync("output/natures.json", json, command.Encoding, cancellationToken);
    _logger.LogInformation("Loaded {Count} natures to JSON file.", natures.Count);
  }
}
