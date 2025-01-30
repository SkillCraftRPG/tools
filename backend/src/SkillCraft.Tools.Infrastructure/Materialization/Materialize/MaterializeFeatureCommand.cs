using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Materialize;

public record MaterializeFeatureCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeFeatureCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeFeatureCommandHandler : IRequestHandler<MaterializeFeatureCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeFeatureCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeFeatureCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    FeatureEntity? feature = await _context.Features
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (feature == null)
    {
      feature = new(command.Event);
      _context.Features.Add(feature);
    }
    else
    {
      feature.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale != null)
    {
      feature.UniqueSlug = locale.UniqueName.Value;
      feature.DisplayName = locale.DisplayName?.Value;
      feature.Description = locale.Description?.Value;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
