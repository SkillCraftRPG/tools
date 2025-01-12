using Logitar.Data;
using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Infrastructure;

public interface ISqlHelper
{
  void ApplyTextSearch(IQueryBuilder query, TextSearch search, params ColumnId[] columns);

  IQueryBuilder QueryFrom(TableId table);
}
