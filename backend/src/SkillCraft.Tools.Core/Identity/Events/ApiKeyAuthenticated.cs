using Logitar.Portal.Contracts.ApiKeys;
using MediatR;

namespace SkillCraft.Tools.Core.Identity.Events;

public record ApiKeyAuthenticated(ApiKeyModel ApiKey) : INotification;
