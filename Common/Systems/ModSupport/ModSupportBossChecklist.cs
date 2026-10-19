using ChangedSpecialMod.Content.Items.Summons;
using ChangedSpecialMod.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportBossChecklist : ModSupportData
    {
        public ModSupportBossChecklist(Mod mod, string modName) : base(mod, modName) { }

        private static readonly Dictionary<string, float> BossChecklistProgressionValues = new()
        {
            { "WhiteTail", 1.5f },
            { "WolfKing", 2.5f },
            { "Behemoth", 5.5f }
            //{ "Shark", ??? },
            //{ "SquidDog", ??? },
        };

        public void AddBoss(Mod hostMod, string name, float difficulty, Func<bool> downed, object npcTypes, Dictionary<string, object> extraInfo)
        {
            mod.Call("LogBoss", hostMod, name, difficulty, downed, npcTypes, extraInfo);
        }

        public static LocalizedText GetText(string key)
        {
            return Language.GetOrRegister("Mods.ChangedSpecialMod." + key);
        }

        private static LocalizedText GetSpawnInfo(string entryName)
        {
            return GetText($"BossChecklistIntegration.{entryName}.SpawnInfo");
        }

        private static LocalizedText GetDespawnMessage(string entryName)
        {
            return GetText($"BossChecklistIntegration.{entryName}.DespawnMessage");
        }

        public override void PostSetupContent(log4net.ILog log)
        {
            var changedMod = ChangedSpecialMod.Instance;

            if (changedMod == null || mod == null)
                return;

            // White Tail
            {
                string entryName = "WhiteTail";
                BossChecklistProgressionValues.TryGetValue(entryName, out float order);
                int type = ModContent.NPCType<WhiteTail>();
                List<int> collection = new List<int>();
                Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) =>
                {
                    Texture2D texture = changedMod.Assets.Request<Texture2D>("Content/NPCs/WhiteTail_Portrait").Value;
                    Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                    sb.Draw(texture, centered, color);
                };
                AddBoss(changedMod, entryName, order, () => DownedBossSystem.DownedWhiteTail, type, new Dictionary<string, object>()
                {
                    ["spawnInfo"] = GetSpawnInfo(entryName),
                    ["despawnMessage"] = GetDespawnMessage(entryName),
                    ["spawnItems"] = ModContent.ItemType<SummonWhiteTail>(),
                    ["collectibles"] = collection,
                    ["overrideHeadTextures"] = "ChangedSpecialMod/Content/NPCs/WhiteTail_Head_Boss",
                    ["customPortrait"] = portrait
                });
            }

            // Wolf King
            {
                string entryName = "WolfKing";
                BossChecklistProgressionValues.TryGetValue(entryName, out float order);
                int type = ModContent.NPCType<WolfKing>();
                List<int> collection = new List<int>();
                Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) =>
                {
                    Texture2D texture = changedMod.Assets.Request<Texture2D>("Content/NPCs/WolfKing_Portrait").Value;
                    Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                    sb.Draw(texture, centered, color);
                };
                AddBoss(changedMod, entryName, order, () => DownedBossSystem.DownedWolfKing, type, new Dictionary<string, object>()
                {
                    ["spawnInfo"] = GetSpawnInfo(entryName),
                    ["despawnMessage"] = GetDespawnMessage(entryName),
                    ["spawnItems"] = ModContent.ItemType<SummonWolfKing>(),
                    ["collectibles"] = collection,
                    ["overrideHeadTextures"] = "ChangedSpecialMod/Content/NPCs/WolfKing_Head_Boss",
                    ["customPortrait"] = portrait
                });
            }

            // Behemoth
            {
                string entryName = "Behemoth";
                BossChecklistProgressionValues.TryGetValue(entryName, out float order);
                int type = ModContent.NPCType<Behemoth>();
                List<int> collection = new List<int>();
                Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) =>
                {
                    Texture2D texture = changedMod.Assets.Request<Texture2D>("Content/NPCs/Behemoth_Portrait").Value;
                    Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                    sb.Draw(texture, centered, color);
                };
                AddBoss(changedMod, entryName, order, () => DownedBossSystem.DownedBehemoth, type, new Dictionary<string, object>()
                {
                    ["spawnInfo"] = GetSpawnInfo(entryName),
                    ["despawnMessage"] = GetDespawnMessage(entryName),
                    ["spawnItems"] = ModContent.ItemType<SummonBehemoth>(),
                    ["collectibles"] = collection,
                    ["overrideHeadTextures"] = "ChangedSpecialMod/Content/NPCs/Behemoth_Head_Boss",
                    ["customPortrait"] = portrait
                });
            }
        }
    }
}
