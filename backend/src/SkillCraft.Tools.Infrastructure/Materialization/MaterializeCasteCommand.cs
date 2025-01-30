using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization;

public record MaterializeCasteCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeCasteCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeCasteCommandHandler : IRequestHandler<MaterializeCasteCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeCasteCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeCasteCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    CasteEntity? caste = await _context.Castes
      .Include(x => x.Features)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (caste == null)
    {
      caste = new(command.Event);
      _context.Castes.Add(caste);
    }
    else
    {
      caste.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale == null)
    {
      caste.Skill = command.FieldValues.TryGetValue(Caste.Skill, out string? skill) ? Enum.Parse<Skill>(skill) : null;
      caste.WealthRoll = command.FieldValues.TryGetValue(Caste.WealthRoll, out string? wealthRoll) ? wealthRoll : null;

      caste.Features.Clear();
      if (command.FieldValues.TryGetValue(Caste.Features, out string? featuresValue))
      {
        IEnumerable<Guid> featureIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(featuresValue) ?? [];
        FeatureEntity[] features = featureIds.Any()
          ? await _context.Features.Where(feature => featureIds.Contains(feature.Id)).ToArrayAsync(cancellationToken)
          : [];
        caste.Features.AddRange(features);
      }
    }
    else
    {
      caste.UniqueSlug = locale.UniqueName.Value;
      caste.DisplayName = locale.DisplayName?.Value;
      caste.Description = locale.Description?.Value;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
