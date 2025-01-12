using MediatR;
using Microsoft.EntityFrameworkCore;

namespace SkillCraft.Tools.Infrastructure.Commands;

public record MigrateDatabaseCommand : IRequest;

internal class MigrateDatabaseCommandHandler : IRequestHandler<MigrateDatabaseCommand>
{
  private readonly SkillCraftContext _context;

  public MigrateDatabaseCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MigrateDatabaseCommand _, CancellationToken cancellationToken)
  {
    await _context.Database.MigrateAsync(cancellationToken);
  }
}
