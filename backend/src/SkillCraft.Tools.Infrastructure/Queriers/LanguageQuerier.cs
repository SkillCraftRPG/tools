using Logitar.Cms.Core.Actors;
using Logitar.Cms.Core.Search;
using Logitar.Cms.Infrastructure;
using Logitar.Cms.Infrastructure.Actors;
using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Languages;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class LanguageQuerier : ILanguageQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<LanguageEntity> _languages;
  private readonly IQueryHelper _queryHelper;

  public LanguageQuerier(IActorService actorService, SkillCraftContext context, IQueryHelper queryHelper)
  {
    _actorService = actorService;
    _languages = context.Languages;
    _queryHelper = queryHelper;
  }

  public async Task<LanguageModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    LanguageEntity? language = await _languages.AsNoTracking()
      .Include(x => x.Scripts)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return language == null ? null : await MapAsync(language, cancellationToken);
  }
  public async Task<LanguageModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    LanguageEntity? language = await _languages.AsNoTracking()
      .Include(x => x.Scripts)
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return language == null ? null : await MapAsync(language, cancellationToken);
  }

  public async Task<SearchResults<LanguageModel>> SearchAsync(SearchLanguagesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _queryHelper.From(Languages.Table).SelectAll(Languages.Table)
      .ApplyIdFilter(Languages.Id, payload.Ids);
    _queryHelper.ApplyTextSearch(builder, payload.Search, Languages.UniqueSlug, Languages.DisplayName);

    if (payload.ScriptId.HasValue)
    {
      builder.Join(LanguageScripts.LanguageId, Languages.LanguageId)
        .Join(Scripts.ScriptId, LanguageScripts.ScriptId)
        .Where(Scripts.Id, Operators.IsEqualTo(payload.ScriptId.Value));
    }

    IQueryable<LanguageEntity> query = _languages.FromQuery(builder).AsNoTracking()
      .Include(x => x.Scripts);

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<LanguageEntity>? ordered = null;
    foreach (LanguageSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case LanguageSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case LanguageSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case LanguageSort.UniqueSlug:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueSlug) : query.OrderBy(x => x.UniqueSlug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueSlug) : ordered.ThenBy(x => x.UniqueSlug));
          break;
        case LanguageSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    LanguageEntity[] languages = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<LanguageModel> items = await MapAsync(languages, cancellationToken);

    return new SearchResults<LanguageModel>(items, total);
  }

  private async Task<LanguageModel> MapAsync(LanguageEntity language, CancellationToken cancellationToken)
  {
    return (await MapAsync([language], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<LanguageModel>> MapAsync(IEnumerable<LanguageEntity> languages, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = languages.SelectMany(language => language.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return languages.Select(mapper.ToLanguage).ToArray();
  }
}
