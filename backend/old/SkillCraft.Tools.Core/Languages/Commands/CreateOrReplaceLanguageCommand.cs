using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Core.Languages.Validators;

namespace SkillCraft.Tools.Core.Languages.Commands;

public record CreateOrReplaceLanguageResult(LanguageModel? Language = null, bool Created = false);

public record CreateOrReplaceLanguageCommand(Guid? Id, CreateOrReplaceLanguagePayload Payload, long? Version) : Activity, IRequest<CreateOrReplaceLanguageResult>;

internal class CreateOrReplaceLanguageCommandHandler : IRequestHandler<CreateOrReplaceLanguageCommand, CreateOrReplaceLanguageResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ILanguageManager _languageManager;
  private readonly ILanguageQuerier _languageQuerier;
  private readonly ILanguageRepository _languageRepository;

  public CreateOrReplaceLanguageCommandHandler(
    IApplicationContext applicationContext,
    ILanguageManager languageManager,
    ILanguageQuerier languageQuerier,
    ILanguageRepository languageRepository)
  {
    _applicationContext = applicationContext;
    _languageManager = languageManager;
    _languageQuerier = languageQuerier;
    _languageRepository = languageRepository;
  }

  public async Task<CreateOrReplaceLanguageResult> Handle(CreateOrReplaceLanguageCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceLanguagePayload payload = command.Payload;
    new CreateOrReplaceLanguageValidator().ValidateAndThrow(payload);

    LanguageId? languageId = null;
    Language? language = null;
    if (command.Id.HasValue)
    {
      languageId = new(command.Id.Value);
      language = await _languageRepository.LoadAsync(languageId.Value, cancellationToken);
    }

    Slug uniqueSlug = new(payload.UniqueSlug);
    ActorId? actorId = _applicationContext.ActorId;
    bool created = false;
    if (language == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceLanguageResult();
      }

      language = new(uniqueSlug, actorId, languageId);
      created = true;
    }

    Language reference = (command.Version.HasValue
      ? await _languageRepository.LoadAsync(language.Id, command.Version.Value, cancellationToken)
      : null) ?? language;

    if (reference.UniqueSlug != uniqueSlug)
    {
      language.UniqueSlug = uniqueSlug;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      language.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      language.Description = description;
    }

    Script? script = Script.TryCreate(payload.Script);
    if (reference.Script != script)
    {
      language.Script = script;
    }
    TypicalSpeakers? typicalSpeakers = TypicalSpeakers.TryCreate(payload.TypicalSpeakers);
    if (reference.TypicalSpeakers != typicalSpeakers)
    {
      language.TypicalSpeakers = typicalSpeakers;
    }

    language.Update(actorId);

    await _languageManager.SaveAsync(language, cancellationToken);

    LanguageModel model = await _languageQuerier.ReadAsync(language, cancellationToken);
    return new CreateOrReplaceLanguageResult(model, created);
  }
}
