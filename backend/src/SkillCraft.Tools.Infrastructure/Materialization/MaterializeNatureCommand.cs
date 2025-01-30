using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Infrastructure.Entities;
using Attribute = SkillCraft.Tools.Core.Attribute;

namespace SkillCraft.Tools.Infrastructure.Materialization;

public record MaterializeNatureCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeNatureCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeNatureCommandHandler : IRequestHandler<MaterializeNatureCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeNatureCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeNatureCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    NatureEntity? nature = await _context.Natures
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (nature == null)
    {
      nature = new(command.Event);
      _context.Natures.Add(nature);
    }
    else
    {
      nature.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale == null)
    {
      nature.Attribute = command.FieldValues.TryGetValue(Nature.Attribute, out string? attribute) ? Enum.Parse<Attribute>(attribute) : null;

      Guid? giftId = command.FieldValues.TryGetValue(Nature.Gift, out string? giftIdValue) ? Guid.Parse(giftIdValue) : null;
      CustomizationEntity? gift = giftId.HasValue
        ? await _context.Customizations.SingleOrDefaultAsync(x => x.Id == giftId.Value, cancellationToken)
        : null;
      nature.SetGift(gift);
    }
    else
    {
      nature.UniqueSlug = locale.UniqueName.Value;
      nature.DisplayName = locale.DisplayName?.Value;
      nature.Description = locale.Description?.Value;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
