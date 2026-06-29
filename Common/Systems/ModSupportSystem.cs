using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Items.Summons;
using ChangedSpecialMod.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static ChangedSpecialMod.Common.Systems.ExternalModItemSystem;

namespace ChangedSpecialMod.Common.Systems
{
    public class ModSupportSystem : ModSystem
    {
        // TODO Coralite
        // Add logic so the white latex lab won't override its ice dragon thing
        // The lab generates before it, but Coralite only replaces snow and ice blocks
        // Might check the other structures as wel

        // TODO Calamity
        // If the lab is very close to the sulphuric sea, Calamity can override its walls.
        // Due to this, you can end up with a lab without the slime walls required for the wolf king and behemoth fights

        public static ChangedSpecialMod changedMod = null;

        // Informational
        public static Mod modBossChecklist = null;
        public static Mod modMusicDisplay = null;
        public static Mod modCensus = null;
        public static Mod modWikiThis = null;

        // Shops
        public static Mod modFargosMutant = null;
        
        // Content mods
        public static Mod modThorium = null;
        public static Mod modCalamity = null;
        public static Mod modSpirit = null;
        public static Mod modSpiritReforged = null;
        public static Mod modCoralite = null;

        // Other
        public static Mod modBoulderBackport = null;
        //public static Mod modDialogueTweak = null;

        // The min and max indecis for the extra title message added in the hjson
        private static int indexTitleMessageMin = 1;
        private static int indexTitleMessageMax = 5;

        public override void Load()
        {
            changedMod = ChangedSpecialMod.Instance;

            // Informational
            modBossChecklist = GetMod("BossChecklist");
            modMusicDisplay = GetMod("MusicDisplay");
            modCensus = GetMod("Census");
            modWikiThis = GetMod("Wikithis");

            // Shops
            modFargosMutant = GetMod("Fargowiltas");

            // Content mods
            modThorium = GetMod("ThoriumMod");
            modCalamity = GetMod("CalamityMod");
            modSpirit = GetMod("SpiritMod");
            modSpiritReforged = GetMod("SpiritReforged");
            modCoralite = GetMod("Coralite");

            // Other
            modBoulderBackport = GetMod("BoulderBackport");
            //modDialogueTweak = GetMod("DialogueTweak");
        }

        private static Mod GetMod(string name)
        {
            ModLoader.TryGetMod(name, out Mod mod);
            return mod;
        }

        public override void Unload()
        {
            changedMod = null;

            // Informational
            modBossChecklist = null;
            modMusicDisplay = null;
            modCensus = null;
            modWikiThis = null;

            // Shops
            modFargosMutant = null;

            // Content mods
            modThorium = null;
            modCalamity = null;
            modSpirit = null;
            modSpiritReforged = null;
            modCoralite = null;

            // Other
            modBoulderBackport = null;
            //modDialogueTweak = null;

            RemoveExtraTitles();
        }

        public override void PostAddRecipes()
        {

        }

        public override void PostSetupContent()
        {
            SetupBossChecklist();
            SetupFargosMutant();
            SetupMusicDisplay();
            SetupCensus();
            SetupWikiThis();
            SetupExtraTitles();
            TryUpdateTitle();
        }

