using Logitar.Cms.Core;
using MediatR;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Core.Lineages.Queries;

public record ReadLineageQuery(Guid? Id, string? Slug) : Activity, IRequest<LineageModel?>;

internal class ReadLineageQueryHandler : IRequestHandler<ReadLineageQuery, LineageModel?>
{
  private readonly ILineageQuerier _lineageQuerier;

  public ReadLineageQueryHandler(ILineageQuerier lineageQuerier)
  {
    _lineageQuerier = lineageQuerier;
  }

  public async Task<LineageModel?> Handle(ReadLineageQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, LineageModel> lineages = new(capacity: 2);

    if (query.Id.HasValue)
    {
      var lineage = await _lineageQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (lineage != null)
      {
        lineages[lineage.Id] = lineage;
      }
    }
    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      var lineage = await _lineageQuerier.ReadAsync(query.Slug, cancellationToken);
      if (lineage != null)
      {
        lineages[lineage.Id] = lineage;
      }
    }

    if (lineages.Count > 1)
    {
      throw TooManyResultsException<LineageModel>.ExpectedSingle(lineages.Count);
    }

    return lineages.Values.SingleOrDefault();
  }
}
