using Logitar.Data;
using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.Infrastructure;

public interface ISqlHelper
{
  void ApplyTextSearch(IQueryBuilder query, TextSearch search, params ColumnId[] columns);

  IQueryBuilder QueryFrom(TableId table);
}
