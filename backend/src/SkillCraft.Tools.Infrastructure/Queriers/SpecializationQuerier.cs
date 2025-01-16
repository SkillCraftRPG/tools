using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Specializations;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class SpecializationQuerier : ISpecializationQuerier
{
  private readonly IActorService _actorService;
  private readonly ISqlHelper _sqlHelper;
  private readonly DbSet<SpecializationEntity> _specializations;

  public SpecializationQuerier(IActorService actorService, SkillCraftContext context, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _sqlHelper = sqlHelper;
    _specializations = context.Specializations;
  }

  public async Task<SpecializationId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug.Value);

    string? streamId = await _specializations.AsNoTracking()
      .Where(x => x.UniqueSlugNormalized == uniqueSlugNormalized)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);

    return streamId == null ? null : new SpecializationId(streamId);
  }

  public async Task<SpecializationModel> ReadAsync(Specialization specialization, CancellationToken cancellationToken)
  {
    return await ReadAsync(specialization.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The specialization entity 'StreamId={specialization.Id}' could not be found.");
  }
  public async Task<SpecializationModel?> ReadAsync(SpecializationId specializationId, CancellationToken cancellationToken)
  {
    string streamId = specializationId.Value;

    SpecializationEntity? specialization = await _specializations.AsNoTracking()
      .Include(x => x.RequiredTalent)
      .Include(x => x.OptionalTalents).ThenInclude(x => x.RequiredTalent)
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);

    return specialization == null ? null : await MapAsync(specialization, cancellationToken);
  }
  public async Task<SpecializationModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    SpecializationEntity? specialization = await _specializations.AsNoTracking()
      .Include(x => x.RequiredTalent)
      .Include(x => x.OptionalTalents).ThenInclude(x => x.RequiredTalent)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return specialization == null ? null : await MapAsync(specialization, cancellationToken);
  }
  public async Task<SpecializationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    SpecializationEntity? specialization = await _specializations.AsNoTracking()
      .Include(x => x.RequiredTalent)
      .Include(x => x.OptionalTalents).ThenInclude(x => x.RequiredTalent)
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return specialization == null ? null : await MapAsync(specialization, cancellationToken);
  }

  public async Task<SearchResults<SpecializationModel>> SearchAsync(SearchSpecializationsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(Specializations.Table).SelectAll(Specializations.Table)
      .ApplyIdFilter(payload, Specializations.Id);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, Specializations.UniqueSlug, Specializations.DisplayName, Specializations.ReservedTalentName);

    if (payload.TalentId.HasValue)
    {
      // TODO(fpion): TalentId Filter
    }

    IQueryable<SpecializationEntity> query = _specializations.FromQuery(builder).AsNoTracking()
      .Include(x => x.RequiredTalent);

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<SpecializationEntity>? ordered = null;
    foreach (SpecializationSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case SpecializationSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case SpecializationSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case SpecializationSort.UniqueSlug:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueSlug) : query.OrderBy(x => x.UniqueSlug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueSlug) : ordered.ThenBy(x => x.UniqueSlug));
          break;
        case SpecializationSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    SpecializationEntity[] specializations = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<SpecializationModel> items = await MapAsync(specializations, cancellationToken);

    return new SearchResults<SpecializationModel>(items, total);
  }

  private async Task<SpecializationModel> MapAsync(SpecializationEntity specialization, CancellationToken cancellationToken)
  {
    return (await MapAsync([specialization], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<SpecializationModel>> MapAsync(IEnumerable<SpecializationEntity> specializations, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = specializations.SelectMany(specialization => specialization.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return specializations.Select(mapper.ToSpecialization).ToArray();
  }
}
