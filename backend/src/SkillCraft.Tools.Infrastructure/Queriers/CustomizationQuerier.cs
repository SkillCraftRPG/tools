using Logitar.Data;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Actors.Models;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.Core.Search;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class CustomizationQuerier : ICustomizationQuerier
{
  private readonly IActorService _actorService;
  private readonly ISqlHelper _sqlHelper;
  private readonly DbSet<CustomizationEntity> _customizations;

  public CustomizationQuerier(IActorService actorService, SkillCraftContext context, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _sqlHelper = sqlHelper;
    _customizations = context.Customizations;
  }

  public async Task<CustomizationId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug.Value);

    string? streamId = await _customizations.AsNoTracking()
      .Where(x => x.UniqueSlugNormalized == uniqueSlugNormalized)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);

    return streamId == null ? null : new CustomizationId(streamId);
  }

  public async Task<CustomizationModel> ReadAsync(Customization customization, CancellationToken cancellationToken)
  {
    return await ReadAsync(customization.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The customization entity 'StreamId={customization.Id}' could not be found.");
  }
  public async Task<CustomizationModel?> ReadAsync(CustomizationId customizationId, CancellationToken cancellationToken)
  {
    string streamId = customizationId.Value;

    CustomizationEntity? customization = await _customizations.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);

    return customization == null ? null : await MapAsync(customization, cancellationToken);
  }
  public async Task<CustomizationModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    CustomizationEntity? customization = await _customizations.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return customization == null ? null : await MapAsync(customization, cancellationToken);
  }
  public async Task<CustomizationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    CustomizationEntity? customization = await _customizations.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return customization == null ? null : await MapAsync(customization, cancellationToken);
  }

  public async Task<SearchResults<CustomizationModel>> SearchAsync(SearchCustomizationsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(Customizations.Table).SelectAll(Customizations.Table)
      .ApplyIdFilter(payload, Customizations.Id);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, Customizations.UniqueSlug, Customizations.DisplayName);

    if (payload.Type.HasValue)
    {
      builder.Where(Customizations.Type, Operators.IsEqualTo(payload.Type.Value));
    }

    IQueryable<CustomizationEntity> query = _customizations.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<CustomizationEntity>? ordered = null;
    foreach (CustomizationSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case CustomizationSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case CustomizationSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case CustomizationSort.UniqueSlug:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueSlug) : query.OrderBy(x => x.UniqueSlug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueSlug) : ordered.ThenBy(x => x.UniqueSlug));
          break;
        case CustomizationSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    CustomizationEntity[] customizations = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<CustomizationModel> items = await MapAsync(customizations, cancellationToken);

    return new SearchResults<CustomizationModel>(items, total);
  }

  private async Task<CustomizationModel> MapAsync(CustomizationEntity customization, CancellationToken cancellationToken)
  {
    return (await MapAsync([customization], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<CustomizationModel>> MapAsync(IEnumerable<CustomizationEntity> customizations, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = customizations.SelectMany(customization => customization.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return customizations.Select(mapper.ToCustomization).ToArray();
  }
}
