using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Customizations.Events;

public record CustomizationCreated(CustomizationType Type, Slug UniqueSlug) : DomainEvent, INotification;
