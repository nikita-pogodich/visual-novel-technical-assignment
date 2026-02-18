using System.Collections.Generic;

namespace Settings
{
    public interface ICharacterSettings
    {
        List<CharacterSetting> Characters { get; }
    }
}