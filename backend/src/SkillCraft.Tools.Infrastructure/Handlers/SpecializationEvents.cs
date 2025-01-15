using Logitar;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Specializations.Events;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class SpecializationEvents : INotificationHandler<SpecializationCreated>, INotificationHandler<SpecializationUpdated>
{
  private readonly SkillCraftContext _context;

  public SpecializationEvents(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(SpecializationCreated @event, CancellationToken cancellationToken)
  {
    SpecializationEntity? specialization = await _context.Specializations.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (specialization == null)
    {
      specialization = new(@event);

      _context.Specializations.Add(specialization);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(SpecializationUpdated @event, CancellationToken cancellationToken)
  {
    SpecializationEntity? specialization = await _context.Specializations
      .Include(x => x.OptionalTalents)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (specialization != null && specialization.Version == (@event.Version - 1))
    {
      HashSet<Guid> talentIds = new(capacity: 1 + @event.OptionalTalentIds.Count);
      if (@event.RequiredTalentId?.Value != null)
      {
        talentIds.Add(@event.RequiredTalentId.Value.Value.ToGuid());
      }
      talentIds.AddRange(@event.OptionalTalentIds.Keys.Select(id => id.ToGuid()));
      Dictionary<Guid, TalentEntity> talents = await _context.Talents
        .Where(x => talentIds.Contains(x.Id))
        .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

      IEnumerable<Guid> missingTalents = talentIds.Except(talents.Keys).Distinct();
      if (missingTalents.Any())
      {
        StringBuilder message = new();
        message.AppendLine("The specified talent entities could not be found.");
        foreach (Guid id in missingTalents)
        {
          message.Append(" - Id: ").Append(id).AppendLine();
        }
        throw new InvalidOperationException(message.ToString());
      }

      specialization.Update(talents, @event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
