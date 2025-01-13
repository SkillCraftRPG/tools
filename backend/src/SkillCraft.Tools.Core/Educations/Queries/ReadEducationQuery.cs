using MediatR;
using SkillCraft.Tools.Core.Educations.Models;

namespace SkillCraft.Tools.Core.Educations.Queries;

public record ReadEducationQuery(Guid? Id, string? Slug) : IRequest<EducationModel?>;

internal class ReadEducationQueryHandler : IRequestHandler<ReadEducationQuery, EducationModel?>
{
  private readonly IEducationQuerier _educationQuerier;

  public ReadEducationQueryHandler(IEducationQuerier educationQuerier)
  {
    _educationQuerier = educationQuerier;
  }

  public async Task<EducationModel?> Handle(ReadEducationQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, EducationModel> educations = new(capacity: 2);

    if (query.Id.HasValue)
    {
      var education = await _educationQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (education != null)
      {
        educations[education.Id] = education;
      }
    }
    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      var education = await _educationQuerier.ReadAsync(query.Slug, cancellationToken);
      if (education != null)
      {
        educations[education.Id] = education;
      }
    }

    if (educations.Count > 1)
    {
      throw TooManyResultsException<EducationModel>.ExpectedSingle(educations.Count);
    }

    return educations.Values.SingleOrDefault();
  }
}
