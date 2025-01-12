using MediatR;
using SkillCraft.Tools.Infrastructure.Commands;

namespace SkillCraft.Tools.Worker.Backend.Tasks;

internal class MigrateDatabaseTask : SeedingTask
{
  public override string? Description => "Applies database migrations.";
}

internal class MigrateDatabaseTaskHandler : INotificationHandler<MigrateDatabaseTask>
{
  private readonly IMediator _mediator;

  public MigrateDatabaseTaskHandler(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task Handle(MigrateDatabaseTask _, CancellationToken cancellationToken)
  {
    await _mediator.Send(new MigrateDatabaseCommand(), cancellationToken);
  }
}
