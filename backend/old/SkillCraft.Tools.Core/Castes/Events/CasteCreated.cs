using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Castes.Events;

public record CasteCreated(Slug UniqueSlug) : DomainEvent, INotification;
