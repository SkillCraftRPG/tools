using CsvHelper;
using Logitar;
using MediatR;
using SkillCraft.Tools.DataTransform.Worker.Inputs;
using SkillCraft.Tools.DataTransform.Worker.Payloads;

namespace SkillCraft.Tools.DataTransform.Worker.Commands;

internal record ConvertTalentsCommand(Encoding Encoding, JsonSerializerOptions SerializerOptions) : IRequest;

internal class ConvertTalentsCommandHandler : IRequestHandler<ConvertTalentsCommand>
{
  private readonly ILogger<ConvertTalentsCommandHandler> _logger;

  public ConvertTalentsCommandHandler(ILogger<ConvertTalentsCommandHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(ConvertTalentsCommand command, CancellationToken cancellationToken)
  {
    // Extract
    using StreamReader reader = new StreamReader("data/talents.csv", command.Encoding);
    using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
    IAsyncEnumerable<TalentInput> inputs = csv.GetRecordsAsync<TalentInput>(cancellationToken);

    // Transform
    List<TalentPayload> talents = [];
    await foreach (TalentInput input in inputs)
    {
      TalentPayload talent = new()
      {
        Id = input.Id,
        UniqueSlug = input.UniqueSlug.Trim(),
        DisplayName = input.DisplayName?.CleanTrim(),
        Description = input.Description?.CleanTrim(),
        AllowMultiplePurchases = input.AllowMultiplePurchases,
        RequiredTalentId = input.RequiredTalentId,
        Skill = input.Skill
      };
      talents.Add(talent);
    }
    _logger.LogInformation("Extracted {Count} talents from CSV file.", talents.Count);

    // Load
    string json = JsonSerializer.Serialize(talents, command.SerializerOptions);
    await File.WriteAllTextAsync("output/talents.json", json, command.Encoding, cancellationToken);
    _logger.LogInformation("Loaded {Count} talents to JSON file.", talents.Count);
  }
}
