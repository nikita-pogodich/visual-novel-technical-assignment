using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class ViewNames : IViewNames
    {
        [field: SerializeField]
        public string MainMenuWindow { get; private set; } = "MainMenuWindow";

        [field: SerializeField]
        public string DialogueWindow { get; private set; } = "DialogueWindow";

        [field: SerializeField]
        public string ChoiceWindow { get; private set; } = "ChoiceWindow";

        [field: SerializeField]
        public string ChoiceListItemView { get; private set; } = "ChoiceListItemView";

        [field: SerializeField]
        public string CharacterView { get; private set; } = "CharacterView";

        [field: SerializeField]
        public string CharacterSelectionView { get; private set; } = "CharacterSelectionView";
    }
}