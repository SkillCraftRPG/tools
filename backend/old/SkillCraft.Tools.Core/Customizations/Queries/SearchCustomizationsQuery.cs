using Logitar.Portal.Contracts.Search;
using MediatR;
using SkillCraft.Tools.Core.Customizations.Models;

namespace SkillCraft.Tools.Core.Customizations.Queries;

public record SearchCustomizationsQuery(SearchCustomizationsPayload Payload) : Activity, IRequest<SearchResults<CustomizationModel>>;

internal class SearchCustomizationsQueryHandler : IRequestHandler<SearchCustomizationsQuery, SearchResults<CustomizationModel>>
{
  private readonly ICustomizationQuerier _customizationQuerier;

  public SearchCustomizationsQueryHandler(ICustomizationQuerier customizationQuerier)
  {
    _customizationQuerier = customizationQuerier;
  }

  public async Task<SearchResults<CustomizationModel>> Handle(SearchCustomizationsQuery query, CancellationToken cancellationToken)
  {
    return await _customizationQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
