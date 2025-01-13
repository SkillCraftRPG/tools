using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Educations.Events;

public record EducationCreated(Slug UniqueSlug) : DomainEvent, INotification;
