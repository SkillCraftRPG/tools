using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.Seeding.Game.Payloads;

public record LineagePayload
{
  public Guid Id { get; set; }

  public string? Parent { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public AttributeBonuses Attributes { get; set; } = new();
  public List<TraitPayload> Traits { get; set; } = [];

  public Languages Languages { get; set; } = new();
  public Names Names { get; set; } = new();

  public Speeds Speeds { get; set; } = new();
  public Size Size { get; set; } = new();
  public Weight Weight { get; set; } = new();
  public Ages Ages { get; set; } = new();
}

public record AttributeBonuses
{
  public int Agility { get; set; }
  public int Coordination { get; set; }
  public int Intellect { get; set; }
  public int Presence { get; set; }
  public int Sensitivity { get; set; }
  public int Spirit { get; set; }
  public int Vigor { get; set; }
  public int Extra { get; set; }
}

public record TraitPayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }
}

public record Languages
{
  public List<string> Items { get; set; } = [];
  public int Extra { get; set; }
  public string? Text { get; set; }
}

public record Names
{
  public string? Text { get; set; }
  public List<string> Family { get; set; } = [];
  public List<string> Female { get; set; } = [];
  public List<string> Male { get; set; } = [];
  public List<string> Unisex { get; set; } = [];
  public List<NameCategory> Custom { get; set; } = [];
}

public record NameCategory
{
  public string Category { get; set; } = string.Empty;
  public List<string> Names { get; set; } = [];
}

public record Speeds
{
  public int Walk { get; set; }
  public int Climb { get; set; }
  public int Swim { get; set; }
  public int Fly { get; set; }
  public int Hover { get; set; }
  public int Burrow { get; set; }
}

public record Size
{
  public SizeCategory Category { get; set; }
  public string? Roll { get; set; }
}

public record Weight
{
  public string? Starved { get; set; }
  public string? Skinny { get; set; }
  public string? Normal { get; set; }
  public string? Overweight { get; set; }
  public string? Obese { get; set; }
}

public record Ages
{
  public int? Adolescent { get; set; }
  public int? Adult { get; set; }
  public int? Mature { get; set; }
  public int? Venerable { get; set; }
}
