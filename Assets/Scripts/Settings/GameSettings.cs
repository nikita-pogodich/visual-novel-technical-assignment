using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class GameSettings : IGameSettings
    {
        [field: SerializeField]
        public string SavesFolderName { get; private set; } = "Saves";

        [field: SerializeField]
        public string AutoSaveSlotName { get; private set; } = "Slot_1";
    }
}