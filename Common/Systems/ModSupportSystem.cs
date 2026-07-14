using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Items.Ammo;
using ChangedSpecialMod.Content.Items.Debug;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Items.Licenses;
using ChangedSpecialMod.Content.Items.Mounts;
using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.Items.Placeable.Latex;
using ChangedSpecialMod.Content.Items.Placeable.Pylons;
using ChangedSpecialMod.Content.Items.Summons;
using ChangedSpecialMod.Content.Items.Syringes;
using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.NPCs.TownPets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        // Race
        public static Mod modMrPlagueRaces = null;

        // Other
        public static Mod modBoulderBackport = null;
        // Don't need to do anything for Biome Titles
        // He is a nice guy, so let's surprise him
        public static Mod modStarsModPack = null;

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
            SetupMusicDisplay();
            SetupCensus();
            SetupWikiThis();

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

        private static Dictionary<int, string> GetNPCUrls()
        {
            return new Dictionary<int, string>()
            {
                // Bosses
                { ModContent.NPCType<WhiteTail>(), "White_Tail" },
                { ModContent.NPCType<WolfKingSpawn>(), "Wolf_King" },
                { ModContent.NPCType<WolfKing>(), "Wolf_King" },
                { ModContent.NPCType<Behemoth>(), "Behemoth" },
                { ModContent.NPCType<BehemothHand>(), "Behemoth" },
                { ModContent.NPCType<BehemothSpawn>(), "Behemoth" },

                // Bosses wip
                { ModContent.NPCType<Experiment009>(), "Experiment_009" },
                { ModContent.NPCType<TigerSharkBoss>(), "Tiger_Shark_Boss" },

                // Town NPCs
                { ModContent.NPCType<Puro>(), "Puro" },
                { ModContent.NPCType<Prototype>(), "Prototype" },
                { ModContent.NPCType<Scientist>(), "Dr_K" },
                { ModContent.NPCType<Colin>(), "Colin" },
                { ModContent.NPCType<DarkLatexCubTownPet>(), "Black_Goo_Puppy" },
                { ModContent.NPCType<WhiteLatexCubTownPet>(), "White_Goo_Puppy" },

                // Black Surface
                { ModContent.NPCType<BlackGoop>(), "Black_Goop" },
                { ModContent.NPCType<DarkLatexCub>(), "Black_Latex_Cub" },
                { ModContent.NPCType<MaleDarkLatex>(), "Black_Latex" },
                { ModContent.NPCType<FemaleDarkLatex>(), "Black_Latex" },
                { ModContent.NPCType<FlyingDarkLatex>(), "Yufeng_Cub" },
                { ModContent.NPCType<WingedDarkLatex>(), "Yufeng" },
                { ModContent.NPCType<Wendigo>(), "Wendigo" },

                // Black Cave
                { ModContent.NPCType<CrystalWolfBlue>(), "Sapphire_Wolf" },
                { ModContent.NPCType<CrystalWolfGreen>(), "Emerald_Wolf" },
                { ModContent.NPCType<CrystalWolfPurple>(), "Amethyst_Wolf" },
                { ModContent.NPCType<CrystalWolfRed>(), "Ruby_Wolf" },

                // Black Drunk
                { ModContent.NPCType<BackLatex>(), "Back_Latex" },
                { ModContent.NPCType<PuroWormHead>(), "Devourer_Of_Oranges" },
                { ModContent.NPCType<PuroWormBody>(), "Devourer_Of_Oranges" },
                { ModContent.NPCType<PuroWormTail>(), "Devourer_Of_Oranges" },
                { ModContent.NPCType<DarkLatexCubOfDoom>(), "Puppy_Of_Doom" },
                { ModContent.NPCType<QuackLatex>(), "Quack_Latex" },
                { ModContent.NPCType<SnackLatex>(), "Snack_Latex" },
                { ModContent.NPCType<StackLatex>(), "Stack_Latex" },
                { ModContent.NPCType<WackLatex>(), "Wack_Latex" },

                // White Surface
                { ModContent.NPCType<WhiteGoop>(), "White_Goop" },
                { ModContent.NPCType<WhiteLatexCub>(), "White_Latex_Cub" },
                { ModContent.NPCType<WhiteKnight>(), "White_Knight" },
                { ModContent.NPCType<WhiteLatexTaur>(), "White_Latex_Taur" },

                // White Cave
                { ModContent.NPCType<Snek>(), "Snake" },
                { ModContent.NPCType<LatexMoth>(), "Slime Moth" },

                // White Drunk
                { ModContent.NPCType<BrightLatex>(), "Bright_Latex" },
                { ModContent.NPCType<FightLatex>(), "Fight_Latex" },
                { ModContent.NPCType<FlightLatex>(), "Flight_Latex" },
                { ModContent.NPCType<HideLatex>(), "Hide_Latex" },
                { ModContent.NPCType<MightLatex>(), "Might_Latex" },
                { ModContent.NPCType<SideLatex>(), "Side_Latex" },
                { ModContent.NPCType<WideLatex>(), "Wide_Latex" },

                // Aquatic
                { ModContent.NPCType<SquidDogCub>(), "Squid_Dog_Cub" },
                { ModContent.NPCType<SquidDog>(), "Squid_Dog" },
                { ModContent.NPCType<TigerShark>(), "Tiger_Shark" },

                // World Evil
                { ModContent.NPCType<Bloodstripe>(), "Bloodstripe" },
                { ModContent.NPCType<Purrpurr>(), "Purrpurr" },

                // Other
                { ModContent.NPCType<Lion>(), "Feng_Yu" },
                { ModContent.NPCType<Snep>(), "Snow_Leopard" },
                { ModContent.NPCType<GermanShepherd>(), "German_Shepherd" },
                { ModContent.NPCType<ExoSuitRobot>(), "Maintenance_Robot" },
                { ModContent.NPCType<MutatedLatex>(), "Mutated_Latex" },
                { ModContent.NPCType<Raccoon>(), "Raccoon" },
                { ModContent.NPCType<Spike>(), "Spike" },
                { ModContent.NPCType<Sweeper>(), "Sweeper" },
                { ModContent.NPCType<SweeperPuro>(), "Puroomba" },
                { ModContent.NPCType<HungryLocker>(), "Hungry_Locker" },
            };
        }

        private static List<int> GetItemsWeapons()
        {
            return new List<int>()
            {
                ModContent.ItemType<Mollash>(),
                ModContent.ItemType<Twintacle>(),
                ModContent.ItemType<Tentatrio>(),

                ModContent.ItemType<TigerPaw>(),
                ModContent.ItemType<Shredder>(),
                ModContent.ItemType<MegaMauler>(),

                ModContent.ItemType<WhiskerStaff>(),
                ModContent.ItemType<TabbyStaff>(),
                ModContent.ItemType<PurrpurrStaff>(),

                ModContent.ItemType<Dreadhorn>(),
                ModContent.ItemType<Bashskewer>(),
                ModContent.ItemType<Daemonspike>(),

                ModContent.ItemType<Encyclopedia>(),
                ModContent.ItemType<Literature>(),
                ModContent.ItemType<BookBarrage>(),

                ModContent.ItemType<BasketballWeapon>(),
            };
        }

        private static List<int> GetItemsDebug()
        {
            return new List<int>()
            {
                ModContent.ItemType<DebugResetBosses>(),
                ModContent.ItemType<DebugSpawnAllNPCs>(),
                ModContent.ItemType<DebugSpawnColin>()
            };
        }

        private static List<int> GetItemsSolutions()
        {
            return new List<int>()
            {
                ModContent.ItemType<BlackLatexSolution>(),
                ModContent.ItemType<DryDirtSolution>(),
                ModContent.ItemType<WhiteLatexSolution>()
            };
        }

        private static List<int> GetItemsPaintings()
        {
            return new List<int>()
            {
                // Painting
                ModContent.ItemType<Painting1>(),
                ModContent.ItemType<Painting2>(),
                ModContent.ItemType<Painting3>(),
                ModContent.ItemType<Painting4>(),
                ModContent.ItemType<Painting5>(),
                ModContent.ItemType<Painting6>(),
                ModContent.ItemType<Painting7>(),
                ModContent.ItemType<Painting8>(),
                ModContent.ItemType<Painting9>(),
                ModContent.ItemType<Painting10>(),
                ModContent.ItemType<Painting11>(),
                ModContent.ItemType<Painting12>(),
                ModContent.ItemType<Painting13>(),
                ModContent.ItemType<Painting14>(),
                ModContent.ItemType<Painting15>(),
                ModContent.ItemType<Painting16>(),
                ModContent.ItemType<Painting17>(),
                ModContent.ItemType<Painting18>(),
                ModContent.ItemType<Painting19>(),

                // Drunk paintings
                ModContent.ItemType<DrunkPainting1>(),
                ModContent.ItemType<DrunkPainting2>(),
                ModContent.ItemType<DrunkPainting3>(),
                ModContent.ItemType<DrunkPainting4>(),
                ModContent.ItemType<DrunkPainting5>(),
                ModContent.ItemType<DrunkPainting6>()
            };
        }

        private static List<int> GetItemsSyringes()
        {
            return new List<int>()
            {
                // Normal syringes
                ModContent.ItemType<BloodstripeSyringe>(),
                ModContent.ItemType<LionSyringe>(),
                ModContent.ItemType<GermanShepherdSyringe>(),
                ModContent.ItemType<PurrpurrSyringe>(),
                ModContent.ItemType<SnepSyringe>(),
                ModContent.ItemType<SquidDogSyringe>(),
                ModContent.ItemType<TigerSharkSyringe>(),

                // Combi syringes
                ModContent.ItemType<BlackSyringe>(),
                ModContent.ItemType<WhiteSyringe>(),
                ModContent.ItemType<AquaticSyringe>(),
                ModContent.ItemType<MiscSyringe>(),
                ModContent.ItemType<SuperSyringe>()
            };
        }

        private static List<int> GetItemsStatues()
        {
            return new List<int>()
            {
                ModContent.ItemType<OrangeStatue>(),
                ModContent.ItemType<PuroStatue>(),
                ModContent.ItemType<SquidDogStatue>()
            };
        }

        private static List<int> GetItemsPylons()
        {
            return new List<int>()
            {
                ModContent.ItemType<BlackLatexPylon>(),
                ModContent.ItemType<WhiteLatexPylon>()
            };
        }

        private static List<int> GetItemsPictures()
        {
            return new List<int>()
            {
                ModContent.ItemType<Pictures1>(),
                ModContent.ItemType<Pictures2>(),
                ModContent.ItemType<Pictures3>(),
                ModContent.ItemType<Pictures4>(),
                ModContent.ItemType<Pictures5>(),
                ModContent.ItemType<Pictures6>()
            };
        }

        private static List<int> GetItemsUnobtainable()
        {
            return new List<int>
            {
                ModContent.ItemType<ElevatorDown>(),
                ModContent.ItemType<ElevatorUp>(),
                ModContent.ItemType<PackingBoxKey>(),
                ModContent.ItemType<AlpineHat>(),
                ModContent.ItemType<SombreroHat>(),
                ModContent.ItemType<Sunglasses>()
            };
        }

        private static List<int> GetItemsToys()
        {
            return new List<int>
            {
                ModContent.ItemType<PuroPlush>(),
                ModContent.ItemType<SharkPlush>(),
                ModContent.ItemType<FennecPlush>(),
                ModContent.ItemType<Basketball>(),
                ModContent.ItemType<Blocks>()
            };
        }

        private static List<int> GetItemsBossSummons()
        {
            return new List<int>
            {
                ModContent.ItemType<SummonWhiteTail>(),
                ModContent.ItemType<SummonWolfKing>(),
                ModContent.ItemType<SummonBehemoth>(),

                ModContent.ItemType<SummonExperiment009>(),
                ModContent.ItemType<SummonShark>()
            };
        }

        private static List<int> GetItemsBanners()
        {
            return new List<int>
            {
                ModContent.ItemType<BlackGoopBanner>(),
                ModContent.ItemType<BloodstripeBanner>(),
                ModContent.ItemType<DarkLatexBanner>(),
                ModContent.ItemType<FlyingDarkLatexBanner>(),
                ModContent.ItemType<GermanShepherdBanner>(),
                ModContent.ItemType<PurrpurrBanner>(),
                ModContent.ItemType<SquidDogBanner>(),
                ModContent.ItemType<WendigoBanner>(),
                ModContent.ItemType<WhiteGoopBanner>(),
                ModContent.ItemType<WhiteLatexBanner>(),
            };
        }

        private static List<int> GetItemsBlocks()
        {
            return new List<int>
            {
                // Latex
                ModContent.ItemType<BlackLatexBlock>(),
                ModContent.ItemType<BlackLatexSand>(),
                ModContent.ItemType<BlackLatexStoneBlock>(),
                ModContent.ItemType<WhiteLatexBlock>(),
                ModContent.ItemType<WhiteLatexSand>(),
                ModContent.ItemType<WhiteLatexStoneBlock>(),

                ModContent.ItemType<CautionTileBlock>(),
                ModContent.ItemType<DryDirtBlock>(),
                ModContent.ItemType<LabTileBlock>()
            };
        }

        private static List<int> GetItemsMusicBoxes()
        {
            return new List<int>
            {
                ModContent.ItemType<MusicBoxBehemoth>(),
                ModContent.ItemType<MusicBoxBlackLatexZone1>(),
                ModContent.ItemType<MusicBoxBlackLatexZone2>(),
                ModContent.ItemType<MusicBoxCrystalZone>(),
                ModContent.ItemType<MusicBoxGreenhouse>(),
                ModContent.ItemType<MusicBoxHappyBirthday>(),
                ModContent.ItemType<MusicBoxLab>(),
                ModContent.ItemType<MusicBoxLabSlow>(),
                ModContent.ItemType<MusicBoxLibrary>(),
                ModContent.ItemType<MusicBoxPuro>(),
                ModContent.ItemType<MusicBoxPuroDance>(),
                ModContent.ItemType<MusicBoxVents>(),
                ModContent.ItemType<MusicBoxWhiteLatexZone>(),
                ModContent.ItemType<MusicBoxWhiteTail>(),
                ModContent.ItemType<MusicBoxWolfKing>()
            };
        }

        private static List<int> GetItemsWallMount()
        {
            return new List<int>
            {
                ModContent.ItemType<DocumentPaper>(),
                ModContent.ItemType<Elevator>(),
                ModContent.ItemType<IrisScanner>(),
                ModContent.ItemType<Fan>(),
                ModContent.ItemType<SkinLion>(),
                ModContent.ItemType<SkinSnep>()
            };
        }

        private static List<int> GetItemsEnvironment()
        {
            return new List<int>
            {
                ModContent.ItemType<CrystalGreen>(),
                ModContent.ItemType<CrystalRed>(),
                ModContent.ItemType<CrystalWhite>(),
                ModContent.ItemType<PillarWhite>()
            };
        }

        private static List<int> GetItemsFurniture()
        {
            return new List<int>
            {
                ModContent.ItemType<MountBookest>(),

                ModContent.ItemType<BlueOfficeChair>(),
                ModContent.ItemType<GreenOfficeChair>(),
                ModContent.ItemType<RedOfficeChair>(),

                ModContent.ItemType<BlueGasTank>(),
                ModContent.ItemType<RedGasTank>(),

                ModContent.ItemType<Generator>(),
                ModContent.ItemType<Cryopod>(),
                ModContent.ItemType<LabDoor>(),
                ModContent.ItemType<LabTable>(),
                ModContent.ItemType<Locker>(),
                ModContent.ItemType<PackingBox>(),
                ModContent.ItemType<StackOfBoxes>(),
                ModContent.ItemType<StorageBox>(),
                ModContent.ItemType<WateringCan>(),
                ModContent.ItemType<WhiteLatexBookcases>()
            };
        }

        private static List<int> GetItemsMounts()
        {
            return new List<int>()
            {
                ModContent.ItemType<FlyingDarkLatexMountItem>(),
                ModContent.ItemType<LatexMothMountItem>(),
                ModContent.ItemType<WhiteLatexTaurMountItem>()
            };
        }

        private static List<int> GetItemsMisc()
        {
            return new List<int>()
            {
                ModContent.ItemType<BlackLatexCubLicense>(),
                ModContent.ItemType<WhiteLatexCubLicense>(),

                ModContent.ItemType<Calendar>(),
                ModContent.ItemType<DiscoCrystal>(),
                ModContent.ItemType<Paper>(),
                ModContent.ItemType<RottenOrange>(),

                // Seasons
                // REMOVED, replaced with calendar

                ModContent.ItemType<SweeperItem>(),
                ModContent.ItemType<SweeperPuroItem>()
            };
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

            var urls = GetNPCUrls();
            var keys = urls.Keys;
            foreach (var key in keys)
                EnemyRedirect(key, urls[key]);

            // Blocks
            var items = GetItemsBlocks();
            foreach (var item in items)
                ItemRedirect(item, "Blocks");

            // Debug items
            items = GetItemsDebug();
            foreach(var item in items)
                ItemRedirect(item, "Debug_Items");

            // Unobtainables
            items = GetItemsUnobtainable();
            foreach (var item in items)
                ItemRedirect(item, "Unobtainable_Items");

            // Boss summons
            items = GetItemsBossSummons();
            foreach (var item in items)
                ItemRedirect(item, "Summoning_Items");

            // Weapons
            items = GetItemsWeapons();
            foreach (var item in items)
                ItemRedirect(item, "Weapons");

            // Solutions
            items = GetItemsSolutions();
            foreach (var item in items)
                ItemRedirect(item, "Solutions");

            // Syringes
            items = GetItemsSyringes();
            foreach (var item in items)
                ItemRedirect(item, "Syringes");

            // Toys
            items = GetItemsToys();
            foreach (var item in items)
                ItemRedirect(item, "Furniture");

            // Banners
            items = GetItemsBanners();
            foreach (var item in items)
                ItemRedirect(item, "Furniture");

            // Statues
            items = GetItemsStatues();
            foreach (var item in items)
                ItemRedirect(item, "Furniture");

            // Pylons
            items = GetItemsPylons();
            foreach (var item in items)
                ItemRedirect(item, "Furniture");

            // Pictures
            items = GetItemsPictures();
            foreach (var item in items)
                ItemRedirect(item, "Furniture");

            // Music Boxes
            items = GetItemsMusicBoxes();
            foreach (var item in items)
                ItemRedirect(item, "Furniture");

            // Wall Mounts
            items = GetItemsWallMount();
            foreach (var item in items)
                ItemRedirect(item, "Furniture");

            // Environment
            items = GetItemsEnvironment();
            foreach (var item in items)
                ItemRedirect(item, "Furniture");

            // Furniture
            items = GetItemsFurniture();
            foreach (var item in items)
                ItemRedirect(item, "Furniture");

            // Paintings
            items = GetItemsPaintings();
            foreach (var item in items)
                ItemRedirect(item, "Paintings");

            // Mounts
            items = GetItemsMounts();
            foreach (var item in items)
                ItemRedirect(item, "Mounts");

            // Misc
            items = GetItemsMisc();
            foreach (var item in items)
                ItemRedirect(item, "Miscellaneous_Items");
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

        private static void SetOtherModBool(Mod mod, string className, string fieldName, bool newValue)
        {
            Type myWorldType = mod.Code.GetType(className);

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
