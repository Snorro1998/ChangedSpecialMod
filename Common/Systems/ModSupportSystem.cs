using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Items.Summons;
using ChangedSpecialMod.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class ModSupportSystem : ModSystem
    {
        public static ChangedSpecialMod changedMod = null;
        public static Mod modBossChecklist = null;
        public static Mod modFargosMutant = null;
        public static Mod modMusicDisplay = null;
        public static Mod modThorium = null;
        public static Mod modCalamity = null;
        public static Mod modSpirit = null;
        public static Mod modSpiritReforged = null;

        public override void Load()
        {
            changedMod = null;
            changedMod = ChangedSpecialMod.Instance;

            modBossChecklist = null;
            ModLoader.TryGetMod("BossChecklist", out modBossChecklist);
            modFargosMutant = null;
            ModLoader.TryGetMod("Fargowiltas", out modFargosMutant);
            modMusicDisplay = null;
            ModLoader.TryGetMod("MusicDisplay", out modMusicDisplay);
            modThorium = null;
            ModLoader.TryGetMod("ThoriumMod", out modThorium);
            modCalamity = null;
            ModLoader.TryGetMod("CalamityMod", out modCalamity);
            modSpirit = null;
            ModLoader.TryGetMod("SpiritMod", out modSpirit);
            modSpiritReforged = null;
            ModLoader.TryGetMod("SpiritReforged", out modSpiritReforged);
        }

        public override void Unload()
        {
            modBossChecklist = null;
            modMusicDisplay = null;
            modFargosMutant = null;
            modMusicDisplay = null;
            modThorium = null;
            modCalamity = null;
            modSpirit = null;
            modSpiritReforged = null;
        }

        public override void PostAddRecipes()
        {

        }

        public override void PostSetupContent()
        {
            SetupBossChecklist();
            SetupFargosMutant();
            SetupMusicDisplay();
        }

        private void SetupFargosMutant()
        {
            if (modFargosMutant is null)
                return;

            void AddToMutantShop(string bossName, string summonItemName, Func<bool> downed, int price)
            {
                BossChecklistProgressionValues.TryGetValue(bossName, out float order);
                modFargosMutant.Call("AddSummon", order, "ChangedSpecialMod", summonItemName, downed, price);
            }

            AddToMutantShop("WhiteTail", "SummonWhiteTail", () => DownedBossSystem.DownedWhiteTail, Item.buyPrice(gold: 2));
            AddToMutantShop("WolfKing", "SummonWolfKing", () => DownedBossSystem.DownedWolfKing, Item.buyPrice(gold: 2));
            AddToMutantShop("Behemoth", "SummonBehemoth", () => DownedBossSystem.DownedBehemoth, Item.buyPrice(gold: 2));
        }

        /*
        1.0 = King Slime
        2.0 = Eye of Cthulhu
        3.0 = Eater of Worlds  Brain of Cthulhu
        4.0 = Queen Bee
        5.0 = Skeletron
        6.0 = Deerclops
        7.0 = Wall of Flesh
        8.0 = Queen Slime
        9.0 = The Twins
        10.0 = The Destroyer
        11.0 = Skeletron Prime
        12.0 = Plantera
        13.0 = Golem
        14.0 = Duke Fishron
        15.0 = Empress of Light
        16.0 = Betsy
        17.0 = Lunatic Cultist
        18.0 = Moon Lord
         */

        private static readonly Dictionary<string, float> BossChecklistProgressionValues = new()
        {
            { "WhiteTail", 1.5f },
            { "WolfKing", 2.5f },
            { "Behemoth", 5.5f }
            //{ "Shark", ??? },
            //{ "SquidDog", ??? },
        };

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

        public static void SetupBossChecklist()
        {
            if (changedMod == null || modBossChecklist == null)
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

        public static void AddBoss(Mod hostMod, string name, float difficulty, Func<bool> downed, object npcTypes, Dictionary<string, object> extraInfo)
        {
            modBossChecklist.Call("LogBoss", hostMod, name, difficulty, downed, npcTypes, extraInfo);
        }

        public static void SetupMusicDisplay()
        {
            if (changedMod == null || modMusicDisplay == null)
                return;

            LocalizedText modName = Language.GetText("Mods.ChangedSpecialMod.MusicDisplay.ModName");
            LocalizedText author = Language.GetText("Mods.ChangedSpecialMod.MusicDisplay.Authors.Shizi");

            // Is displayed like this. The number is the parameter index
            // 4: Current Music
            // 1: Song name
            // 2: Artist
            // 3: Mod name
            var defaultColors = new Color[] { Color.White, new Color(230, 230, 230), new Color(180, 180, 180), new Color(120, 120, 120) };
            var partyColors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow };

            // Normal
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicBlackLatexZone, "MusicBlackLatexZone", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicBlackLatexZone2, "MusicBlackLatexZone2", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicCrystalZone, "MusicCrystalZone", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicWhiteLatexZone, "MusicWhiteLatexZone", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicLibrary, "MusicLibrary", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicLabSlow, "MusicLabSlow", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicLab, "MusicLab", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicVents, "MusicVents", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicPuro, "MusicPuro", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicGreenhouse, "MusicGreenhouse", defaultColors);
            
            // Party
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicHappyBirthday, "MusicHappyBirthday", partyColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicPuroDance, "MusicPuroDance", partyColors);
            
            // Drunk
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicRun, "MusicRun", defaultColors);
            
            // Bosses
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicWhiteTailChase2, "MusicWhiteTailChase2", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicWolfKing, "MusicWolfKing", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicBehemoth, "MusicBehemoth", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicShark, "MusicShark", defaultColors);
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.MusicSquidDog, "MusicSquidDog", defaultColors);
        }

        private static void MusicDisplayAddTrack(Mod hostMod, LocalizedText modName, LocalizedText author, string musicPath, string musicName, Color[] colors)
        {
            LocalizedText displayName = Language.GetText($"Mods.ChangedSpecialMod.MusicDisplay.Music.{musicName}");
            modMusicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot(hostMod, musicPath), displayName, author, modName, null, colors);
        }
    }
}
