using Logitar.Cms.Core;
using MediatR;
using SkillCraft.Tools.Core.Customizations.Models;

namespace SkillCraft.Tools.Core.Customizations.Queries;

public record ReadCustomizationQuery(Guid? Id, string? Slug) : Activity, IRequest<CustomizationModel?>;

internal class ReadCustomizationQueryHandler : IRequestHandler<ReadCustomizationQuery, CustomizationModel?>
{
  private readonly ICustomizationQuerier _customizationQuerier;

  public ReadCustomizationQueryHandler(ICustomizationQuerier customizationQuerier)
  {
    _customizationQuerier = customizationQuerier;
  }

  public async Task<CustomizationModel?> Handle(ReadCustomizationQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, CustomizationModel> customizations = new(capacity: 2);

    if (query.Id.HasValue)
    {
      var customization = await _customizationQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (customization != null)
      {
        customizations[customization.Id] = customization;
      }
    }
    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      var customization = await _customizationQuerier.ReadAsync(query.Slug, cancellationToken);
      if (customization != null)
      {
        customizations[customization.Id] = customization;
      }
    }

    if (customizations.Count > 1)
    {
      throw TooManyResultsException<CustomizationModel>.ExpectedSingle(customizations.Count);
    }

    return customizations.Values.SingleOrDefault();
  }
}
