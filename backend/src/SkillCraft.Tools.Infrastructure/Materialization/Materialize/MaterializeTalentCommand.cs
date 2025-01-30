using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization.Materialize;

public record MaterializeTalentCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeTalentCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeTalentCommandHandler : IRequestHandler<MaterializeTalentCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeTalentCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeTalentCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    TalentEntity? talent = await _context.Talents
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (talent == null)
    {
      talent = new(command.Event);
      _context.Talents.Add(talent);
    }
    else
    {
      talent.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale == null)
    {
      if (command.FieldValues.TryGetValue(Talent.Tier, out string? tier))
      {
        talent.Tier = int.Parse(tier);
      }
      if (command.FieldValues.TryGetValue(Talent.AllowMultiplePurchases, out string? allowMultiplePurchases))
      {
        talent.AllowMultiplePurchases = bool.Parse(allowMultiplePurchases);
      }
      talent.Skill = command.FieldValues.TryGetValue(Talent.Skill, out string? skill) ? Enum.Parse<Skill>(skill) : null;

      Guid? requiredTalentId = command.FieldValues.TryGetValue(Talent.RequiredTalent, out string? requiredTalentValue) ? Guid.Parse(requiredTalentValue) : null;
      TalentEntity? requiredTalent = requiredTalentId.HasValue
        ? await _context.Talents.SingleOrDefaultAsync(x => x.Id == requiredTalentId.Value, cancellationToken)
        : null;
      talent.SetRequiredTalent(requiredTalent);
    }
    else
    {
      talent.UniqueSlug = locale.UniqueName.Value;
      talent.DisplayName = locale.DisplayName?.Value;
      talent.Description = locale.Description?.Value;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
