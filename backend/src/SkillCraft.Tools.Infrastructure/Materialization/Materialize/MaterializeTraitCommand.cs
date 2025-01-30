using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Materialize;

public record MaterializeTraitCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeTraitCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeTraitCommandHandler : IRequestHandler<MaterializeTraitCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeTraitCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeTraitCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    TraitEntity? trait = await _context.Traits
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (trait == null)
    {
      trait = new(command.Event);
      _context.Traits.Add(trait);
    }
    else
    {
      trait.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale != null)
    {
      trait.UniqueSlug = locale.UniqueName.Value;
      trait.DisplayName = locale.DisplayName?.Value;
      trait.Description = locale.Description?.Value;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
