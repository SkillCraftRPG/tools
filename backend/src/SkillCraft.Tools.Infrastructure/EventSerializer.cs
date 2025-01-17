using SkillCraft.Tools.Infrastructure.Converters;

namespace SkillCraft.Tools.Infrastructure;

internal class EventSerializer : Logitar.EventSourcing.Infrastructure.EventSerializer
{
  protected override void RegisterConverters()
  {
    base.RegisterConverters();

    SerializerOptions.Converters.Add(new AspectIdConverter());
    SerializerOptions.Converters.Add(new CasteIdConverter());
    SerializerOptions.Converters.Add(new CustomizationIdConverter());
    SerializerOptions.Converters.Add(new DescriptionConverter());
    SerializerOptions.Converters.Add(new DisplayNameConverter());
    SerializerOptions.Converters.Add(new LanguageIdConverter());
    SerializerOptions.Converters.Add(new LineageIdConverter());
    SerializerOptions.Converters.Add(new NatureIdConverter());
    SerializerOptions.Converters.Add(new OtherOptionConverter());
    SerializerOptions.Converters.Add(new OtherRequirementConverter());
    SerializerOptions.Converters.Add(new ScriptConverter());
    SerializerOptions.Converters.Add(new RollConverter());
    SerializerOptions.Converters.Add(new SlugConverter());
    SerializerOptions.Converters.Add(new SpecializationIdConverter());
    SerializerOptions.Converters.Add(new TalentIdConverter());
    SerializerOptions.Converters.Add(new TypicalSpeakersConverter());
    SerializerOptions.Converters.Add(new WealthMultiplierConverter());
  }
}
