using CsvHelper;
using Logitar;
using MediatR;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.DataTransform.Worker.Inputs;
using SkillCraft.Tools.DataTransform.Worker.Payloads;

namespace SkillCraft.Tools.DataTransform.Worker.Commands;

internal record ConvertCastesCommand(Encoding Encoding, JsonSerializerOptions SerializerOptions) : IRequest;

internal class ConvertCastesCommandHandler : IRequestHandler<ConvertCastesCommand>
{
  private readonly ILogger<ConvertCastesCommandHandler> _logger;

  public ConvertCastesCommandHandler(ILogger<ConvertCastesCommandHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(ConvertCastesCommand command, CancellationToken cancellationToken)
  {
    // Extract
    using StreamReader reader = new StreamReader("data/castes.csv", command.Encoding);
    using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
    IAsyncEnumerable<CasteInput> inputs = csv.GetRecordsAsync<CasteInput>(cancellationToken);

    // Transform
    List<CastePayload> castes = [];
    await foreach (CasteInput input in inputs)
    {
      CastePayload caste = new()
      {
        Id = input.Id,
        UniqueSlug = input.UniqueSlug.Trim(),
        DisplayName = input.DisplayName?.CleanTrim(),
        Description = input.Description?.CleanTrim(),
        Skill = input.Skill,
        WealthRoll = input.WealthRoll?.CleanTrim()
      };
      if (!string.IsNullOrWhiteSpace(input.Feature1Name))
      {
        caste.Features.Add(new FeaturePayload
        {
          Id = input.Feature1Id,
          Name = input.Feature1Name.Trim(),
          Description = input.Feature1Description?.CleanTrim()
        });
      }
      if (!string.IsNullOrWhiteSpace(input.Feature2Name))
      {
        caste.Features.Add(new FeaturePayload
        {
          Id = input.Feature2Id,
          Name = input.Feature2Name.Trim(),
          Description = input.Feature2Description?.CleanTrim()
        });
      }
      castes.Add(caste);
    }
    _logger.LogInformation("Extracted {Count} castes from CSV file.", castes.Count);

    // Load
    string json = JsonSerializer.Serialize(castes, command.SerializerOptions);
    await File.WriteAllTextAsync("output/castes.json", json, command.Encoding, cancellationToken);
    _logger.LogInformation("Loaded {Count} castes to JSON file.", castes.Count);
  }
}
