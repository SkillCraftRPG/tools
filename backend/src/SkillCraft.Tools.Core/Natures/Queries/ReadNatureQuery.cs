using Logitar.Cms.Core;
using MediatR;
using SkillCraft.Tools.Core.Natures.Models;

namespace SkillCraft.Tools.Core.Natures.Queries;

public record ReadNatureQuery(Guid? Id, string? Slug) : Activity, IRequest<NatureModel?>;

internal class ReadNatureQueryHandler : IRequestHandler<ReadNatureQuery, NatureModel?>
{
  private readonly INatureQuerier _natureQuerier;

  public ReadNatureQueryHandler(INatureQuerier natureQuerier)
  {
    _natureQuerier = natureQuerier;
  }

  public async Task<NatureModel?> Handle(ReadNatureQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, NatureModel> natures = new(capacity: 2);

    if (query.Id.HasValue)
    {
      var nature = await _natureQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (nature != null)
      {
        natures[nature.Id] = nature;
      }
    }
    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      var nature = await _natureQuerier.ReadAsync(query.Slug, cancellationToken);
      if (nature != null)
      {
        natures[nature.Id] = nature;
      }
    }

    if (natures.Count > 1)
    {
      throw TooManyResultsException<NatureModel>.ExpectedSingle(natures.Count);
    }

    return natures.Values.SingleOrDefault();
  }
}
