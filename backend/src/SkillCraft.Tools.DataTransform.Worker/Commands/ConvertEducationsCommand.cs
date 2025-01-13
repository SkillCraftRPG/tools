using CsvHelper;
using Logitar;
using MediatR;
using SkillCraft.Tools.DataTransform.Worker.Inputs;
using SkillCraft.Tools.DataTransform.Worker.Payloads;

namespace SkillCraft.Tools.DataTransform.Worker.Commands;

internal record ConvertEducationsCommand(Encoding Encoding, JsonSerializerOptions SerializerOptions) : IRequest;

internal class ConvertEducationsCommandHandler : IRequestHandler<ConvertEducationsCommand>
{
  private readonly ILogger<ConvertEducationsCommandHandler> _logger;

  public ConvertEducationsCommandHandler(ILogger<ConvertEducationsCommandHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(ConvertEducationsCommand command, CancellationToken cancellationToken)
  {
    // Extract
    using StreamReader reader = new StreamReader("data/educations.csv", command.Encoding);
    using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
    IAsyncEnumerable<EducationInput> inputs = csv.GetRecordsAsync<EducationInput>(cancellationToken);

    // Transform
    List<EducationPayload> educations = [];
    await foreach (EducationInput input in inputs)
    {
      EducationPayload education = new()
      {
        Id = input.Id,
        UniqueSlug = input.UniqueSlug.Trim(),
        DisplayName = input.DisplayName?.CleanTrim(),
        Description = input.Description?.CleanTrim(),
        Skill = input.Skill,
        WealthMultiplier = input.WealthMultiplier
      };
      educations.Add(education);
    }
    _logger.LogInformation("Extracted {Count} educations from CSV file.", educations.Count);

    // Load
    string json = JsonSerializer.Serialize(educations, command.SerializerOptions);
    await File.WriteAllTextAsync("output/educations.json", json, command.Encoding, cancellationToken);
    _logger.LogInformation("Loaded {Count} educations to JSON file.", educations.Count);
  }
}
