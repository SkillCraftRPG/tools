using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Natures.Events;

public record NatureCreated(Slug UniqueSlug) : DomainEvent, INotification;
