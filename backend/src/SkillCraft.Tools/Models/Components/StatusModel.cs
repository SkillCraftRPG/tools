using Logitar.Portal.Contracts.Actors;

namespace SkillCraft.Tools.Models.Components;

public record StatusModel(string Text, ActorModel Actor, DateTime Timestamp);
