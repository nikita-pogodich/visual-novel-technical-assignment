using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class CharacterSettings : ICharacterSettings
    {
        [field: SerializeField]
        public List<CharacterSetting> Characters { get; private set; }
    }
}