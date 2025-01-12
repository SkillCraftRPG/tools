using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace SkillCraft.Tools.Infrastructure.Commands;

public record MigrateDatabaseCommand : IRequest;

internal class MigrateDatabaseCommandHandler : IRequestHandler<MigrateDatabaseCommand>
{
  private readonly EventContext _eventContext;
  private readonly SkillCraftContext _skillCraftContext;

  public MigrateDatabaseCommandHandler(EventContext eventContext, SkillCraftContext skillCraftContext)
  {
    _eventContext = eventContext;
    _skillCraftContext = skillCraftContext;
  }

  public async Task Handle(MigrateDatabaseCommand _, CancellationToken cancellationToken)
  {
    await _eventContext.Database.MigrateAsync(cancellationToken);
    await _skillCraftContext.Database.MigrateAsync(cancellationToken);
  }
}
