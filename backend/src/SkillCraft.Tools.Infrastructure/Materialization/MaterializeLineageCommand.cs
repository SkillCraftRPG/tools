using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Materialization;

public record MaterializeLineageCommand : IRequest
{
  public ContentLocalePublished Event { get; }
  public IReadOnlyDictionary<string, string> FieldValues { get; }
  public ContentLocale? Locale { get; }

  public MaterializeLineageCommand(ContentLocalePublished @event, IReadOnlyDictionary<string, string> fieldValues, ContentLocale? locale = null)
  {
    Event = @event;
    FieldValues = fieldValues;
    Locale = locale;
  }
}

internal class MaterializeLineageCommandHandler : IRequestHandler<MaterializeLineageCommand>
{
  private readonly SkillCraftContext _context;

  public MaterializeLineageCommandHandler(SkillCraftContext context)
  {
    _context = context;
  }

  public async Task Handle(MaterializeLineageCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;

    LineageEntity? lineage = await _context.Lineages
      .Include(x => x.Languages)
      .Include(x => x.Traits)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (lineage == null)
    {
      lineage = new(command.Event);
      _context.Lineages.Add(lineage);
    }
    else
    {
      lineage.Update(command.Event);
    }

    ContentLocale? locale = command.Locale;
    if (locale == null)
    {
      LineageEntity? parent = null;
      if (command.FieldValues.TryGetValue(Lineage.Parent, out string? parentValue))
      {
        Guid parentId = Guid.Parse(parentValue);
        parent = await _context.Lineages.SingleOrDefaultAsync(x => x.Id == parentId, cancellationToken);
      }
      lineage.SetParent(parent);

      lineage.Agility = command.FieldValues.TryGetValue(Lineage.Agility, out string? agility) ? int.Parse(agility) : 0;
      lineage.Coordination = command.FieldValues.TryGetValue(Lineage.Coordination, out string? coordination) ? int.Parse(coordination) : 0;
      lineage.Intellect = command.FieldValues.TryGetValue(Lineage.Intellect, out string? intellect) ? int.Parse(intellect) : 0;
      lineage.Presence = command.FieldValues.TryGetValue(Lineage.Presence, out string? presence) ? int.Parse(presence) : 0;
      lineage.Sensitivity = command.FieldValues.TryGetValue(Lineage.Sensitivity, out string? sensitivity) ? int.Parse(sensitivity) : 0;
      lineage.Spirit = command.FieldValues.TryGetValue(Lineage.Spirit, out string? spirit) ? int.Parse(spirit) : 0;
      lineage.Vigor = command.FieldValues.TryGetValue(Lineage.Vigor, out string? vigor) ? int.Parse(vigor) : 0;
      lineage.ExtraAttributes = command.FieldValues.TryGetValue(Lineage.ExtraAttributes, out string? extraAttributes) ? int.Parse(extraAttributes) : 0;

      lineage.Traits.Clear();
      if (command.FieldValues.TryGetValue(Lineage.Traits, out string? traitsValue))
      {
        IEnumerable<Guid> traitIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(traitsValue) ?? [];
        TraitEntity[] traits = traitIds.Any()
          ? await _context.Traits.Where(trait => traitIds.Contains(trait.Id)).ToArrayAsync(cancellationToken)
          : [];
        lineage.Traits.AddRange(traits);
      }

      lineage.Languages.Clear();
      if (command.FieldValues.TryGetValue(Lineage.Languages, out string? languagesValue))
      {
        IEnumerable<Guid> languageIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(languagesValue) ?? [];
        LanguageEntity[] languages = languageIds.Any()
          ? await _context.Languages.Where(language => languageIds.Contains(language.Id)).ToArrayAsync(cancellationToken)
          : [];
        lineage.Languages.AddRange(languages);
      }
      lineage.ExtraLanguages = command.FieldValues.TryGetValue(Lineage.ExtraLanguages, out string? extraLanguages) ? int.Parse(extraLanguages) : 0;
      lineage.LanguagesText = command.FieldValues.TryGetValue(Lineage.LanguagesText, out string? languagesText) ? languagesText : null;

      if (command.FieldValues.TryGetValue(Lineage.FamilyNames, out string? familyNames))
      {
        IEnumerable<string> names = (JsonSerializer.Deserialize<IEnumerable<string>>(familyNames) ?? [])
          .Where(name => !string.IsNullOrWhiteSpace(name))
          .Select(name => name.Trim())
          .Distinct()
          .OrderBy(name => name);
        lineage.FamilyNames = names.Any() ? JsonSerializer.Serialize(names) : null;
      }
      else
      {
        lineage.FamilyNames = null;
      }

      if (command.FieldValues.TryGetValue(Lineage.FemaleNames, out string? femaleNames))
      {
        IEnumerable<string> names = (JsonSerializer.Deserialize<IEnumerable<string>>(femaleNames) ?? [])
          .Where(name => !string.IsNullOrWhiteSpace(name))
          .Select(name => name.Trim())
          .Distinct()
          .OrderBy(name => name);
        lineage.FemaleNames = names.Any() ? JsonSerializer.Serialize(names) : null;
      }
      else
      {
        lineage.FemaleNames = null;
      }

      if (command.FieldValues.TryGetValue(Lineage.MaleNames, out string? maleNames))
      {
        IEnumerable<string> names = (JsonSerializer.Deserialize<IEnumerable<string>>(maleNames) ?? [])
          .Where(name => !string.IsNullOrWhiteSpace(name))
          .Select(name => name.Trim())
          .Distinct()
          .OrderBy(name => name);
        lineage.MaleNames = names.Any() ? JsonSerializer.Serialize(names) : null;
      }
      else
      {
        lineage.MaleNames = null;
      }

      if (command.FieldValues.TryGetValue(Lineage.UnisexNames, out string? unisexNames))
      {
        IEnumerable<string> names = (JsonSerializer.Deserialize<IEnumerable<string>>(unisexNames) ?? [])
          .Where(name => !string.IsNullOrWhiteSpace(name))
          .Select(name => name.Trim())
          .Distinct()
          .OrderBy(name => name);
        lineage.UnisexNames = names.Any() ? JsonSerializer.Serialize(names) : null;
      }
      else
      {
        lineage.UnisexNames = null;
      }

      if (command.FieldValues.TryGetValue(Lineage.CustomNames, out string? customNamesValue))
      {
        Dictionary<string, string[]> customNames = JsonSerializer.Deserialize<Dictionary<string, string[]>>(customNamesValue) ?? [];
        foreach (KeyValuePair<string, string[]> category in customNames)
        {
          string[] names = [.. category.Value
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .Distinct()
            .OrderBy(name => name)];
          if (names.Length < 1)
          {
            customNames.Remove(category.Key);
          }
          else
          {
            customNames[category.Key] = names;
          }
        }
        lineage.CustomNames = customNames.Count > 0 ? JsonSerializer.Serialize(customNames) : null;
      }
      else
      {
        lineage.CustomNames = null;
      }

      lineage.WalkSpeed = command.FieldValues.TryGetValue(Lineage.WalkSpeed, out string? walkSpeed) ? int.Parse(walkSpeed) : 0;
      lineage.ClimbSpeed = command.FieldValues.TryGetValue(Lineage.ClimbSpeed, out string? climbSpeed) ? int.Parse(climbSpeed) : 0;
      lineage.SwimSpeed = command.FieldValues.TryGetValue(Lineage.SwimSpeed, out string? swimSpeed) ? int.Parse(swimSpeed) : 0;
      lineage.FlySpeed = command.FieldValues.TryGetValue(Lineage.FlySpeed, out string? flySpeed) ? int.Parse(flySpeed) : 0;
      lineage.HoverSpeed = command.FieldValues.TryGetValue(Lineage.HoverSpeed, out string? hoverSpeed) ? int.Parse(hoverSpeed) : 0;
      lineage.BurrowSpeed = command.FieldValues.TryGetValue(Lineage.BurrowSpeed, out string? burrowSpeed) ? int.Parse(burrowSpeed) : 0;

      if (command.FieldValues.TryGetValue(Lineage.SizeCategory, out string? sizeCategory))
      {
        lineage.SizeCategory = Enum.Parse<SizeCategory>(sizeCategory);
      }
      lineage.SizeRoll = command.FieldValues.TryGetValue(Lineage.SizeRoll, out string? sizeRoll) ? sizeRoll : null;

      lineage.StarvedRoll = command.FieldValues.TryGetValue(Lineage.StarvedRoll, out string? starvedRoll) ? starvedRoll : null;
      lineage.SkinnyRoll = command.FieldValues.TryGetValue(Lineage.SkinnyRoll, out string? skinnyRoll) ? skinnyRoll : null;
      lineage.NormalRoll = command.FieldValues.TryGetValue(Lineage.NormalRoll, out string? normalRoll) ? normalRoll : null;
      lineage.OverweightRoll = command.FieldValues.TryGetValue(Lineage.OverweightRoll, out string? overweightRoll) ? overweightRoll : null;
      lineage.ObeseRoll = command.FieldValues.TryGetValue(Lineage.ObeseRoll, out string? obeseRoll) ? obeseRoll : null;

      lineage.AdolescentAge = command.FieldValues.TryGetValue(Lineage.AdolescentAge, out string? adolescentAge) ? int.Parse(adolescentAge) : null;
      lineage.AdultAge = command.FieldValues.TryGetValue(Lineage.AdultAge, out string? adultAge) ? int.Parse(adultAge) : null;
      lineage.MatureAge = command.FieldValues.TryGetValue(Lineage.MatureAge, out string? matureAge) ? int.Parse(matureAge) : null;
      lineage.VenerableAge = command.FieldValues.TryGetValue(Lineage.VenerableAge, out string? venerableAge) ? int.Parse(venerableAge) : null;
    }
    else
    {
      lineage.UniqueSlug = locale.UniqueName.Value;
      lineage.DisplayName = locale.DisplayName?.Value;
      lineage.Description = locale.Description?.Value;

      lineage.LanguagesText = command.FieldValues.TryGetValue(Lineage.LanguagesText, out string? languagesText) ? languagesText : null;
      lineage.NamesText = command.FieldValues.TryGetValue(Lineage.NamesText, out string? namesText) ? namesText : null;
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
