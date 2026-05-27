using System.Text.RegularExpressions;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;

namespace ChangedSpecialMod.Content.NPCs
{
    public class HappinessObject
    {
        public int Happiness { get; private set; }
        public bool ContainsAny { get; private set; }

        private readonly string basePath;

        public HappinessObject(
            NPC npc,
            string npcName,
            int hateNPC, int dislikeNPC, int likeNPC, int loveNPC,
            string hateBiomeKey, string dislikeBiomeKey, string likeBiomeKey, string loveBiomeKey)
        {
            basePath = $"Mods.ChangedSpecialMod.NPCs.{npcName}.TownNPCMood";
            string npcText = Main.npcChatText;
            string hateBiome = GetBiomeName(hateBiomeKey);
            string dislikeBiome = GetBiomeName(dislikeBiomeKey);
            string likeBiome = GetBiomeName(likeBiomeKey);
            string loveBiome = GetBiomeName(loveBiomeKey);

            ProcessCategory(npcText, -2,
                GetText("NoHome"),
                GetText("FarFromHome"),
                GetText("HateCrowded"),
                GetNPCText(hateNPC, "HateNPC"),
                GetBiomeText(hateBiome, "HateBiome"));

            ProcessCategory(npcText, -1,
                GetText("DislikeCrowded"),
                GetNPCText(dislikeNPC, "DislikeNPC"),
                GetBiomeText(dislikeBiome, "DislikeBiome"));

            ProcessCategory(npcText, +1,
                GetText("Content"),
                GetNPCText(likeNPC, "LikeNPC"),
                GetBiomeText(likeBiome, "LikeBiome"));

            ProcessCategory(npcText, +2,
                GetText("LoveSpace"),
                GetNPCText(loveNPC, "LoveNPC"),
                GetBiomeText(loveBiome, "LoveBiome"));
        }

        public string GetEmotion()
        {
            if (Happiness > 3)
                return "Love";
            if (Happiness > 0)
                return "Happy";
            if (Happiness < -3)
                return "Evil";
            if (Happiness < 0)
                return "Angry";
            return "Neutral";
        }

        private void ProcessCategory(string npcText, int value, params string[] texts)
        {
            foreach (var text in texts)
            {
                if (string.IsNullOrEmpty(text))
                    continue;

                if (npcText.Contains(text))
                {
                    ContainsAny = true;
                    Happiness += value;
                }
            }
        }

        private string GetText(string key)
        {
            return Language.GetTextValue($"{basePath}.{key}");
        }

        private string GetNPCText(int npcType, string key)
        {
            if (npcType == -1)
                return null;

            int npcIndex = NPC.FindFirstNPC(npcType);
            if (npcIndex == -1)
                return null;

            string text = GetText(key);
            return ReplacePlaceholder(text, Main.npc[npcIndex].FullName);
        }

        private string GetBiomeText(string biomeName, string key)
        {
            if (string.IsNullOrEmpty(biomeName))
                return null;

            string text = GetText(key);
            return ReplacePlaceholder(text, biomeName);
        }

        private static string GetBiomeName(string key)
        {
            return key == null ? null : ShopHelper.BiomeNameByKey(key);
        }

        private static string ReplacePlaceholder(string text, string value)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            return Regex.Replace(text, @"\{.*?\}", value);
        }
    }
}
