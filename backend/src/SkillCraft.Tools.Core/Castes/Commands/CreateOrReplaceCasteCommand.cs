using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Core.Castes.Validators;
using SkillCraft.Tools.Core.Customizations;

namespace SkillCraft.Tools.Core.Castes.Commands;

public record CreateOrReplaceCasteResult(CasteModel? Caste = null, bool Created = false);

public record CreateOrReplaceCasteCommand(Guid? Id, CreateOrReplaceCastePayload Payload, long? Version) : IRequest<CreateOrReplaceCasteResult>;

internal class CreateOrReplaceCasteCommandHandler : IRequestHandler<CreateOrReplaceCasteCommand, CreateOrReplaceCasteResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ICustomizationRepository _customizationRepository;
  private readonly ICasteManager _casteManager;
  private readonly ICasteQuerier _casteQuerier;
  private readonly ICasteRepository _casteRepository;

  public CreateOrReplaceCasteCommandHandler(
    IApplicationContext applicationContext,
    ICustomizationRepository customizationRepository,
    ICasteManager casteManager,
    ICasteQuerier casteQuerier,
    ICasteRepository casteRepository)
  {
    _applicationContext = applicationContext;
    _customizationRepository = customizationRepository;
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

    // TODO(fpion): Features

    caste.Update(actorId);

    await _casteManager.SaveAsync(caste, cancellationToken);

    CasteModel model = await _casteQuerier.ReadAsync(caste, cancellationToken);
    return new CreateOrReplaceCasteResult(model, created);
  }
}