        private void TryUpdateTitle()
        {
            MethodInfo setTitleMethod = typeof(Main).GetMethod("SetTitle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (setTitleMethod != null)
                setTitleMethod.Invoke(Main.instance, null);
        }

        private void SetupExtraTitles()
        {
            for (int i = indexTitleMessageMin; i <= indexTitleMessageMax; i++)
            {
                if (!LanguageManager.Instance.GetKeysInCategory("GameTitle").Contains($"Changed{i.ToString()}"))
                    LanguageManager.Instance.GetKeysInCategory("GameTitle").Add($"Changed{i.ToString()}");
            }
        }

        private void RemoveExtraTitles()
        {
            for (int i = indexTitleMessageMin; i <= indexTitleMessageMax; i++)
            {
                if (LanguageManager.Instance.GetKeysInCategory("GameTitle").Contains($"Changed{i.ToString()}"))
                    LanguageManager.Instance.GetKeysInCategory("GameTitle").Remove($"Changed{i.ToString()}");
            }
        }

        private void SetupCensus()
        {
            if (changedMod == null || modCensus == null)
                return;

            modCensus.Call("TownNPCCondition", ModContent.NPCType<Puro>());
            modCensus.Call("TownNPCCondition", ModContent.NPCType<Prototype>());
            modCensus.Call("TownNPCCondition", ModContent.NPCType<Scientist>());
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

        private static void SetupWikiThis()
        {
            // Wikithis is a clientside mod
            if (Main.dedServ)
                return;

            if (modWikiThis == null)
                return;

            var wikiURL = "https://changedterraria.wiki.gg/wiki/{}";

            modWikiThis.Call("AddModURL", changedMod, wikiURL);
            modWikiThis.Call(0, changedMod, wikiURL);
            //wiki.Call("AddWikiTexture", calamity, Request<Texture2D>("CalamityMod/ModSupport/WikiThisIcon"));
            //wiki.Call(3, calamity, Request<Texture2D>("CalamityMod/ModSupport/WikiThisIcon"));

            // Clear up name conflicts
            void ItemRedirect(int item, string pageName) => modWikiThis.Call(1, item, "https://changedterraria.wiki.gg/wiki/" + pageName);
            void EnemyRedirect(int item, string pageName) => modWikiThis.Call(2, item, "https://changedterraria.wiki.gg/wiki/" + pageName);

            EnemyRedirect(ModContent.NPCType<Puro>(), "Puro");
            EnemyRedirect(ModContent.NPCType<Scientist>(), "Dr_K");
            /*
            // Items
            ItemRedirect(ItemType<PineapplePet>(), "Pineapple (calamity)");
            ItemRedirect(ItemType<TrashmanTrashcan>(), "Trash Can (pet)");
            // Lore items
            ItemRedirect(ItemType<LoreAstralInfection>(), loreItemPage);
            ItemRedirect(ItemType<LoreAbyss>(), loreItemPage);
            ItemRedirect(ItemType<LoreAquaticScourge>(), loreItemPage);
            ItemRedirect(ItemType<LoreArchmage>(), loreItemPage);
            ItemRedirect(ItemType<LoreAstrumAureus>(), loreItemPage);
            ItemRedirect(ItemType<LoreAstrumDeus>(), loreItemPage);
            ItemRedirect(ItemType<LoreAwakening>(), loreItemPage);
            ItemRedirect(ItemType<LoreAzafure>(), loreItemPage);
            ItemRedirect(ItemType<LoreBloodMoon>(), loreItemPage);
            ItemRedirect(ItemType<LoreBrainofCthulhu>(), loreItemPage);
            ItemRedirect(ItemType<LoreBrimstoneElemental>(), loreItemPage);
            ItemRedirect(ItemType<LoreCalamitas>(), loreItemPage);
            ItemRedirect(ItemType<LoreCalamitasClone>(), loreItemPage);
            ItemRedirect(ItemType<LoreCeaselessVoid>(), loreItemPage);
            ItemRedirect(ItemType<LoreCorruption>(), loreItemPage);
            ItemRedirect(ItemType<LoreCrabulon>(), loreItemPage);
            ItemRedirect(ItemType<LoreCrimson>(), loreItemPage);
            ItemRedirect(ItemType<LoreCynosure>(), loreItemPage);
            ItemRedirect(ItemType<LoreDesertScourge>(), loreItemPage);
            ItemRedirect(ItemType<LoreDestroyer>(), loreItemPage);
            ItemRedirect(ItemType<LoreDevourerofGods>(), loreItemPage);
            ItemRedirect(ItemType<LoreDragonfolly>(), loreItemPage);
            ItemRedirect(ItemType<LoreDukeFishron>(), loreItemPage);
            ItemRedirect(ItemType<LoreEaterofWorlds>(), loreItemPage);
            ItemRedirect(ItemType<LoreEmpressofLight>(), loreItemPage);
            ItemRedirect(ItemType<LoreExoMechs>(), loreItemPage);
            ItemRedirect(ItemType<LoreEyeofCthulhu>(), loreItemPage);
            ItemRedirect(ItemType<LoreGolem>(), loreItemPage);
            ItemRedirect(ItemType<LoreHiveMind>(), loreItemPage);
            ItemRedirect(ItemType<LoreKingSlime>(), loreItemPage);
            ItemRedirect(ItemType<LoreLeviathanAnahita>(), loreItemPage);
            ItemRedirect(ItemType<LoreMechs>(), loreItemPage);
            ItemRedirect(ItemType<LoreOldDuke>(), loreItemPage);
            ItemRedirect(ItemType<LorePerforators>(), loreItemPage);
            ItemRedirect(ItemType<LorePlaguebringerGoliath>(), loreItemPage);
            ItemRedirect(ItemType<LorePlantera>(), loreItemPage);
            ItemRedirect(ItemType<LorePolterghast>(), loreItemPage);
            ItemRedirect(ItemType<LorePrelude>(), loreItemPage);
            ItemRedirect(ItemType<LoreProfanedGuardians>(), loreItemPage);
            ItemRedirect(ItemType<LoreProvidence>(), loreItemPage);
            ItemRedirect(ItemType<LoreQueenBee>(), loreItemPage);
            ItemRedirect(ItemType<LoreQueenSlime>(), loreItemPage);
            ItemRedirect(ItemType<LoreRavager>(), loreItemPage);
            ItemRedirect(ItemType<LoreRequiem>(), loreItemPage);
            ItemRedirect(ItemType<LoreSignus>(), loreItemPage);
            ItemRedirect(ItemType<LoreSkeletron>(), loreItemPage);
            ItemRedirect(ItemType<LoreSkeletronPrime>(), loreItemPage);
            ItemRedirect(ItemType<LoreSlimeGod>(), loreItemPage);
            ItemRedirect(ItemType<LoreStormWeaver>(), loreItemPage);
            ItemRedirect(ItemType<LoreSulphurSea>(), loreItemPage);
            ItemRedirect(ItemType<LoreTwins>(), loreItemPage);
            ItemRedirect(ItemType<LoreUnderworld>(), loreItemPage);
            ItemRedirect(ItemType<LoreWallofFlesh>(), loreItemPage);
            ItemRedirect(ItemType<LoreYharon>(), loreItemPage);

            // Enemies
            EnemyRedirect(NPCType<KingSlimeJewelRuby>(), "Crown Jewels");
            EnemyRedirect(NPCType<OldDukeToothBall>(), "Tooth Ball (Old Duke)");
            EnemyRedirect(NPCType<CalamitasEnchantDemon>(), "Enchantment");
            EnemyRedirect(NPCType<LeviathanStart>(), "%3F%3F%3F");
            */
        }

        /*
        public static void SpiritActivateMysticMoon()
        {
            if (modSpirit == null || Main.IsItDay())
                return;

            SetOtherModBool("SpiritMod.MyWorld", "calmNight", false);
            SetOtherModBool("SpiritMod.MyWorld", "jellySky", false);
            SetOtherModBool("SpiritMod.MyWorld", "blueMoon", true);
        }

        public static void SpiritActivateJellyMoon()
        {
            if (modSpirit == null || Main.IsItDay())
                return;

            SetOtherModBool("SpiritMod.MyWorld", "calmNight", false);
            SetOtherModBool("SpiritMod.MyWorld", "jellySky", true);
            SetOtherModBool("SpiritMod.MyWorld", "blueMoon", false);
        }
        */

        private static void SetOtherModBool(string className, string fieldName, bool newValue)
        {
            Type myWorldType = modSpirit.Code.GetType(className);

            FieldInfo boolField = myWorldType?.GetField(
                fieldName,
                BindingFlags.Public | BindingFlags.Static
            );

            if (boolField != null)
                boolField.SetValue(null, newValue);
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
            LocalizedText authorHaise = Language.GetText("Mods.ChangedSpecialMod.MusicDisplay.Authors.Haise");

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

            // Upcoming, unknown song names
            MusicDisplayAddTrack(changedMod, modName, author, Sounds.Music30, "Music30", defaultColors);

            // Changed Minecraft addon
            // Need to ask for permission, so don't use it anywhere now. It sounds very different from most songs
            MusicDisplayAddTrack(changedMod, modName, authorHaise, Sounds.MusicMeaninglessStrafe, "MusicMeaninglessStrafe", defaultColors);
        }

        private static void MusicDisplayAddTrack(Mod hostMod, LocalizedText modName, LocalizedText author, string musicPath, string musicName, Color[] colors)
        {
            LocalizedText displayName = Language.GetText($"Mods.ChangedSpecialMod.MusicDisplay.Music.{musicName}");
            modMusicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot(hostMod, musicPath), displayName, author, modName, null, colors);
        }
    }
}
