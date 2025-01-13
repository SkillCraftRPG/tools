using MediatR;
using SkillCraft.Tools.Core.Languages.Models;

namespace SkillCraft.Tools.Core.Languages.Queries;

public record ReadLanguageQuery(Guid? Id, string? Slug) : IRequest<LanguageModel?>;

internal class ReadLanguageQueryHandler : IRequestHandler<ReadLanguageQuery, LanguageModel?>
{
  private readonly ILanguageQuerier _casteQuerier;

  public ReadLanguageQueryHandler(ILanguageQuerier casteQuerier)
  {
    _casteQuerier = casteQuerier;
  }

  public async Task<LanguageModel?> Handle(ReadLanguageQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, LanguageModel> castes = new(capacity: 2);

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
      throw TooManyResultsException<LanguageModel>.ExpectedSingle(castes.Count);
    }

    return castes.Values.SingleOrDefault();
  }
}
