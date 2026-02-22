using System;
using System.Collections.Generic;

namespace Features.Narrative
{
    public class TagEffects
    {
        public string SpeakerKey { get; private set; }
        public string BackgroundKey { get; private set; }
        public string EndingKey { get; private set; }
        public WorldMode? Mode { get; private set; }

        public TagEffects(string speakerKey, string backgroundKey, string endingKey, WorldMode? mode)
        {
            SpeakerKey = speakerKey;
            BackgroundKey = backgroundKey;
            EndingKey = endingKey;
            Mode = mode;
        }
    }

    /// <summary>
    /// Parses Ink tags authored as key/value pairs, e.g.:
    ///   # speaker:alice
    ///   # bg:tavern
    ///   # mode:character_select
    /// Accepts "key:value" and "key=value".
    /// </summary>
    public static class InkTagParser
    {
        public static TagEffects Parse(IReadOnlyList<string> tags)
        {
            string speaker = null;
            string bg = null;
            string ending = null;
            WorldMode? mode = null;

            if (tags == null)
            {
                return new TagEffects(null, null, null, null);
            }

            for (int i = 0; i < tags.Count; i++)
            {
                string raw = tags[i];
                if (raw == null) continue;

                string tag = raw.Trim();
                if (tag.Length == 0) continue;

                int idx = tag.IndexOf(':');
                if (idx < 0) idx = tag.IndexOf('=');
                if (idx <= 0 || idx >= tag.Length - 1) continue;

                string key = tag.Substring(0, idx).Trim().ToLowerInvariant();
                string val = tag.Substring(idx + 1).Trim();

                if (key == "speaker" || key == "char" || key == "character")
                {
                    speaker = val;
                }
                else if (key == "bg" || key == "background")
                {
                    bg = val;
                }
                else if (key == "ending" || key == "end")
                {
                    ending = val;
                }
                else if (key == "mode")
                {
                    mode = ParseMode(val);
                }
            }

            return new TagEffects(speaker, bg, ending, mode);
        }

        private static WorldMode? ParseMode(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            string v = value.Trim().ToLowerInvariant();

            if (v == "characterselect" || v == "character_select" || v == "character-select" || v == "select" || v == "hub")
            {
                return WorldMode.CharacterSelect;
            }

            if (v == "inconversation" || v == "in_conversation" || v == "in-conversation" || v == "conversation" || v == "dialogue")
            {
                return WorldMode.InConversation;
            }

            if (v == "ending" || v == "end")
            {
                return WorldMode.Ending;
            }

            if (Enum.TryParse(value, ignoreCase: true, result: out WorldMode parsed))
            {
                return parsed;
            }

            return null;
        }
    }
}