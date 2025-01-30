using Logitar.Cms.Core;
using MediatR;
using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.Core.Specializations.Queries;

public record ReadSpecializationQuery(Guid? Id, string? Slug) : Activity, IRequest<SpecializationModel?>;

internal class ReadSpecializationQueryHandler : IRequestHandler<ReadSpecializationQuery, SpecializationModel?>
{
  private readonly ISpecializationQuerier _specializationQuerier;

  public ReadSpecializationQueryHandler(ISpecializationQuerier specializationQuerier)
  {
    _specializationQuerier = specializationQuerier;
  }

  public async Task<SpecializationModel?> Handle(ReadSpecializationQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, SpecializationModel> specializations = new(capacity: 2);

    if (query.Id.HasValue)
    {
      var specialization = await _specializationQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (specialization != null)
      {
        specializations[specialization.Id] = specialization;
      }
    }
    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      var specialization = await _specializationQuerier.ReadAsync(query.Slug, cancellationToken);
      if (specialization != null)
      {
        specializations[specialization.Id] = specialization;
      }
    }

    if (specializations.Count > 1)
    {
      throw TooManyResultsException<SpecializationModel>.ExpectedSingle(specializations.Count);
    }

    return specializations.Values.SingleOrDefault();
  }
}
