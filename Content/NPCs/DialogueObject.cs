using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria.Localization;

namespace ChangedSpecialMod.Content.NPCs
{
    public class DialogueObject
    {
        private readonly string basePath;
        private string CharacterName;
        private List<DialogueElement> Dialogue = new List<DialogueElement>();
        private Dictionary<string, string> KeyWords = new Dictionary<string, string>();

        public DialogueObject(string characterName, List<DialogueElement> dialogue)
        {
            basePath = "Mods.ChangedSpecialMod.NPCs.Dialogue";
            CharacterName = characterName;
            Dialogue = dialogue ?? new List<DialogueElement>();
            ChangedUtils.Shuffle(Dialogue);
        }

        // Keywords can often change so we don't set it during creation. For example when an NPC dies it should not pick an option with his name anymore
        public (string, string) GetDialogue(Dictionary<string, string> keyWords)
        {
            if (keyWords != null) KeyWords = keyWords;
            var length = Dialogue.Count;
            if (length == 0) return (null, null);

            // Iterate all dialogue options
            for (var i = 0; i < length; i++)
            {
                var strKey = Dialogue.First();
                var str = Language.GetTextValue($"{basePath}.{CharacterName}.{strKey.Dialogue}");

                Dialogue.RemoveAt(0);
                Dialogue.Add(strKey);

                // Get all variables from the string which are formatted like {0}, {1}, {2}
                var matches = Regex.Matches(str, @"\{([^}]*)\}");
                List<string> keys = matches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();
                if (keys != null && keys.Count > 0)
                {
                    var failed = false;
                    foreach (var key in keys)
                    {
                        if (!KeyWords.ContainsKey(key))
                        {
                            failed = true;
                            break;
                        }
                        str = str.Replace("{" + key + "}", KeyWords[key]);
                    }
                    if (!failed) return (str, strKey.Emotion);
                }
                else
                {
                    return (str, strKey.Emotion);
                }
            }
            return (null, null);
        }
    }

}
