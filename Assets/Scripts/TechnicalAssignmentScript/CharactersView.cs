using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechnicalAssignmentScript
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class CharactersView : MonoBehaviour
    {
        public enum RefreshMode
        {
            EveryNFrames,
            IntervalSeconds
        }

        [Header("Data (drag Character components here, or register at runtime)")]
        [SerializeField]
        private List<Character> _characters = new();

        [Header("Refresh")]
        [SerializeField]
        private RefreshMode _refreshMode = RefreshMode.IntervalSeconds;

        [SerializeField, Min(1)]
        private int _refreshEveryNFrames = 10;

        [SerializeField, Min(0f)]
        private float _refreshIntervalSeconds = 0.25f;

        [Header("Debug")]
        [SerializeField]
        private bool _logToConsole = false;

        private Text _text;
        private float _nextRefreshTime;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _nextRefreshTime = Time.unscaledTime;
            Refresh();
        }

        private void Update()
        {
            if (ShouldRefresh() == false)
            {
                return;
            }

            Refresh();
        }

        private bool ShouldRefresh()
        {
            if (_refreshMode == RefreshMode.EveryNFrames)
            {
                return Time.frameCount % _refreshEveryNFrames == 0;
            }

            float now = Time.unscaledTime;
            if (now < _nextRefreshTime)
            {
                return false;
            }

            _nextRefreshTime = now + _refreshIntervalSeconds;
            return true;
        }

        private void Refresh()
        {
            int charactersCount = 0;
            float sum = 0f;

            for (int i = 0; i < _characters.Count; i++)
            {
                Character character = _characters[i];
                if (character == null)
                {
                    continue;
                }

                charactersCount++;
                sum += character.Value;
            }

            float average = charactersCount > 0 ? sum / charactersCount : 0f;

            string message = $"Characters: {charactersCount}  Average value: {average:0.##}";
            _text.text = message;

            if (_logToConsole)
            {
                Debug.Log(message, this);
            }
        }

        public void Register(Character character)
        {
            if (character == null)
            {
                return;
            }

            if (_characters.Contains(character) == false)
            {
                _characters.Add(character);
            }
        }

        public void Unregister(Character character)
        {
            if (character == null)
            {
                return;
            }

            _characters.Remove(character);
        }
    }
}