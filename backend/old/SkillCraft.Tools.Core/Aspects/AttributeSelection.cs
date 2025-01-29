using FluentValidation;
using SkillCraft.Tools.Core.Aspects.Validators;

namespace SkillCraft.Tools.Core.Aspects;

public record AttributeSelection : IAttributeSelection
{
  public Ability? Mandatory1 { get; }
  public Ability? Mandatory2 { get; }
  public Ability? Optional1 { get; }
  public Ability? Optional2 { get; }

  public AttributeSelection(IAttributeSelection attributes) : this(attributes.Mandatory1, attributes.Mandatory2, attributes.Optional1, attributes.Optional2)
  {
  }

  [JsonConstructor]
  public AttributeSelection(Ability? mandatory1 = null, Ability? mandatory2 = null, Ability? optional1 = null, Ability? optional2 = null)
  {
    Mandatory1 = mandatory1;
    Mandatory2 = mandatory2;
    Optional1 = optional1;
    Optional2 = optional2;
    new AttributeSelectionValidator().ValidateAndThrow(this);
  }
}
