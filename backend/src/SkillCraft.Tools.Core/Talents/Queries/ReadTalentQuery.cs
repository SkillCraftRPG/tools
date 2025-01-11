using MediatR;
using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.Core.Talents.Queries;

public record ReadTalentQuery(Guid? Id, string? Slug) : IRequest<TalentModel?>;

internal class ReadTalentQueryHandler : IRequestHandler<ReadTalentQuery, TalentModel?>
{
  private readonly ITalentQuerier _talentQuerier;

  public ReadTalentQueryHandler(ITalentQuerier talentQuerier)
  {
    _talentQuerier = talentQuerier;
  }

  public async Task<TalentModel?> Handle(ReadTalentQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, TalentModel> talents = new(capacity: 2);

    if (query.Id.HasValue)
    {
      var talent = await _talentQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (talent != null)
      {
        talents[talent.Id] = talent;
      }
    }
    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      var talent = await _talentQuerier.ReadAsync(query.Slug, cancellationToken);
      if (talent != null)
      {
        talents[talent.Id] = talent;
      }
    }

    if (talents.Count > 1)
    {
      throw TooManyResultsException<TalentModel>.ExpectedSingle(talents.Count);
    }

    return talents.Values.SingleOrDefault();
  }
}
