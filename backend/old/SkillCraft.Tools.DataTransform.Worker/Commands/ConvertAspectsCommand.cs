using CsvHelper;
using Logitar;
using MediatR;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.DataTransform.Worker.Inputs;
using SkillCraft.Tools.DataTransform.Worker.Payloads;

namespace SkillCraft.Tools.DataTransform.Worker.Commands;

internal record ConvertAspectsCommand(Encoding Encoding, JsonSerializerOptions SerializerOptions) : IRequest;

internal class ConvertAspectsCommandHandler : IRequestHandler<ConvertAspectsCommand>
{
  private readonly ILogger<ConvertAspectsCommandHandler> _logger;

  public ConvertAspectsCommandHandler(ILogger<ConvertAspectsCommandHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(ConvertAspectsCommand command, CancellationToken cancellationToken)
  {
    // Extract
    using StreamReader reader = new StreamReader("data/aspects.csv", command.Encoding);
    using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
    IAsyncEnumerable<AspectInput> inputs = csv.GetRecordsAsync<AspectInput>(cancellationToken);

    // Transform
    List<AspectPayload> aspects = [];
    await foreach (AspectInput input in inputs)
    {
      AspectPayload aspect = new()
      {
        Id = input.Id,
        UniqueSlug = input.UniqueSlug.Trim(),
        DisplayName = input.DisplayName?.CleanTrim(),
        Description = input.Description?.CleanTrim(),
        Attributes = new AttributeSelectionModel
        {
          Mandatory1 = input.MandatoryAttribute1,
          Mandatory2 = input.MandatoryAttribute2,
          Optional1 = input.OptionalAttribute1,
          Optional2 = input.OptionalAttribute2
        },
        Skills = new SkillSelectionModel
        {
          Discounted1 = input.DiscountedSkill1,
          Discounted2 = input.DiscountedSkill2
        }
      };
      aspects.Add(aspect);
    }
    _logger.LogInformation("Extracted {Count} aspects from CSV file.", aspects.Count);

    // Load
    string json = JsonSerializer.Serialize(aspects, command.SerializerOptions);
    await File.WriteAllTextAsync("output/aspects.json", json, command.Encoding, cancellationToken);
    _logger.LogInformation("Loaded {Count} aspects to JSON file.", aspects.Count);
  }
}
