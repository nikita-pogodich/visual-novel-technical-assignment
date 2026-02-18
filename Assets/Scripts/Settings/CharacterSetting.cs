using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class CharacterSetting
    {
        public string CharacterId;
        public string CharacterVisualKey;
        public string DisplayName;
        public string InkEntryPath;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}