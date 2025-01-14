﻿using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Languages.Events;

namespace SkillCraft.Tools.Core.Languages;

public class Language : AggregateRoot
{
  private LanguageUpdated _updated = new();

  public new LanguageId Id => new(base.Id);

  private Slug? _uniqueSlug = null;
  public Slug UniqueSlug
  {
    get => _uniqueSlug ?? throw new InvalidOperationException($"The {nameof(UniqueSlug)} has not been initialized yet.");
    set
    {
      if (_uniqueSlug != value)
      {
        _uniqueSlug = value;
        _updated.UniqueSlug = value;
      }
    }
  }
  private DisplayName? _displayName = null;
  public DisplayName? DisplayName
  {
    get => _displayName;
    set
    {
      if (_displayName != value)
      {
        _displayName = value;
        _updated.DisplayName = new Change<DisplayName>(value);
      }
    }
  }
  private Description? _description = null;
  public Description? Description
  {
    get => _description;
    set
    {
      if (_description != value)
      {
        _description = value;
        _updated.Description = new Change<Description>(value);
      }
    }
  }

  private Script? _script = null;
  public Script? Script
  {
    get => _script;
    set
    {
      if (_script != value)
      {
        _script = value;
        _updated.Script = new Change<Script>(value);
      }
    }
  }
  private TypicalSpeakers? _typicalSpeakers = null;
  public TypicalSpeakers? TypicalSpeakers
  {
    get => _typicalSpeakers;
    set
    {
      if (_typicalSpeakers != value)
      {
        _typicalSpeakers = value;
        _updated.TypicalSpeakers = new Change<TypicalSpeakers>(value);
      }
    }
  }

  public Language() : base()
  {
  }

  public Language(Slug uniqueSlug, ActorId? actorId = null, LanguageId? languageId = null) : base(languageId?.StreamId)
  {
    Raise(new LanguageCreated(uniqueSlug), actorId);
  }
  protected virtual void Handle(LanguageCreated @event)
  {
    _uniqueSlug = @event.UniqueSlug;
  }

  public void Update(ActorId? actorId = null)
  {
    if (_updated.HasChanges)
    {
      Raise(_updated, actorId, DateTime.Now);
      _updated = new();
    }
  }
  protected virtual void Handle(LanguageUpdated @event)
  {
    if (@event.UniqueSlug != null)
    {
      _uniqueSlug = @event.UniqueSlug;
    }
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName.Value;
    }
    if (@event.Description != null)
    {
      _description = @event.Description.Value;
    }

    if (@event.Script != null)
    {
      _script = @event.Script.Value;
    }
    if (@event.TypicalSpeakers != null)
    {
      _typicalSpeakers = @event.TypicalSpeakers.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
