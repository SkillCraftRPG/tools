using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Languages;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.Core.Lineages.Validators;

namespace SkillCraft.Tools.Core.Lineages.Commands;

public record CreateOrReplaceLineageResult(LineageModel? Lineage = null, bool Created = false);

public record CreateOrReplaceLineageCommand(Guid? Id, CreateOrReplaceLineagePayload Payload, long? Version) : Activity, IRequest<CreateOrReplaceLineageResult>;

internal class CreateOrReplaceLineageCommandHandler : IRequestHandler<CreateOrReplaceLineageCommand, CreateOrReplaceLineageResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ILanguageRepository _languageRepository;
  private readonly ILineageManager _lineageManager;
  private readonly ILineageQuerier _lineageQuerier;
  private readonly ILineageRepository _lineageRepository;

  public CreateOrReplaceLineageCommandHandler(
    IApplicationContext applicationContext,
    ILanguageRepository languageRepository,
    ILineageManager lineageManager,
    ILineageQuerier lineageQuerier,
    ILineageRepository lineageRepository)
  {
    _applicationContext = applicationContext;
    _languageRepository = languageRepository;
    _lineageManager = lineageManager;
    _lineageQuerier = lineageQuerier;
    _lineageRepository = lineageRepository;
  }

  public async Task<CreateOrReplaceLineageResult> Handle(CreateOrReplaceLineageCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceLineagePayload payload = command.Payload;
    new CreateOrReplaceLineageValidator().ValidateAndThrow(payload);

    LineageId? lineageId = null;
    Lineage? lineage = null;
    if (command.Id.HasValue)
    {
      lineageId = new(command.Id.Value);
      lineage = await _lineageRepository.LoadAsync(lineageId.Value, cancellationToken);
    }

    Slug uniqueSlug = new(payload.UniqueSlug);
    ActorId? actorId = _applicationContext.ActorId;
    bool created = false;
    if (lineage == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceLineageResult();
      }

      Lineage? parent = null;
      if (payload.ParentId.HasValue)
      {
        LineageId parentId = new(payload.ParentId.Value);
        parent = await _lineageRepository.LoadAsync(parentId, cancellationToken)
          ?? throw new NotImplementedException(); // ISSUE #56: https://github.com/SkillCraftRPG/tools/issues/56
      }

      lineage = new(uniqueSlug, parent, actorId, lineageId);
      created = true;
    }

    Lineage reference = (command.Version.HasValue
      ? await _lineageRepository.LoadAsync(lineage.Id, command.Version.Value, cancellationToken)
      : null) ?? lineage;

    if (reference.UniqueSlug != uniqueSlug)
    {
      lineage.UniqueSlug = uniqueSlug;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      lineage.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      lineage.Description = description;
    }

    AttributeBonuses attributes = new(payload.Attributes);
    if (attributes != reference.Attributes)
    {
      lineage.Attributes = attributes;
    }
    SetTraits(lineage, reference, payload);

    await SetLanguagesAsync(lineage, reference, payload.Languages, cancellationToken);
    SetNames(lineage, reference, payload.Names);

    Speeds speeds = new(payload.Speeds);
    if (speeds != reference.Speeds)
    {
      lineage.Speeds = speeds;
    }
    Size size = new(payload.Size.Category, Roll.TryCreate(payload.Size.Roll));
    if (size != reference.Size)
    {
      lineage.Size = size;
    }
    Weight weight = new(
      Roll.TryCreate(payload.Weight.Starved),
      Roll.TryCreate(payload.Weight.Skinny),
      Roll.TryCreate(payload.Weight.Normal),
      Roll.TryCreate(payload.Weight.Overweight),
      Roll.TryCreate(payload.Weight.Obese));
    if (weight != reference.Weight)
    {
      lineage.Weight = weight;
    }
    Ages ages = new(payload.Ages);
    if (ages != reference.Ages)
    {
      lineage.Ages = ages;
    }

    lineage.Update(actorId);

    await _lineageManager.SaveAsync(lineage, cancellationToken);

    LineageModel model = await _lineageQuerier.ReadAsync(lineage, cancellationToken);
    return new CreateOrReplaceLineageResult(model, created);
  }

  private async Task SetLanguagesAsync(Lineage lineage, Lineage reference, LanguagesPayload payload, CancellationToken cancellationToken)
  {
    HashSet<LanguageId> languageIds = payload.Ids.Select(id => new LanguageId(id)).ToHashSet();
    IReadOnlyCollection<Language> languageItems = await _languageRepository.LoadAsync(languageIds, cancellationToken);

    IEnumerable<LanguageId> missingLanguages = languageIds.Except(languageItems.Select(language => language.Id));
    if (missingLanguages.Any())
    {
      throw new NotImplementedException(); // ISSUE #56: https://github.com/SkillCraftRPG/tools/issues/56
    }

    Languages languages = new(languageItems, payload.Extra, payload.Text);
    if (languages != reference.Languages)
    {
      lineage.Languages = languages;
    }
  }

  private static void SetNames(Lineage lineage, Lineage reference, NamesModel payload)
  {
    Dictionary<string, IReadOnlyCollection<string>> custom = new(capacity: payload.Custom.Count);
    foreach (NameCategory category in payload.Custom)
    {
      custom[category.Key] = category.Values;
    }
    Names names = new(payload.Text, payload.Family, payload.Female, payload.Male, payload.Unisex, custom);
    if (names != reference.Names)
    {
      lineage.Names = names;
    }
  }

  private static void SetTraits(Lineage lineage, Lineage reference, CreateOrReplaceLineagePayload payload)
  {
    HashSet<Guid> traitIds = payload.Traits.Where(x => x.Id.HasValue).Select(x => x.Id!.Value).ToHashSet();
    foreach (Guid traitId in reference.Traits.Keys)
    {
      if (!traitIds.Contains(traitId))
      {
        lineage.RemoveTrait(traitId);
      }
    }

    foreach (TraitPayload traitPayload in payload.Traits)
    {
      Trait trait = new(new DisplayName(traitPayload.Name), Description.TryCreate(traitPayload.Description));
      if (traitPayload.Id.HasValue)
      {
        if (!reference.Traits.TryGetValue(traitPayload.Id.Value, out Trait? existingTrait) || existingTrait != trait)
        {
          lineage.SetTrait(traitPayload.Id.Value, trait);
        }
      }
      else
      {
        lineage.AddTrait(trait);
      }
    }
  }
}
