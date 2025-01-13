using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Core.Languages.Validators;

namespace SkillCraft.Tools.Core.Languages.Commands;

public record CreateOrReplaceLanguageResult(LanguageModel? Language = null, bool Created = false);

public record CreateOrReplaceLanguageCommand(Guid? Id, CreateOrReplaceLanguagePayload Payload, long? Version) : IRequest<CreateOrReplaceLanguageResult>;

internal class CreateOrReplaceLanguageCommandHandler : IRequestHandler<CreateOrReplaceLanguageCommand, CreateOrReplaceLanguageResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ICustomizationRepository _customizationRepository;
  private readonly ILanguageManager _casteManager;
  private readonly ILanguageQuerier _casteQuerier;
  private readonly ILanguageRepository _casteRepository;

  public CreateOrReplaceLanguageCommandHandler(
    IApplicationContext applicationContext,
    ICustomizationRepository customizationRepository,
    ILanguageManager casteManager,
    ILanguageQuerier casteQuerier,
    ILanguageRepository casteRepository)
  {
    _applicationContext = applicationContext;
    _customizationRepository = customizationRepository;
    _casteManager = casteManager;
    _casteQuerier = casteQuerier;
    _casteRepository = casteRepository;
  }

  public async Task<CreateOrReplaceLanguageResult> Handle(CreateOrReplaceLanguageCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceLanguagePayload payload = command.Payload;
    new CreateOrReplaceLanguageValidator().ValidateAndThrow(payload);

    LanguageId? casteId = null;
    Language? caste = null;
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
        return new CreateOrReplaceLanguageResult();
      }

      caste = new(uniqueSlug, actorId, casteId);
      created = true;
    }

    Language reference = (command.Version.HasValue
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

    // TODO(fpion): Script
    // TODO(fpion): TypicalSpeakers

    caste.Update(actorId);

    await _casteManager.SaveAsync(caste, cancellationToken);

    LanguageModel model = await _casteQuerier.ReadAsync(caste, cancellationToken);
    return new CreateOrReplaceLanguageResult(model, created);
  }
}
