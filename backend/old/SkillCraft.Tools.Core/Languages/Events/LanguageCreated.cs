using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Languages.Events;

public record LanguageCreated(Slug UniqueSlug) : DomainEvent, INotification;
