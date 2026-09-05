using ChangedSpecialMod.Common.Systems.Mods;
using ChangedSpecialMod.Content.Items.Summons;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.NPCs.TownPets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.OS;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class ExternalModData
    {
        public Mod mod;
        public Dictionary<string, int> tileTypes = new Dictionary<string, int>();
        public Dictionary<string, int> townNPCIds = new Dictionary<string, int>();
        public List<int> avoidedByOrangeShrines = new List<int>();
        public bool isActive => mod != null;

        public ExternalModData(Mod mod) 
        {
            this.mod = mod;
        }

        public void AddTileType(string name, bool avoidedByOrangeShrine = false)
        {
            if (tileTypes == null)
                tileTypes = new Dictionary<string, int>();

            if (mod == null || tileTypes.ContainsKey(name))
                return;

            if (mod.TryFind(name, out ModTile modTile))
            {
                var itemType = modTile.Type;
                tileTypes.Add(name, itemType);
                if (avoidedByOrangeShrine)
                    avoidedByOrangeShrines.Add(itemType);
            }
        }

        public void AddTownNPC(string internalName, string localizationName)
        {
            if (townNPCIds == null)
                townNPCIds = new Dictionary<string, int>();

            if (mod == null || townNPCIds.ContainsKey(localizationName))
                return;

            if (mod.TryFind(internalName, out ModNPC modNPC))
            {
                var npcType = modNPC.Type;
                townNPCIds.Add(localizationName, npcType);
            }
        }

        public bool ShouldAvoidTileType(int tileType)
        {
            return avoidedByOrangeShrines.Contains(tileType);
        }
    }

    public class ModSupportSystem : ModSystem
    {
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
        public static Mod modStarsModPack = null;

        // Race
        public static Mod modMrPlagueRaces = null;

        // Other
        public static Mod modBoulderBackport = null;
        // Don't need to do anything for Biome Titles

        // The min and max indecis for the extra title message added in the hjson
        private static int indexTitleMessageMin = 1;
        private static int indexTitleMessageMax = 5;

        public static List<ExternalModData> externalModsData;

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

            // Race
            modMrPlagueRaces = GetMod("MrPlagueRaces");

            // Other
            modBoulderBackport = GetMod("BoulderBackport");
            modStarsModPack = GetMod("StarsModPack");

            externalModsData = new List<ExternalModData>();
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

            // Race
            modMrPlagueRaces = null;

            // Other
            modBoulderBackport = null;
            modStarsModPack = null;

            externalModsData = null;

            if (Platform.Current.Type == PlatformType.Windows)
                RemoveExtraTitles();
        }

        public override void PostSetupContent()
        {
            SetupBossChecklist();
            SetupFargosMutant();
            MusicDisplaySystem.Setup(changedMod, modMusicDisplay);
            SetupCensus();
            WikithisSystem.Setup(changedMod, modWikiThis);

            SetupExternalModData();

            // This might fix a startup crash on MacOS, but I can't confirm this
            if (Platform.Current.Type == PlatformType.Windows)
            {
                SetupExtraTitles();
                TryUpdateTitle();
            }
        }

        private static void SetupExternalModData()
        {
            if (modCalamity != null)
            {
                var externalModData = new ExternalModData(modCalamity);

                // Draedon Lab
                externalModData.AddTileType("LaboratoryPlating", true);

                // Sunken sea
                externalModData.AddTileType("EutrophicSand", true);
                externalModData.AddTileType("Navystone", true);

                // Sulphuric sea
                externalModData.AddTileType("SulphurousSand", true);
                externalModData.AddTileType("SulphurousSandstone", true);
                externalModData.AddTileType("SulphurousShale", true);
                externalModData.AddTileType("HardenedSulphurousSandstone", true);

                // Abyss
                externalModData.AddTileType("AbyssGravel", true);
                externalModData.AddTileType("PyreMantle", true);
                externalModData.AddTileType("Voidstone", true);

                // Town NPCs
                externalModData.AddTownNPC("DILF", "CalamityArchmage");
                externalModData.AddTownNPC("THIEF", "CalamityBandit");
                externalModData.AddTownNPC("SEAHOE", "CalamitySeaKing");
                externalModData.AddTownNPC("WITCH", "CalamityBrimstoneWitch");

                externalModsData.Add(externalModData);
            }

            if (modCoralite != null)
            {
                var externalModData = new ExternalModData(modCoralite);

                // Crystal cave
                externalModData.AddTileType("BasaltTile", true);
                externalModData.AddTileType("CrystalBasaltTile", true);
                externalModData.AddTileType("HardBasaltTile", true);
                externalModData.AddTileType("MagicCrystalBrickTile", true);

                // Town NPCs
                externalModData.AddTownNPC("CrystalRobot", "CoraliteCrystalRobot");
                externalModData.AddTownNPC("ElfRanger", "CoraliteElfRanger");

                externalModsData.Add(externalModData);
            }

            if (modThorium != null)
            {
                var externalModData = new ExternalModData(modThorium);

                // Town NPCs
                externalModData.AddTownNPC("Blacksmith", "ThoriumBlacksmith");
                externalModData.AddTownNPC("Cobbler", "ThoriumCobbler");
                externalModData.AddTownNPC("ConfusedZombie", "ThoriumConfusedZombie");
                externalModData.AddTownNPC("Cook", "ThoriumCook");
                externalModData.AddTownNPC("DesertAcolyte", "ThoriumDesertAcolyte");
                externalModData.AddTownNPC("Diverman", "ThoriumDiverman");
                externalModData.AddTownNPC("Druid", "ThoriumDruid");
                externalModData.AddTownNPC("Spiritualist", "ThoriumSpiritualist");
                externalModData.AddTownNPC("Tracker", "ThoriumTracker");
                externalModData.AddTownNPC("WeaponMaster", "ThoriumWeaponMaster");

                externalModsData.Add(externalModData);
            }

            if (modStarsModPack != null)
            {
                var externalModData = new ExternalModData(modStarsModPack);

                // Town NPCs
                externalModData.AddTownNPC("Farmer", "StarsModPackFarmer");
                externalModData.AddTownNPC("Jellybean", "StarsModPackJellybean");
                externalModData.AddTownNPC("Scarfy", "StarsModPackScarfy");
                externalModData.AddTownNPC("ScrapyardGeek", "StarsModPackScrapyardGeek");
                externalModData.AddTownNPC("SubzeroSpecialist", "StarsModPackSubzeroSpecialist");

                externalModsData.Add(externalModData);
            }
        }

        public static List<int> GetAvoidTiles()
        {
            var list = new List<int>();

            if (externalModsData != null)
            {
                foreach (var externalModData in  externalModsData)
                    list.AddRange(externalModData.avoidedByOrangeShrines);
            }

            return list;
        }

        public static bool CheckIfShouldAvoidTile(int tileType)
        {
            if (externalModsData == null)
                return false;

            foreach (var externalModData in externalModsData)
            {
                if (externalModData.ShouldAvoidTileType(tileType))
                    return true;
            }

            return false;
        }

        private void TryUpdateTitle()
        {
            MethodInfo setTitleMethod = typeof(Main).GetMethod("SetTitle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (setTitleMethod != null)
                setTitleMethod.Invoke(Main.instance, null);
        }

        private void SetupExtraTitles()
        {
            var gameTitleKeys = LanguageManager.Instance.GetKeysInCategory("GameTitle");
            for (int i = indexTitleMessageMin; i <= indexTitleMessageMax; i++)
            {
                if (!gameTitleKeys.Contains($"Changed{i.ToString()}"))
                    gameTitleKeys.Add($"Changed{i.ToString()}");
            }
        }

        private void RemoveExtraTitles()
        {
            var gameTitleKeys = LanguageManager.Instance.GetKeysInCategory("GameTitle");
            for (int i = indexTitleMessageMin; i <= indexTitleMessageMax; i++)
            {
                if (gameTitleKeys.Contains($"Changed{i.ToString()}"))
                    gameTitleKeys.Remove($"Changed{i.ToString()}");
            }
        }

        private void SetupCensus()
        {
            if (changedMod == null || modCensus == null)
                return;

            try
            {
                modCensus.Call("TownNPCCondition", ModContent.NPCType<Puro>(), Language.GetText("Mods.ChangedSpecialMod.NPCs.Puro.Census.SpawnCondition"));
                modCensus.Call("TownNPCCondition", ModContent.NPCType<Prototype>(), Language.GetText("Mods.ChangedSpecialMod.NPCs.Prototype.Census.SpawnCondition"));
                modCensus.Call("TownNPCCondition", ModContent.NPCType<Scientist>(), Language.GetText("Mods.ChangedSpecialMod.NPCs.Scientist.Census.SpawnCondition"));
                modCensus.Call("TownNPCCondition", ModContent.NPCType<Colin>(), Language.GetText("Mods.ChangedSpecialMod.NPCs.Colin.Census.SpawnCondition"));
                modCensus.Call("TownNPCCondition", ModContent.NPCType<DarkLatexCubTownPet>(), Language.GetText("Mods.ChangedSpecialMod.NPCs.DarkLatexCubTownPet.Census.SpawnCondition"));
                modCensus.Call("TownNPCCondition", ModContent.NPCType<WhiteLatexCubTownPet>(), Language.GetText("Mods.ChangedSpecialMod.NPCs.WhiteLatexCubTownPet.Census.SpawnCondition"));
            }
            catch(Exception ex)
            {
                Mod.Logger.Error("Census Call Error: " + ex.Message);
            }
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
    }
}
