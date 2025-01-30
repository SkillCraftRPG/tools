using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Infrastructure.Entities;
using Attribute = SkillCraft.Tools.Core.Attribute;

namespace SkillCraft.Tools.Infrastructure.Materialization.Materialize;

public record MaterializeAspectCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeAspectCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeAspectCommandHandler : IRequestHandler<MaterializeAspectCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeAspectCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeAspectCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    AspectEntity? aspect = await _context.Aspects
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (aspect == null)
    {
      aspect = new(command.Event);
      _context.Aspects.Add(aspect);
    }
    else
    {
      aspect.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale == null)
    {
      aspect.MandatoryAttribute1 = command.FieldValues.TryGetValue(Aspect.MandatoryAttribute1, out string? mandatoryAttribute1) ? Enum.Parse<Attribute>(mandatoryAttribute1) : null;
      aspect.MandatoryAttribute2 = command.FieldValues.TryGetValue(Aspect.MandatoryAttribute1, out string? mandatoryAttribute2) ? Enum.Parse<Attribute>(mandatoryAttribute2) : null;
      aspect.OptionalAttribute1 = command.FieldValues.TryGetValue(Aspect.OptionalAttribute1, out string? optionalAttribute1) ? Enum.Parse<Attribute>(optionalAttribute1) : null;
      aspect.OptionalAttribute2 = command.FieldValues.TryGetValue(Aspect.OptionalAttribute2, out string? optionalAttribute2) ? Enum.Parse<Attribute>(optionalAttribute2) : null;
      aspect.DiscountedSkill1 = command.FieldValues.TryGetValue(Aspect.DiscountedSkill1, out string? discountedSkill1) ? Enum.Parse<Skill>(discountedSkill1) : null;
      aspect.DiscountedSkill2 = command.FieldValues.TryGetValue(Aspect.DiscountedSkill1, out string? discountedSkill2) ? Enum.Parse<Skill>(discountedSkill2) : null;
    }
    else
    {
      aspect.UniqueSlug = locale.UniqueName.Value;
      aspect.DisplayName = locale.DisplayName?.Value;
      aspect.Description = locale.Description?.Value;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
