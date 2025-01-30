using Logitar.Cms.Core.Actors;
using Logitar.Cms.Core.Search;
using Logitar.Cms.Infrastructure;
using Logitar.Cms.Infrastructure.Actors;
using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Educations;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class EducationQuerier : IEducationQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<EducationEntity> _educations;
  private readonly IQueryHelper _queryHelper;

  public EducationQuerier(IActorService actorService, SkillCraftContext context, IQueryHelper queryHelper)
  {
    _actorService = actorService;
    _educations = context.Educations;
    _queryHelper = queryHelper;
  }

  public async Task<EducationModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    EducationEntity? education = await _educations.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return education == null ? null : await MapAsync(education, cancellationToken);
  }
  public async Task<EducationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    EducationEntity? education = await _educations.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return education == null ? null : await MapAsync(education, cancellationToken);
  }

  public async Task<SearchResults<EducationModel>> SearchAsync(SearchEducationsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _queryHelper.From(Educations.Table).SelectAll(Educations.Table)
      .ApplyIdFilter(Educations.Id, payload.Ids);
    _queryHelper.ApplyTextSearch(builder, payload.Search, Educations.UniqueSlug, Educations.DisplayName);

    if (payload.Skill.HasValue)
    {
      builder.Where(Educations.Skill, Operators.IsEqualTo(payload.Skill.Value.ToString()));
    }

    IQueryable<EducationEntity> query = _educations.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<EducationEntity>? ordered = null;
    foreach (EducationSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case EducationSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case EducationSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case EducationSort.UniqueSlug:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueSlug) : query.OrderBy(x => x.UniqueSlug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueSlug) : ordered.ThenBy(x => x.UniqueSlug));
          break;
        case EducationSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    EducationEntity[] educations = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<EducationModel> items = await MapAsync(educations, cancellationToken);

    return new SearchResults<EducationModel>(items, total);
  }

  private async Task<EducationModel> MapAsync(EducationEntity education, CancellationToken cancellationToken)
  {
    return (await MapAsync([education], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<EducationModel>> MapAsync(IEnumerable<EducationEntity> educations, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = educations.SelectMany(education => education.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return educations.Select(mapper.ToEducation).ToArray();
  }
}
