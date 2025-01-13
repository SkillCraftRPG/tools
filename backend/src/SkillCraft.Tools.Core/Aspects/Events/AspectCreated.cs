using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Aspects.Events;

public record AspectCreated(Slug UniqueSlug) : DomainEvent, INotification;
