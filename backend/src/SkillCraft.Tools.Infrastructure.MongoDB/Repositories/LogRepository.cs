using MongoDB.Driver;
using SkillCraft.Tools.Core.Logging;
using SkillCraft.Tools.Infrastructure.MongoDB.Entities;

namespace SkillCraft.Tools.Infrastructure.MongoDB.Repositories;

internal class LogRepository : ILogRepository
{
  private readonly IMongoCollection<LogEntity> _logs;
  private readonly JsonSerializerOptions _serializerOptions = new();

  public LogRepository(IMongoDatabase database)
  {
    _logs = database.GetCollection<LogEntity>("logs");
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  public async Task SaveAsync(Log log, CancellationToken cancellationToken)
  {
    LogEntity entity = new(log, _serializerOptions);

    await _logs.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);
  }
}
