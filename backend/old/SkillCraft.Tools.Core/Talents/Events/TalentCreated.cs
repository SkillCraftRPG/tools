using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Tools.Core.Talents.Events;

public record TalentCreated(int Tier, Slug UniqueSlug) : DomainEvent, INotification;
