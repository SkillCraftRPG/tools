using Logitar.Cms.Core;
using MediatR;
using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.Core.Castes.Queries;

public record ReadCasteQuery(Guid? Id, string? Slug) : Activity, IRequest<CasteModel?>;

internal class ReadCasteQueryHandler : IRequestHandler<ReadCasteQuery, CasteModel?>
{
  private readonly ICasteQuerier _casteQuerier;

  public ReadCasteQueryHandler(ICasteQuerier casteQuerier)
  {
    _casteQuerier = casteQuerier;
  }

  public async Task<CasteModel?> Handle(ReadCasteQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, CasteModel> castes = new(capacity: 2);

    if (query.Id.HasValue)
    {
      var caste = await _casteQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (caste != null)
      {
        castes[caste.Id] = caste;
      }
    }
    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      var caste = await _casteQuerier.ReadAsync(query.Slug, cancellationToken);
      if (caste != null)
      {
        castes[caste.Id] = caste;
      }
    }

    if (castes.Count > 1)
    {
      throw TooManyResultsException<CasteModel>.ExpectedSingle(castes.Count);
    }

    return castes.Values.SingleOrDefault();
  }
}
