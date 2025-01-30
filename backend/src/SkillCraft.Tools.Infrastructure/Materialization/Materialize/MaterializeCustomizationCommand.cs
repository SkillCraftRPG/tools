using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Materialize;

public record MaterializeCustomizationCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeCustomizationCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeCustomizationCommandHandler : IRequestHandler<MaterializeCustomizationCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeCustomizationCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeCustomizationCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    CustomizationEntity? customization = await _context.Customizations
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (customization == null)
    {
      customization = new(command.Event);
      _context.Customizations.Add(customization);
    }
    else
    {
      customization.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale == null)
    {
      if (command.FieldValues.TryGetValue(Core.Contents.Customization.Type, out string? type))
      {
        customization.Type = Enum.Parse<Core.Customizations.CustomizationType>(type);
      }
    }
    else
    {
      customization.UniqueSlug = locale.UniqueName.Value;
      customization.DisplayName = locale.DisplayName?.Value;
      customization.Description = locale.Description?.Value;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
