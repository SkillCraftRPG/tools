using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Core.Castes.Validators;

namespace SkillCraft.Tools.Core.Castes.Commands;

public record CreateOrReplaceCasteResult(CasteModel? Caste = null, bool Created = false);

public record CreateOrReplaceCasteCommand(Guid? Id, CreateOrReplaceCastePayload Payload, long? Version) : Activity, IRequest<CreateOrReplaceCasteResult>;

internal class CreateOrReplaceCasteCommandHandler : IRequestHandler<CreateOrReplaceCasteCommand, CreateOrReplaceCasteResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ICasteManager _casteManager;
  private readonly ICasteQuerier _casteQuerier;
  private readonly ICasteRepository _casteRepository;

  public CreateOrReplaceCasteCommandHandler(
    IApplicationContext applicationContext,
    ICasteManager casteManager,
    ICasteQuerier casteQuerier,
    ICasteRepository casteRepository)
  {
    _applicationContext = applicationContext;
    _casteManager = casteManager;
    _casteQuerier = casteQuerier;
    _casteRepository = casteRepository;
  }

  public async Task<CreateOrReplaceCasteResult> Handle(CreateOrReplaceCasteCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceCastePayload payload = command.Payload;
    new CreateOrReplaceCasteValidator().ValidateAndThrow(payload);

    CasteId? casteId = null;
    Caste? caste = null;
    if (command.Id.HasValue)
    {
      casteId = new(command.Id.Value);
      caste = await _casteRepository.LoadAsync(casteId.Value, cancellationToken);
    }

    Slug uniqueSlug = new(payload.UniqueSlug);
    ActorId? actorId = _applicationContext.ActorId;
    bool created = false;
    if (caste == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceCasteResult();
      }

      caste = new(uniqueSlug, actorId, casteId);
      created = true;
    }

    Caste reference = (command.Version.HasValue
      ? await _casteRepository.LoadAsync(caste.Id, command.Version.Value, cancellationToken)
      : null) ?? caste;

    if (reference.UniqueSlug != uniqueSlug)
    {
      caste.UniqueSlug = uniqueSlug;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      caste.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      caste.Description = description;
    }

    if (reference.Skill != payload.Skill)
    {
      caste.Skill = payload.Skill;
    }
    Roll? wealthRoll = Roll.TryCreate(payload.WealthRoll);
    if (reference.WealthRoll != wealthRoll)
    {
      caste.WealthRoll = wealthRoll;
    }

    SetFeatures(caste, reference, payload);

    caste.Update(actorId);

    await _casteManager.SaveAsync(caste, cancellationToken);

    CasteModel model = await _casteQuerier.ReadAsync(caste, cancellationToken);
    return new CreateOrReplaceCasteResult(model, created);
  }

  private static void SetFeatures(Caste caste, Caste reference, CreateOrReplaceCastePayload payload)
  {
    HashSet<Guid> featureIds = payload.Features.Where(x => x.Id.HasValue).Select(x => x.Id!.Value).ToHashSet();
    foreach (Guid featureId in reference.Features.Keys)
    {
      if (!featureIds.Contains(featureId))
      {
        caste.RemoveFeature(featureId);
      }
    }

    foreach (FeaturePayload featurePayload in payload.Features)
    {
      Feature feature = new(new DisplayName(featurePayload.Name), Description.TryCreate(featurePayload.Description));
      if (featurePayload.Id.HasValue)
      {
        if (!reference.Features.TryGetValue(featurePayload.Id.Value, out Feature? existingFeature) || existingFeature != feature)
        {
          caste.SetFeature(featurePayload.Id.Value, feature);
        }
      }
      else
      {
        caste.AddFeature(feature);
      }
    }
  }
}
