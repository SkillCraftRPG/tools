using Logitar.EventSourcing;
using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;

namespace SkillCraft.Tools.Core.Logging;

public interface ILoggingService
{
  void Open(string? correlationId = null, string? method = null, string? destination = null, string? source = null, string? additionalInformation = null, DateTime? startedOn = null);
  void Report(IIdentifiableEvent @event);
  void Report(Exception exception);
  void SetActivity(IActivity activity);
  void SetOperation(Operation operation);
  void SetApiKey(ApiKeyModel? apiKey);
  void SetSession(SessionModel? session);
  void SetUser(UserModel? user);
  Task CloseAndSaveAsync(int statusCode, CancellationToken cancellationToken = default);
}
