using Logitar.Portal.Contracts.Users;
using MediatR;

namespace SkillCraft.Tools.Core.Identity.Events;

public record UserAuthenticated(UserModel User) : INotification;
