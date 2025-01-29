using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Lineages.Events;

public record LineageCreated(LineageId? ParentId, Slug UniqueSlug) : DomainEvent, INotification;
