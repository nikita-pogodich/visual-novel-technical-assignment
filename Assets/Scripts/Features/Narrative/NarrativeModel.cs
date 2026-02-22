using System;
using System.Collections.Generic;

namespace Features.Narrative
{
    public class NarrativeModel
    {
        private readonly INarrativeEngine _narrativeEngine;

        private readonly Dictionary<string, CharacterData> _talkTargetsById = new();
        private readonly List<ChoiceData> _choices = new();

        public WorldMode Mode { get; private set; }
        public string ActiveCharacter { get; private set; }
        public string CurrentSpeakerKey { get; private set; }
        public string CurrentBackgroundKey { get; private set; }
        public string CurrentEndingKey { get; private set; }
        public string CurrentLineText { get; private set; }
        public string NarrativeStateJson { get; private set; }

        public NarrativeModel(IEnumerable<CharacterData> initialTargets, INarrativeEngine narrativeEngine)
        {
            _narrativeEngine = narrativeEngine;

            Mode = WorldMode.CharacterSelect;
            foreach (CharacterData character in initialTargets)
            {
                if (character != null)
                {
                    _talkTargetsById.Add(character.Id, character);
                }
            }
        }

        public void StartNewStory(string storyJson)
        {
            _narrativeEngine.LoadStory(storyJson);
            _narrativeEngine.ResetToStart();

            StepUntilLineOrChoice();
        }

        public DialogueSnapshot GetSnapshot()
        {
            return new DialogueSnapshot(
                Mode,
                ActiveCharacter,
                CurrentSpeakerKey,
                CurrentBackgroundKey,
                CurrentEndingKey,
                CurrentLineText,
                _choices,
                _talkTargetsById
            );
        }

        public void StartConversation(string characterId)
        {
            if (_talkTargetsById.TryGetValue(characterId, out CharacterData target) == false)
            {
                throw new InvalidOperationException("Character not found: " + characterId);
            }

            BeginConversation(characterId);

            _narrativeEngine.ChoosePath(target.InkEntryPath);
            StepUntilLineOrChoice();
            StepUntilLineOrChoice();
        }

        public void Continue()
        {
            StepUntilLineOrChoice();
        }

        public void Choose(int choiceIndex)
        {
            _narrativeEngine.ChooseChoiceIndex(choiceIndex);
            StepUntilLineOrChoice();
        }

        private void EnterCharacterSelect()
        {
            Mode = WorldMode.CharacterSelect;
            ActiveCharacter = null;

            _choices.Clear();
        }

        private void StepUntilLineOrChoice()
        {
            ApplyChoices(null);

            IReadOnlyList<NarrativeChoice> choices = _narrativeEngine.CurrentChoices;
            if (choices != null && choices.Count > 0)
            {
                ApplyChoicesFromNarrativeEngine(choices);

                if (HasCharacterSelectionChoices(choices))
                {
                    SetMode(WorldMode.CharacterSelect);
                }

                return;
            }

            if (_narrativeEngine.CanContinue)
            {
                NarrativeLine line = _narrativeEngine.Continue();
                if (line == null)
                {
                    return;
                }

                TagEffects effects = InkTagParser.Parse(line.Tags);
                ApplyTagEffects(effects.SpeakerKey, effects.BackgroundKey, effects.EndingKey);
                ApplyModeFromTags(effects.Mode);
                ApplyLine(line.Text);

                choices = _narrativeEngine.CurrentChoices;
                if (choices != null && choices.Count > 0)
                {
                    ApplyChoicesFromNarrativeEngine(choices);

                    if (HasCharacterSelectionChoices(choices))
                    {
                        SetMode(WorldMode.CharacterSelect);
                    }
                }

                return;
            }

            EnterCharacterSelect();
        }

        private void ApplyChoicesFromNarrativeEngine(IReadOnlyList<NarrativeChoice> choicesFromNarrativeEngine)
        {
            var choices = new List<ChoiceData>(choicesFromNarrativeEngine.Count);
            for (int i = 0; i < choicesFromNarrativeEngine.Count; i++)
            {
                NarrativeChoice choice = choicesFromNarrativeEngine[i];
                choices.Add(new ChoiceData(choice.Index, choice.Text));
            }

            ApplyChoices(choices);
        }

        public void BeginConversation(string characterId)
        {
            Mode = WorldMode.InConversation;
            ActiveCharacter = characterId;
            CurrentLineText = null;
            _choices.Clear();
        }

        public void ApplyLine(string lineText)
        {
            CurrentLineText = lineText;
        }

        public void ApplyChoices(IEnumerable<ChoiceData> choices)
        {
            _choices.Clear();
            if (choices == null)
            {
                return;
            }

            foreach (ChoiceData choice in choices)
            {
                if (choice != null)
                {
                    _choices.Add(choice);
                }
            }
        }

        public void ApplyTagEffects(string speakerKey, string backgroundKey, string endingKey)
        {
            if (string.IsNullOrWhiteSpace(speakerKey) == false)
            {
                CurrentSpeakerKey = speakerKey;
            }

            if (string.IsNullOrWhiteSpace(backgroundKey) == false)
            {
                CurrentBackgroundKey = backgroundKey;
            }

            if (string.IsNullOrWhiteSpace(endingKey) == false)
            {
                CurrentEndingKey = endingKey;
            }
        }

        public void CaptureNarrativeState(string stateJson)
        {
            NarrativeStateJson = stateJson;
        }

        private void ApplyModeFromTags(WorldMode? modeFromTags)
        {
            if (modeFromTags.HasValue)
            {
                SetMode(modeFromTags.Value);
            }
        }

        private void SetMode(WorldMode mode)
        {
            Mode = mode;

            if (Mode == WorldMode.CharacterSelect)
            {
                ActiveCharacter = null;
            }
        }

        private bool HasCharacterSelectionChoices(IReadOnlyList<NarrativeChoice> engineChoices)
        {
            if (engineChoices == null || engineChoices.Count == 0 ||
                engineChoices.Count != _talkTargetsById.Count)
            {
                return false;
            }

            for (int i = 0; i < engineChoices.Count; i++)
            {
                NarrativeChoice choice = engineChoices[i];
                if (choice == null || string.IsNullOrWhiteSpace(choice.Text))
                {
                    return false;
                }

                bool isCharacterMatched = false;
                foreach (KeyValuePair<string, CharacterData> kv in _talkTargetsById)
                {
                    CharacterData characterData = kv.Value;
                    if (characterData == null)
                    {
                        continue;
                    }

                    //TODO Find better solution to detect character selection choices
                    if (ContainsIgnoreCase(choice.Text, characterData.DisplayName) ||
                        ContainsIgnoreCase(choice.Text, characterData.Id))
                    {
                        isCharacterMatched = true;
                        break;
                    }
                }

                if (isCharacterMatched == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ContainsIgnoreCase(string text, string fragment)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(fragment))
                return false;

            return text.IndexOf(fragment, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}