using Logitar;
using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization;

public record MaterializeSpecializationCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeSpecializationCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeSpecializationCommandHandler : IRequestHandler<MaterializeSpecializationCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeSpecializationCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeSpecializationCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    SpecializationEntity? specialization = await _context.Specializations
      .Include(x => x.OptionalTalents)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (specialization == null)
    {
      specialization = new(command.Event);
      _context.Specializations.Add(specialization);
    }
    else
    {
      specialization.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale == null)
    {
      if (command.FieldValues.TryGetValue(Specialization.Tier, out string? tier))
      {
        specialization.Tier = int.Parse(tier);
      }

      Guid? requiredTalentId = command.FieldValues.TryGetValue(Specialization.RequiredTalent, out string? requiredTalentValue) ? Guid.Parse(requiredTalentValue) : null;
      TalentEntity? requiredTalent = requiredTalentId.HasValue
        ? await _context.Talents.SingleOrDefaultAsync(x => x.Id == requiredTalentId.Value, cancellationToken)
        : null;
      specialization.SetRequiredTalent(requiredTalent);

      specialization.OptionalTalents.Clear();
      if (command.FieldValues.TryGetValue(Specialization.OptionalTalents, out string? optionalTalentsValue))
      {
        IEnumerable<Guid> talentIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(optionalTalentsValue) ?? [];
        TalentEntity[] talents = talentIds.Any()
          ? await _context.Talents.Where(talent => talentIds.Contains(talent.Id)).ToArrayAsync(cancellationToken)
          : [];
        specialization.OptionalTalents.AddRange(talents);
      }
    }
    else
    {
      specialization.UniqueSlug = locale.UniqueName.Value;
      specialization.DisplayName = locale.DisplayName?.Value;
      specialization.Description = locale.Description?.Value;

      if (command.FieldValues.TryGetValue(Specialization.OtherRequirements, out string? otherRequirements))
      {
        IEnumerable<string> values = ParseStringList(otherRequirements);
        specialization.OtherRequirements = values.Any() ? JsonSerializer.Serialize(values) : null;
      }
      else
      {
        specialization.OtherRequirements = null;
      }

      if (command.FieldValues.TryGetValue(Specialization.OtherOptions, out string? otherOptions))
      {
        IEnumerable<string> values = ParseStringList(otherOptions);
        specialization.OtherOptions = values.Any() ? JsonSerializer.Serialize(values) : null;
      }
      else
      {
        specialization.OtherOptions = null;
      }

      specialization.ReservedTalentName = command.FieldValues.TryGetValue(Specialization.ReservedTalentName, out string? reservedTalentName) ? reservedTalentName : null;
      specialization.ReservedTalentDescription = command.FieldValues.TryGetValue(Specialization.ReservedTalentDescription, out string? reservedTalentDescription) ? reservedTalentDescription : null;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }

  private static IEnumerable<string> ParseStringList(string value) => value.Remove("\r").Split('\n')
    .Where(value => !string.IsNullOrWhiteSpace(value))
    .Select(value => value.Trim())
    .Distinct();
}
