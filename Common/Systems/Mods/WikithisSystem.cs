using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Items.Ammo;
using ChangedSpecialMod.Content.Items.Debug;
using ChangedSpecialMod.Content.Items.Licenses;
using ChangedSpecialMod.Content.Items.Mounts;
using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.Items.Placeable.Latex.Black;
using ChangedSpecialMod.Content.Items.Placeable.Latex.White;
using ChangedSpecialMod.Content.Items.Placeable.Pylons;
using ChangedSpecialMod.Content.Items.Summons;
using ChangedSpecialMod.Content.Items.Syringes;
using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.NPCs.Drunk;
using ChangedSpecialMod.Content.NPCs.TownPets;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.Mods
{
    public static class WikithisSystem
    {
        private static ChangedSpecialMod changedMod;
        private static Mod targetMod;

        private static string wikiUrl = "https://changedterraria.wiki.gg/wiki/";

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

        public static void Setup(ChangedSpecialMod _changedMod, Mod _targetMod)
        {
            // Wikithis is a clientside mod
            if (Main.dedServ)
                return;

            changedMod = _changedMod;
            targetMod = _targetMod;

            if (targetMod == null)
                return;

            var wikiURL = "https://changedterraria.wiki.gg/wiki/{}";

            targetMod.Call("AddModURL", changedMod, wikiURL);
            targetMod.Call(0, changedMod, wikiURL);
            //wiki.Call("AddWikiTexture", calamity, Request<Texture2D>("CalamityMod/ModSupport/WikiThisIcon"));
            //wiki.Call(3, calamity, Request<Texture2D>("CalamityMod/ModSupport/WikiThisIcon"));

            // Clear up name conflicts
            void ItemRedirect(int item, string pageName) => targetMod.Call(1, item, wikiUrl + pageName);
            void EnemyRedirect(int item, string pageName) => targetMod.Call(2, item, wikiUrl + pageName);

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
            foreach (var item in items)
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
    }
}
