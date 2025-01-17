namespace SkillCraft.Tools.Core.Lineages;

public interface IAttributeBonuses
{
  int Agility { get; }
  int Coordination { get; }
  int Intellect { get; }
  int Presence { get; }
  int Sensitivity { get; }
  int Spirit { get; }
  int Vigor { get; }

  int Extra { get; }
}
