using MediatR;
using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.Core.Aspects.Queries;

public record ReadAspectQuery(Guid? Id, string? Slug) : IRequest<AspectModel?>;

internal class ReadAspectQueryHandler : IRequestHandler<ReadAspectQuery, AspectModel?>
{
  private readonly IAspectQuerier _aspectQuerier;

  public ReadAspectQueryHandler(IAspectQuerier aspectQuerier)
  {
    _aspectQuerier = aspectQuerier;
  }

  public async Task<AspectModel?> Handle(ReadAspectQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, AspectModel> aspects = new(capacity: 2);

    if (query.Id.HasValue)
    {
      var aspect = await _aspectQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (aspect != null)
      {
        aspects[aspect.Id] = aspect;
      }
    }
    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      var aspect = await _aspectQuerier.ReadAsync(query.Slug, cancellationToken);
      if (aspect != null)
      {
        aspects[aspect.Id] = aspect;
      }
    }

    if (aspects.Count > 1)
    {
      throw TooManyResultsException<AspectModel>.ExpectedSingle(aspects.Count);
    }

    return aspects.Values.SingleOrDefault();
  }
}
