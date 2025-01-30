using Logitar.Cms.Core.Actors;
using Logitar.Cms.Core.Search;
using Logitar.Cms.Infrastructure;
using Logitar.Cms.Infrastructure.Actors;
using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Specializations;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class SpecializationQuerier : ISpecializationQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<SpecializationEntity> _specializations;
  private readonly IQueryHelper _queryHelper;

  public SpecializationQuerier(IActorService actorService, SkillCraftContext context, IQueryHelper queryHelper)
  {
    _actorService = actorService;
    _specializations = context.Specializations;
    _queryHelper = queryHelper;
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
    IQueryBuilder builder = _queryHelper.From(Specializations.Table).SelectAll(Specializations.Table)
      .ApplyIdFilter(Specializations.Id, payload.Ids);
    _queryHelper.ApplyTextSearch(builder, payload.Search, Specializations.UniqueSlug, Specializations.DisplayName, Specializations.ReservedTalentName);

    if (payload.TalentId.HasValue)
    {
      TableId requiredTalent = new(Talents.Table.Schema, Talents.Table.Table ?? string.Empty, "RequiredTalent");
      ColumnId requiredTalentId = new(nameof(TalentEntity.TalentId), requiredTalent);
      ColumnId requiredTalentUid = new(nameof(TalentEntity.Id), requiredTalent);

      TableId optionalTalent = new(Talents.Table.Schema, Talents.Table.Table ?? string.Empty, "OptionalTalent");
      ColumnId optionalTalentId = new(nameof(TalentEntity.TalentId), optionalTalent);
      ColumnId optionalTalentUid = new(nameof(TalentEntity.Id), optionalTalent);

      builder.LeftJoin(requiredTalentId, Specializations.RequiredTalentId)
        .LeftJoin(SpecializationOptionalTalents.SpecializationId, Specializations.SpecializationId)
        .LeftJoin(optionalTalentId, SpecializationOptionalTalents.TalentId)
        .WhereOr(
          new OperatorCondition(requiredTalentUid, Operators.IsEqualTo(payload.TalentId.Value)),
          new OperatorCondition(optionalTalentUid, Operators.IsEqualTo(payload.TalentId.Value))
        );
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
