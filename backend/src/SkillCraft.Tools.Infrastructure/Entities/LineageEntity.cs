﻿using Logitar.Cms.Core.Contents.Events;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class LineageEntity : AggregateEntity
{
  public int LineageId { get; private set; }
  public Guid Id { get; private set; }

  public LineageEntity? Parent { get; private set; }
  public int? ParentId { get; private set; }
  public List<LineageEntity> Children { get; private set; } = [];

  public string UniqueSlug { get; set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public int Agility { get; set; }
  public int Coordination { get; set; }
  public int Intellect { get; set; }
  public int Presence { get; set; }
  public int Sensitivity { get; set; }
  public int Spirit { get; set; }
  public int Vigor { get; set; }
  public int ExtraAttributes { get; set; }
  public List<TraitEntity> Traits { get; private set; } = [];

  public List<LanguageEntity> Languages { get; private set; } = [];
  public int ExtraLanguages { get; set; }
  public string? LanguagesText { get; set; }

  public string? NamesText { get; set; }
  public string? FamilyNames { get; set; }
  public string? FemaleNames { get; set; }
  public string? MaleNames { get; set; }
  public string? UnisexNames { get; set; }
  public string? CustomNames { get; set; }

  public int WalkSpeed { get; set; }
  public int ClimbSpeed { get; set; }
  public int SwimSpeed { get; set; }
  public int FlySpeed { get; set; }
  public int HoverSpeed { get; set; }
  public int BurrowSpeed { get; set; }

  public SizeCategory SizeCategory { get; set; }
  public string? SizeRoll { get; set; }

  public string? StarvedRoll { get; set; }
  public string? SkinnyRoll { get; set; }
  public string? NormalRoll { get; set; }
  public string? OverweightRoll { get; set; }
  public string? ObeseRoll { get; set; }

  public int? AdolescentAge { get; set; }
  public int? AdultAge { get; set; }
  public int? MatureAge { get; set; }
  public int? VenerableAge { get; set; }

  public LineageEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();
  }

  private LineageEntity() : base()
  {
  }

  public override IReadOnlyCollection<ActorId> GetActorIds() => GetActorIds(includeChildren: true, includeParent: true);
  public IReadOnlyCollection<ActorId> GetActorIds(bool includeChildren, bool includeParent)
  {
    List<ActorId> actorIds = [.. base.GetActorIds()];
    foreach (LanguageEntity language in Languages)
    {
      actorIds.AddRange(language.GetActorIds());
    }
    foreach (TraitEntity trait in Traits)
    {
      actorIds.AddRange(trait.GetActorIds());
    }
    if (includeChildren)
    {
      foreach (LineageEntity child in Children)
      {
        actorIds.AddRange(child.GetActorIds(includeChildren: true, includeParent: false));
      }
    }
    if (includeParent && Parent != null)
    {
      actorIds.AddRange(Parent.GetActorIds(includeChildren: false, includeParent: true));
    }
    return actorIds.AsReadOnly();
  }

  public void Update(ContentLocalePublished @event)
  {
    base.Update(@event);
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
