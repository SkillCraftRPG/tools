using FluentValidation;
using SkillCraft.Tools.Core.Lineages.Validators;

namespace SkillCraft.Tools.Core.Lineages;

public record Speeds : ISpeeds
{
  public int Walk { get; }
  public int Climb { get; }
  public int Swim { get; }
  public int Fly { get; }
  public int Hover { get; }
  public int Burrow { get; }

  public Speeds()
  {
  }

  public Speeds(ISpeeds speeds)
    : this(speeds.Walk, speeds.Climb, speeds.Swim, speeds.Fly, speeds.Hover, speeds.Burrow)
  {
  }

  [JsonConstructor]
  public Speeds(int walk, int climb, int swim, int fly, int hover, int burrow)
  {
    Walk = walk;
    Climb = climb;
    Swim = swim;
    Fly = fly;
    Hover = hover;
    Burrow = burrow;
    new SpeedsValidator().ValidateAndThrow(this);
  }
}
