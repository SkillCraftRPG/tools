using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Materialize;

public record MaterializeEducationCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeEducationCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeEducationCommandHandler : IRequestHandler<MaterializeEducationCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeEducationCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeEducationCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    EducationEntity? education = await _context.Educations
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (education == null)
    {
      education = new(command.Event);
      _context.Educations.Add(education);
    }
    else
    {
      education.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale == null)
    {
      education.Skill = command.FieldValues.TryGetValue(Education.Skill, out string? skill) ? Enum.Parse<Skill>(skill) : null;
      education.WealthMultiplier = command.FieldValues.TryGetValue(Education.WealthMultiplier, out string? wealthMultiplier) ? double.Parse(wealthMultiplier) : null;
    }
    else
    {
      education.UniqueSlug = locale.UniqueName.Value;
      education.DisplayName = locale.DisplayName?.Value;
      education.Description = locale.Description?.Value;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
