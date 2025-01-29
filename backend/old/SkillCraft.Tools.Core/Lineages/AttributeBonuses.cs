using FluentValidation;
using SkillCraft.Tools.Core.Lineages.Validators;

namespace SkillCraft.Tools.Core.Lineages;

public record AttributeBonuses : IAttributeBonuses
{
  public int Agility { get; }
  public int Coordination { get; }
  public int Intellect { get; }
  public int Presence { get; }
  public int Sensitivity { get; }
  public int Spirit { get; }
  public int Vigor { get; }

  public int Extra { get; }

  public AttributeBonuses()
  {
  }

  public AttributeBonuses(IAttributeBonuses attributes)
    : this(attributes.Agility, attributes.Coordination, attributes.Intellect, attributes.Presence, attributes.Sensitivity, attributes.Spirit, attributes.Vigor, attributes.Extra)
  {
  }

  [JsonConstructor]
  public AttributeBonuses(int agility, int coordination, int intellect, int presence, int sensitivity, int spirit, int vigor, int extra)
  {
    Agility = agility;
    Coordination = coordination;
    Intellect = intellect;
    Presence = presence;
    Sensitivity = sensitivity;
    Spirit = spirit;
    Vigor = vigor;

    Extra = extra;

    new AttributeBonusesValidator().ValidateAndThrow(this);
  }
}
