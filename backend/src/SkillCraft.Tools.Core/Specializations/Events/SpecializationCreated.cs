using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Specializations.Events;

public record SpecializationCreated(int Tier, Slug UniqueSlug) : DomainEvent, INotification;
