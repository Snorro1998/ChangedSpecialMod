using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Content.Tiles.Latex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

//using ModLiquidLib.Utils.LiquidContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static ChangedSpecialMod.ChangedSpecialMod;


// This thing is a mess and contains all kinds of functions that probably should be in their own class
namespace ChangedSpecialMod.Utilities
{
    public static partial class Things
    {
        public static ChangedNPC Changed(this NPC npc) => npc.GetGlobalNPC<ChangedNPC>();
        //public static ModLiquidNPC ModLiquid(this NPC npc) => npc.GetGlobalNPC<ModLiquidNPC>();
        public static ChangedSpecialModPlayer ChangedPlayer(this Player player) => player.GetModPlayer<ChangedSpecialModPlayer>();
    }

    public class ChangedUtils : ModSystem
    {
        // Black
        public static bool InBlackLatexSurfaceBiome(Player player) => player.InModBiome<BlackLatexSurfaceBiome>();
        public static bool InBlackLatexUndergroundBiome(Player player) => player.InModBiome<BlackLatexUndergroundBiome>();
        public static bool InBlackLatexBiome(Player player)
        {
            return InBlackLatexSurfaceBiome(player) || InBlackLatexUndergroundBiome(player);
        }

        // White
        public static bool InWhiteLatexSurfaceBiome(Player player) => player.InModBiome<WhiteLatexSurfaceBiome>();
        public static bool InWhiteLatexUndergroundBiome(Player player) => player.InModBiome<WhiteLatexUndergroundBiome>();
        public static bool InWhiteLatexBiome(Player player)
        {
            return InWhiteLatexSurfaceBiome(player) || InWhiteLatexUndergroundBiome(player);
        }


        public static bool InCityRuinsBiome(Player player) => player.InModBiome<CityRuinsSurfaceBiome>();
        public static bool InChangedSurfaceBiome(Player player)
        {
            return InCityRuinsBiome(player) || InBlackLatexSurfaceBiome(player) || InWhiteLatexSurfaceBiome(player);
        }
        public static bool InChangedBiome(Player player)
        {
            return InCityRuinsBiome(player) || InBlackLatexBiome(player) || InWhiteLatexBiome(player);
        }

        public override void Load()
        {
            On_UIWorldCreation.AssignRandomWorldName += MyAssignRandomWorldName;
            On_Lang.GetDryadWorldStatusDialog += Hook_GetDryadWorldStatusDialog;
        }

        public override void Unload()
        {
            On_UIWorldCreation.AssignRandomWorldName -= MyAssignRandomWorldName;
            On_Lang.GetDryadWorldStatusDialog -= Hook_GetDryadWorldStatusDialog;
        }

        private static string Hook_GetDryadWorldStatusDialog(
    On_Lang.orig_GetDryadWorldStatusDialog orig,
    out bool worldIsEntirelyPure)
        {
            var config = ModContent.GetInstance<ChangedSpecialModClientConfig>();

            if (!config.CustomDryadWorldStatus)
                return orig(out worldIsEntirelyPure);

            // Force to recalculate
            WorldGen.AddUpAlignmentCounts(true);

            string text = "";
            worldIsEntirelyPure = false;
            
            var nTotalBlocks = 0;
            var nGood = 0;
            var nEvil = 0;
            var nBlood = 0;
            var nLatex = 0;

            List<int> latexBlocks = new List<int>()
            {
                ModContent.TileType<BlackLatexTile>(),
                ModContent.TileType<BlackLatexSandTile>(),
                ModContent.TileType<BlackLatexStoneTile>(),

                ModContent.TileType<WhiteLatexTile>(),
                ModContent.TileType<WhiteLatexSandTile>(),
                ModContent.TileType<WhiteLatexStoneTile>()
            };

            var CorruptCountCollection = new List<int> { 23, 661, 25, 112, 163, 398, 400, 636, 24, 32 };
            var CrimsonCountCollection = new List<int> { 199, 662, 203, 234, 200, 399, 401, 205, 201, 352 };
            var HallowCountCollection = new List<int> { 109, 117, 116, 164, 402, 403, 115, 110, 113 };

            for (var x = 0; x < Main.maxTilesX; x++)
            {
                for (var y = 0; y < Main.maxTilesY; y++)
                {
                    var tile = Main.tile[x, y];
                    if (!tile.HasTile)
                        continue;
                    nTotalBlocks++;
                    if (latexBlocks.Contains(tile.TileType))
                        nLatex++;
                    else if (CorruptCountCollection.Contains(tile.TileType))
                        nEvil++;
                    else if (CrimsonCountCollection.Contains(tile.TileType))
                        nBlood++;
                    else if (HallowCountCollection.Contains(tile.TileType))
                        nGood++;
                }
            }

            int tGood = (byte)Math.Round((double)nGood / (double)WorldGen.totalSolid * 100.0);// WorldGen.tGood;
            if (tGood == 0 && nGood > 0)
                tGood = 1;
            int tEvil = (byte)Math.Round((double)nEvil / (double)WorldGen.totalSolid * 100.0);// WorldGen.tEvil;
            if (tEvil == 0 && nEvil > 0)
                tEvil = 1;
            int tBlood = (byte)Math.Round((double)nBlood / (double)WorldGen.totalSolid * 100.0);// WorldGen.tBlood;
            if (tBlood == 0 && nBlood > 0)
                tBlood = 1;
            int tLatex = (byte)Math.Round((double)nLatex / (double)WorldGen.totalSolid * 100.0);
            if (tLatex == 0 && nLatex > 0)
                tLatex = 1;

            var baseTextPath = "Mods.ChangedSpecialMod.ExtraDialogue.Dryad.WorldStatus.";

            if (tLatex > 0)
            {
                if (tGood > 0 && tEvil > 0 && tBlood > 0)
                {
                    text = Language.GetTextValue($"{baseTextPath}WorldIsHallowCorruptCrimsonLatex", Main.worldName, tGood, tEvil, tBlood, tLatex);
                }
                else if (tGood > 0 && tEvil > 0)
                {
                    text = Language.GetTextValue($"{baseTextPath}WorldIsHallowCorruptLatex", Main.worldName, tGood, tEvil, tLatex);
                }
                else if (tGood > 0 && tBlood > 0)
                {
                    text = Language.GetTextValue($"{baseTextPath}WorldIsHallowCrimsonLatex", Main.worldName, tGood, tBlood, tLatex);
                }
                else if (tEvil > 0 && tBlood > 0)
                {
                    text = Language.GetTextValue($"{baseTextPath}WorldIsCorruptCrimsonLatex", Main.worldName, tEvil, tBlood, tLatex);
                }
                else if (tEvil > 0)
                {
                    text = Language.GetTextValue($"{baseTextPath}WorldIsCorruptLatex", Main.worldName, tEvil, tLatex);
                }
                else if (tBlood > 0)
                {
                    text = Language.GetTextValue($"{baseTextPath}WorldIsCrimsonLatex", Main.worldName, tBlood, tLatex);
                }
                else if (tGood > 0)
                {
                    text = Language.GetTextValue($"{baseTextPath}WorldIsHallowLatex", Main.worldName, tGood, tLatex);
                }
                else
                {
                    text = Language.GetTextValue($"{baseTextPath}WorldIsLatex", Main.worldName, tLatex);
                }
            }
            else
            {
                // old
                if (tGood > 0 && tEvil > 0 && tBlood > 0)
                {
                    text = Language.GetTextValue("DryadSpecialText.WorldStatusAll", Main.worldName, tGood, tEvil, tBlood);
                }
                else if (tGood > 0 && tEvil > 0)
                {
                    text = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCorrupt", Main.worldName, tGood, tEvil);
                }
                else if (tGood > 0 && tBlood > 0)
                {
                    text = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCrimson", Main.worldName, tGood, tBlood);
                }
                else if (tEvil > 0 && tBlood > 0)
                {
                    text = Language.GetTextValue("DryadSpecialText.WorldStatusCorruptCrimson", Main.worldName, tEvil, tBlood);
                }
                else if (tEvil > 0)
                {
                    text = Language.GetTextValue("DryadSpecialText.WorldStatusCorrupt", Main.worldName, tEvil);
                }
                else if (tBlood > 0)
                {
                    text = Language.GetTextValue("DryadSpecialText.WorldStatusCrimson", Main.worldName, tBlood);
                }
                else if (tGood > 0)
                {
                    text = Language.GetTextValue("DryadSpecialText.WorldStatusHallow", Main.worldName, tGood);
                }
                else
                {
                    text = Language.GetTextValue("DryadSpecialText.WorldStatusPure", Main.worldName);
                    worldIsEntirelyPure = true;
                    return text;
                }
            }

            string arg;
            int evilTotal = tEvil + tBlood;
            double good = tGood;

            if (tLatex > tGood && tLatex > evilTotal)
            {
                if (tLatex >= 15)
                    arg = Language.GetTextValue($"{baseTextPath}NoteGooLargeAmount");
                else if (tLatex >= 5)
                    arg = Language.GetTextValue($"{baseTextPath}NoteGooSmallAmount");
                else
                    arg = Language.GetTextValue($"{baseTextPath}NoteGooNearlyGone");
            }
            else if (tLatex > tGood + 15 && tLatex > evilTotal + 15)
            {
                arg = Language.GetTextValue($"{baseTextPath}NoteGooLargeAmount");
            }
            else if (good * 1.2 >= evilTotal && good * 0.8 <= evilTotal)
            {
                arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced");
            }
            else if (tGood >= evilTotal)
            {
                arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionFairyTale");
            }
            else if (evilTotal > tGood + 20)
            {
                arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionGrim");
            }
            else if (evilTotal <= 5)
            {
                arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionClose");
            }
            else
            {
                arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionWork");
            }

            return text + " " + arg;
            /*
            string arg = (((double)tGood * 1.2 >= (double)(tEvil + tBlood) && (double)tGood * 0.8 <= (double)(tEvil + tBlood)) ? 
                Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced") : 
                ((tGood >= tEvil + tBlood) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionFairyTale") : 
                ((tEvil + tBlood > tGood + 20) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionGrim") : 
                ((tEvil + tBlood <= 5) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionClose") : 
                Language.GetTextValue("DryadSpecialText.WorldDescriptionWork")))));
            return text + " " + arg;
            */
        }

        private static void MyAssignRandomWorldName(On_UIWorldCreation.orig_AssignRandomWorldName orig, UIWorldCreation self)
        {
            if (!ModContent.GetInstance<ChangedSpecialModClientConfig>().CustomWorldNames)
            {
                orig(self);
                return;
            }

            MyAssignRandomWorldName(self);
            // Copy vanilla code here and use your own composition list.
        }

        private static readonly FieldInfo OptionWorldNameField =
            typeof(UIWorldCreation).GetField("_optionwWorldName",
                BindingFlags.Instance | BindingFlags.NonPublic);

        private static void MyAssignRandomWorldName(UIWorldCreation self)
        {
            string worldName;

            do
            {                
                LocalizedText composition = SelectWorldNamePartString("Composition");
                LocalizedText adjective = SelectWorldNamePartString("Adjective");
                LocalizedText location = SelectWorldNamePartString("Location");
                LocalizedText noun = SelectWorldNamePartString("Noun"); 

                var args = new
                {
                    Adjective = adjective.Value,
                    Location = location.Value,
                    Noun = noun.Value
                };

                worldName = composition.FormatWith(args);

                if (Main.rand.Next(10000) == 0)
                    worldName = Language.GetTextValue("SpecialWorldName.TheConstant");
            }
            while (worldName.Length > 27);

            OptionWorldNameField.SetValue(self, worldName);
        }

        private static LocalizedText SelectWorldNamePartString(string subject)
        {
            var defaultOptions = Language.FindAll(Lang.CreateDialogFilter($"RandomWorldName_{subject}."));
            var customOptions = Language.FindAll(Lang.CreateDialogFilter($"RandomWorldName_{subject}_Custom."));
            var allOptions = defaultOptions.ToList();
            allOptions.AddRange(customOptions.ToList());
            return Main.rand.Next(allOptions);
        } 

        private static int[] BalloonItems = new int[]
        {
            ItemID.YellowHorseshoeBalloon,
            ItemID.BalloonPufferfish,
            ItemID.BlizzardinaBalloon,
            ItemID.BlueHorseshoeBalloon,
            ItemID.BundleofBalloons,
            ItemID.HorseshoeBundle,
            ItemID.CloudinaBalloon,
            ItemID.FartInABalloon,
            ItemID.BalloonHorseshoeFart,
            ItemID.HoneyBalloon,
            ItemID.BalloonHorseshoeSharkron,
            ItemID.SandstorminaBalloon,
            ItemID.SharkronBalloon,
            ItemID.ShinyRedBalloon,
            ItemID.WhiteHorseshoeBalloon,
            ItemID.YellowHorseshoeBalloon,
            ItemID.PartyBalloonAnimal
        };

        private static Dictionary<int, int> NumberOfStylesPerItem = new Dictionary<int, int>
        {
            { TileID.PottedPlants1, 4 },
            { TileID.PottedPlants2, 8 },
            { ModContent.TileType<RedGasTank>(), 5 },
            { ModContent.TileType<BlueGasTank>(), 5 },
            { ModContent.TileType<CrystalWhite>(), 4 },
            { ModContent.TileType<CrystalGreen>(), 4 },
            { ModContent.TileType<CrystalRed>(), 4 },
            { ModContent.TileType<PillarWhite>(), 4 },
        };

        public static int GetNumberOfStylesPerItem(int itemId)
        {
            if (NumberOfStylesPerItem.ContainsKey(itemId))
                return NumberOfStylesPerItem[itemId];
            return 0;
        }

        public static void PlaceRandomTile(int i, int j, int tileId)
        {
            var nStyles = GetNumberOfStylesPerItem(tileId);
            WorldGen.PlaceObject(i, j - 1, tileId, true, ChangedUtils.WorldGenRandNext(0, nStyles), 0, -1, -1);
            NetMessage.SendTileSquare(-1, i, j - 1, TileChangeType.None);
        }

        private static int[] DefaultMergeBlocks = new int[]
        {
            TileID.Grass,
            TileID.CorruptGrass,
            TileID.CrimsonGrass,

            TileID.Stone,
            TileID.Ebonstone,
            TileID.Crimstone,

            TileID.GreenMoss,
            TileID.BrownMoss,
            TileID.RedMoss,
            TileID.BlueMoss,
            TileID.PurpleMoss,
            TileID.LavaMoss,
            TileID.KryptonMoss,
            TileID.XenonMoss,
            TileID.ArgonMoss,
            TileID.VioletMoss,
            TileID.RainbowMoss,

            TileID.Dirt,

            TileID.Sand,
            TileID.Sandstone,

            TileID.ClayBlock,
            TileID.Mud,
            TileID.SnowBlock,
            TileID.IceBlock,

            ModContent.TileType<DryDirt>(),
            ModContent.TileType<BlackLatexTile>(),
            ModContent.TileType<BlackLatexSandTile>(),
            ModContent.TileType<WhiteLatexTile>(),
            ModContent.TileType<Lab_TileTile>(),
        };

        public static void SetTileMerge(int tileType)
        {
            foreach (var block in DefaultMergeBlocks)
            {
                Main.tileMerge[block][tileType] = true;
                Main.tileMerge[tileType][block] = true;
            }
        }

        public static void DestroyTile(int xPos, int yPos)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            WorldGen.KillTile(xPos, yPos);

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, xPos, yPos, 0);
        }

        public static void PlaceTile(int xPos, int yPos, int tileType)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            WorldGen.PlaceTile(xPos, yPos, tileType);

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, xPos, yPos, tileType);
        }

        public static void SwitchAllNPCState(int npcType, int state)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                var npc = Main.npc[i];
                if (npc.type == npcType)
                {
                    npc.ai[0] = state; //State
                    npc.ai[1] = 0;     //Timer

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.SyncNPC, number: i);
                }
            }
        }

        public static int GetNumberOfNPCType(int npcType)
        {
            return Main.npc.Where(x => x.type == npcType && x.active).Count();
        }

        public static void CreateFlyingGasTank(int projectileType, int i, int j)
        {
            var player = Main.LocalPlayer;
            var source = WorldGen.GetItemSource_FromTileBreak(i, j);
            var xPos = i * 16 + 8;
            var yPos = (j + 1) * 16 + 8;
            Projectile.NewProjectile(source, xPos, yPos, 0, 0, projectileType, 0, 0, player.whoAmI, 0f, 0f);
        }

        public static void SpawnOranges(IEntitySource source, Player player, int x, int y)
        {
            var nCurrentOranges = Main.projectile.Where(x => x.active && x.type == ModContent.ProjectileType<OrangeProjectile>()).Count();
            if (nCurrentOranges > Main.maxProjectiles / 2)
                return;

            for (int i = 0; i < 30; i++)
            {
                //owner player.whoAmI
                var index = Projectile.NewProjectile(source, x * 16, y * 16, 0, 0, ModContent.ProjectileType<OrangeProjectile>(), 0, 0, -1, 0f, 0f);
                if (index >= 0 && index < Main.projectile.Length)
                {
                    var angle = Main.rand.NextFloat((float)Math.PI * 2);
                    Main.projectile[index].velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 5;
                }
            }
        }

        public static void DrawProjectileCentered(Projectile proj, Color lightColor, Texture2D texture = null, bool drawCentered = true)
        {
            if (texture is null)
                texture = TextureAssets.Projectile[proj.type].Value;

            int frameHeight = texture.Height / Main.projFrames[proj.type];
            int frameY = frameHeight * proj.frame;
            float scale = proj.scale;
            float rotation = proj.rotation;

            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (proj.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Vector2 startPos = drawCentered ? proj.Center : proj.position;
            Vector2 drawPos = startPos - Main.screenPosition + new Vector2(0f, proj.gfxOffY);

            Main.spriteBatch.Draw(texture, drawPos, rectangle, proj.GetAlpha(lightColor), rotation, origin, scale, spriteEffects, 0f);

        }

        public static bool IsDrunk(Player player)
        {
            if (player == null) 
                return false;
            return Main.drunkWorld || player.inventory.Any(item => item.type == ModContent.ItemType<RottenOrange>());
        }

        public static T Choose<T>(params T[] items)
        {
            if (items.Length == 0)
                return default!;
            return items[Main.rand.Next(0, items.Length)];
        }

        public static void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static float GetNPCValue(int platinum = 0, int gold = 0, int silver = 0, int copper = 0)
        {
            return copper + 100 * silver + 10000 * gold + 1000000 * platinum;
        }

        public static Player GetClosestPlayer(int x, int y, bool includeDead = false)
        {
            float distance = float.MaxValue;
            Player closestPlayer = null;
            var worldPos = new Vector2(x * 16, y * 16);
            foreach (var player in Main.player)
            {
                if (!player.active && !includeDead)
                    continue;
                var tmpDistance = Vector2.DistanceSquared(player.Center, worldPos);
                if (tmpDistance < distance)
                {
                    distance = tmpDistance;
                    closestPlayer = player;
                }
            }

            return closestPlayer;
        }

        public static int MainRandNext(int minValue, int maxValue)
        {
            if (maxValue <= minValue)
                maxValue = minValue + 1;
            // This is stupid. In all random functions it is exclusive maxValue but Utils.Next is inclusive maxValue
            return Utils.Next(Main.rand, new Terraria.Utilities.IntRange(minValue, maxValue - 1));
        }

        public static int WorldGenRandNext(int minValue, int maxValue)
        {
            if (maxValue <= minValue)
                maxValue = minValue + 1;
            // This is stupid. In all random functions it is exclusive maxValue but Utils.Next is inclusive maxValue
            return Utils.Next(WorldGen.genRand, new Terraria.Utilities.IntRange(minValue, maxValue - 1));
        }

        public static int[] GetRandomIntegerArray(int nElems, int minValue, int maxValue)
        {
            var arr = new int[nElems];
            for (var i = 0; i < nElems; i++)
                arr[i] = Utils.Next(WorldGen.genRand, new Terraria.Utilities.IntRange(minValue, maxValue));
            return arr;
        }

        private static void AddBossValue(List<float> numbers, bool condition, float bossValue)
        {
            if (condition)
                numbers.Add(bossValue);
        }

        private static float GetBossProgressionNumber()
        {
            var numbers = new List<float>() { 0 };

            // Vanilla vars
            AddBossValue(numbers, NPC.downedSlimeKing, 1);
            AddBossValue(numbers, NPC.downedBoss1, 2);
            AddBossValue(numbers, NPC.downedBoss2, 3);
            AddBossValue(numbers, NPC.downedQueenBee, 4);
            AddBossValue(numbers, NPC.downedBoss3, 5);
            AddBossValue(numbers, NPC.downedDeerclops, 6);
            AddBossValue(numbers, Main.hardMode, 7);
            AddBossValue(numbers, NPC.downedQueenSlime, 8);
            AddBossValue(numbers, NPC.downedMechBoss2, 9);
            AddBossValue(numbers, NPC.downedMechBoss1, 10);
            AddBossValue(numbers, NPC.downedMechBoss3, 11);
            AddBossValue(numbers, NPC.downedPlantBoss, 12);
            AddBossValue(numbers, NPC.downedGolemBoss, 13);
            AddBossValue(numbers, NPC.downedFishron, 14);
            AddBossValue(numbers, NPC.downedEmpressOfLight, 15);
            // 16 is for betsy, but it is not tracked
            AddBossValue(numbers, NPC.downedAncientCultist, 17);
            AddBossValue(numbers, NPC.downedMoonlord, 18);

            // Our mod vars
            AddBossValue(numbers, DownedBossSystem.DownedWhiteTail, 1.5f);
            AddBossValue(numbers, DownedBossSystem.DownedWolfKing, 2.5f);
            AddBossValue(numbers, DownedBossSystem.DownedBehemoth, 5.5f);

            // Other mods
            AddBosses(numbers, CalamityBossValues, ModSupportSystem.modCalamity);
            AddBosses(numbers, CoraliteBossValues, ModSupportSystem.modCoralite);
            AddBosses(numbers, ThoriumBossValues, ModSupportSystem.modThorium, "GetDownedBoss");

            return numbers.Max();
        }

        private static readonly Dictionary<string, float> CalamityBossValues = new()
        {
            { "DesertScourge", 1.6f },
            { "GiantClam", 1.61f },
            { "AcidRainT1", 2.67f },
            { "Crabulon", 2.7f },
            { "HiveMind", 3.98f },
            { "Perforators", 3.99f },
            { "SlimeGod", 6.7f },
            { "Cryogen", 8.5f },
            { "AquaticScourge", 9.5f },
            { "AcidRainT2", 9.51f },
            { "CragmawMire", 9.52f },
            { "BrimstoneElemental", 10.5f },
            { "CalamitasClone", 11.7f },
            { "GreatSandShark", 12.09f },
            { "Leviathan", 12.8f },
            { "AstrumAureus", 12.81f },
            { "PlaguebringerGoliath", 14.5f },
            { "Ravager", 16.5f },
            { "AstrumDeus", 17.5f },
            { "ProfanedGuardians", 18.5f },
            { "Dragonfolly", 18.6f },
            { "Providence", 19f },
            { "CeaselessVoid", 19.6f },
            { "StormWeaver", 19.61f },
            { "Signus", 19.62f },
            { "Polterghast", 20f },
            { "AcidRainT3", 20.49f },
            { "Mauler", 20.491f },
            { "NuclearTerror", 20.492f },
            { "OldDuke", 20.5f },
            { "DevourerofGods", 21f },
            { "Yharon", 22f },
            { "ExoMechs", 22.99f },
            { "Calamitas", 23f },
            { "BossRush", 25.99f }
        };

        private static readonly Dictionary<string, float> CoraliteBossValues = new()
        {
            { "rediancie", 0.9f },
            { "babyicedragon", 3.1f },
            { "slimeemperor", 3.2f },
            { "bloodiancie", 8.2f },
            { "thunderveindragon", 11.1f },
            { "zacurrentdragon", 15.1f },
            { "nightmareplantera", 18.1f }
        };

        private static readonly Dictionary<string, float> ThoriumBossValues = new()
        {
            // TODO Figure these out
            { "TheGrandThunderBird", 0.1f },
            { "QueenJellyfish", 0.1f },
            { "Viscount", 0.1f },
            { "BoreanStrider", 0.1f },
            { "FallenBeholder", 0.1f },
            { "ForgottenOne", 0.1f },
            { "PatchWerk", 0.1f },
            { "CorpseBloom", 0.1f },
            { "Illusionist", 0.1f },

            { "GraniteEnergyStorm", 6.4f },
            { "BuriedChampion", 6.5f },
            { "StarScouter", 6.9f },
            { "Lich", 11.6f },
            { "ThePrimordials", 19.5f }
        };


        private static void AddBosses(List<float> bossNumberList, Dictionary<string, float> bosses, Mod mod, string bossCheckMethodName = "BossDowned")
        {
            if (mod == null)
                return;
            foreach (var (bossName, bossValue) in bosses)
            {
                if ((bool)mod.Call(bossCheckMethodName, bossName))
                    bossNumberList.Add(bossValue);
            }
        }

        public static bool CanSpawn(SpawnRequirement spawnRequirement)
        {
            var bossProgression = GetBossProgressionNumber();

            switch (spawnRequirement)
            {
                case SpawnRequirement.None:
                    return true;
                case SpawnRequirement.WhiteTail:
                    return bossProgression >= 1.5f;
                case SpawnRequirement.WolfKing:
                    return bossProgression >= 2.5f;
                case SpawnRequirement.Behemoth:
                    return bossProgression >= 5.5f;
            }
            return false;
        }

        public static bool CorrectDepth(ChangedNPC npc, Player player)
        {
            if (npc == null || player == null)
                return false;

            var underground = player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight;

            switch(npc.spawnDepth)
            {
                case SpawnDepth.Everywhere:
                    return true;
                case SpawnDepth.Surface:
                    return !underground;
                case SpawnDepth.Cave:
                    return underground;
            }

            return false;
        }

        public static bool CanSpawnExtraStrong()
        {
            // Skeletron or others if you skipped him
            var downedVanilla = NPC.downedBoss3 || Main.hardMode;
            return downedVanilla || DownedBossSystem.DownedBehemoth;
        }

        public static bool PlayerIsWearingBalloon(Player player)
        {
            return player.armor.Any(item => BalloonItems.Contains(item.type));
        }

        public static bool PlayerIsWearingWeddingDress(Player player)
        {
            return (player.armor.Any(item => item.type == ItemID.TheBrideHat) &&
                player.armor.Any(item => item.type == ItemID.TheBrideDress));
        }

        public static bool IsBlackLatexWall(Tile tile)
        {
            return tile.WallType == WallID.Slime && tile.WallColor == PaintID.BlackPaint;
        }

        public static bool IsWhiteLatexWall(Tile tile)
        {
            return tile.WallType == WallID.Slime && tile.WallColor == PaintID.WhitePaint;
        }

        public static bool HasWolfKingFightStarted()
        {
            var wolfKingSpawn = ModContent.NPCType<WolfKingSpawn>();
            var wolfKing = ModContent.NPCType<WolfKing>();

            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == wolfKingSpawn)
                {
                    // Not the idle state
                    return npc.ai[0] != 0;
                }
                else if (npc.type == wolfKing)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsInInvasionZone(Player player)
        {
            if (player == null)
                return false;

            // Invasion active and player on the surface
            if (player.active && Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0 && ((double)player.position.Y < Main.worldSurface * 16.0 + (double)NPC.sHeight || Main.remixWorld))
            {
                int num7 = 3000;
                // In the invasion zone
                if ((double)player.position.X > Main.invasionX * 16.0 - (double)num7 && (double)player.position.X < Main.invasionX * 16.0 + (double)num7)
                {
                    return true;
                }
                // On the edge of the invasion zone
                else if (Main.invasionX >= (double)(Main.maxTilesX / 2 - 5) && Main.invasionX <= (double)(Main.maxTilesX / 2 + 5))
                {
                    for (int l = 0; l < 200; l++)
                    {
                        if (Main.npc[l].townNPC && Math.Abs(player.position.X - Main.npc[l].Center.X) < (float)num7)
                        {
                            if (Main.rand.Next(3) != 0)
                            {
                                return true;
                            }
                            break;
                        }
                    }
                }
            }

            // Player is in a lunar pillar zone
            if (player.ZoneTowerSolar || player.ZoneTowerNebula || player.ZoneTowerVortex || player.ZoneTowerStardust)
            {
                return true;
            }

            return false;
        }

        public static bool IsBossAlive()
        {
            foreach (var npc in Main.npc)
            {
                if (npc.active)
                    continue;

                // Return true if the boss is not a pillar
                if (npc.boss && 
                    npc.type != NPCID.LunarTowerNebula && npc.type != NPCID.LunarTowerSolar && 
                    npc.type != NPCID.LunarTowerStardust && npc.type != NPCID.LunarTowerVortex)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ShouldDisableVanillaSpawn(Player player)
        {
            if (player == null)
                return false;

            var isInInvasionZone = IsInInvasionZone(player);
            var pumpkinMoon = Main.pumpkinMoon;
            var frostMoon = Main.snowMoon;
            var solarEclipse = Main.eclipse && Main.IsItDay();
            var DD2EventActive = (DD2Event.Ongoing && player.ZoneOldOneArmy);
            var nearMeteor = player.ZoneMeteor;
            var bossAlive = IsBossAlive();

            return (!isInInvasionZone && !pumpkinMoon && !frostMoon && !solarEclipse && !DD2EventActive && !nearMeteor && !bossAlive);
        }

        public static bool NoInvasionOrEvent()
        {
            return Main.invasionType == 0 && !(Main.CurrentFrameFlags.AnyActiveBossNPC && !NPC.LunarApocalypseIsUp);
        }

        public static bool IsItWindy()
        {
            return Main.windSpeedCurrent < -0.4 || Main.windSpeedCurrent  > 0.4;
        }

        public static float GetWeatherSpawnChance(ElementType elementType)
        {
            var highChance = 2.0f;
            var defaultChance = 1.0f;
            var lowChance = 0.8f;
            var isRaining = Main.IsItRaining;
            var isWindy = IsItWindy();

            if (elementType == ElementType.Water)
                return isRaining ? highChance : lowChance;

            if (elementType == ElementType.Wind)
            {
                lowChance = 1.0f;
                highChance = 4.0f;
                return isWindy ? highChance : lowChance;
            }

            if (elementType == ElementType.None)
                return (isRaining || isWindy) ? lowChance : defaultChance;

            return defaultChance;
        }

        /// <summary>
        /// Used by strong monsters that can rarely spawn, or much more commonly when close or inside the world evil biome
        /// </summary>
        /// <param name="spawnInfo"></param>
        /// <param name="npc"></param>
        /// <param name="NPCType"></param>
        /// <param name="crimson"></param>
        /// <returns></returns>
        public static float GetWorldEvilSpawnChance(NPCSpawnInfo spawnInfo, NPC npc, int NPCType, bool crimson = false)
        {
            if (!CanSpawn(SpawnRequirement.WolfKing) || NPC.AnyNPCs(NPCType))
                return 0;

            if (!CorrectDepth(npc.Changed(), spawnInfo.Player))
                return 0;

            var multiplier = 0.2f;
            var multiplierRightEvilBiomeLow = 0.2f;
            var multiplierRightEvilBiomeHigh = 0.4f;
            var multiplierWrongEvilBiomeLow = 0.1f;
            var multiplierWrongEvilBiomeHigh = 0.2f;

            // If the player made an artificial evil biome that is normally not in the world and is now in both evil biomes
            if (spawnInfo.Player.ZoneCorrupt && spawnInfo.Player.ZoneCrimson)
                multiplier = multiplierRightEvilBiomeHigh;
            // Corruption biome
            else if (spawnInfo.Player.ZoneCorrupt)
                multiplier = !crimson ? multiplierRightEvilBiomeHigh : multiplierWrongEvilBiomeHigh;
            // Crimson biome
            else if (spawnInfo.Player.ZoneCrimson)
                multiplier = crimson ? multiplierRightEvilBiomeHigh : multiplierWrongEvilBiomeHigh;
            // Corrupt world and not in an evil biome
            else if (!WorldGen.crimson)
                multiplier = !crimson ? multiplierRightEvilBiomeLow : multiplierWrongEvilBiomeLow;
            // Crimson world and not in an evil biome
            else
                multiplier = crimson ? multiplierRightEvilBiomeLow : multiplierWrongEvilBiomeLow;

            var changedNPC = npc.Changed();
            return multiplier * GetSurfaceSpawnChance(spawnInfo, changedNPC, npc.type);
        }

        private static Tile GetTile(NPCSpawnInfo info)
        {
            return Main.tile[info.SpawnTileX, info.SpawnTileY];
        }

        public static float GetSurfaceSpawnChance(NPCSpawnInfo spawnInfo, ChangedNPC changedNPC, int NPCID)
        {
            var canSpawn = CanSpawn(changedNPC.spawnRequirement);
            if (!canSpawn)
                return 0;

            if (!CorrectDepth(changedNPC, spawnInfo.Player))
                return 0;

            var environmentSpawnChance = GetSurfaceEnvironmentSpawnChance(spawnInfo, changedNPC);
            var weatherSpawnChance = GetWeatherSpawnChance(changedNPC.ElementType);
            return environmentSpawnChance * weatherSpawnChance;
        }

        public static float GetUndergroundSpawnChance(NPCSpawnInfo spawnInfo, ChangedNPC npc, int NPCID)
        {
            var canSpawn = CanSpawn(npc.spawnRequirement);
            if (!canSpawn)
                return 0;

            var player = spawnInfo.Player;
            var gooType = npc.GooType;

            // Spawn tile checks
            var tileType = GetTile(spawnInfo).TileType;
            var spawnTileIsBlackLatexTile = tileType == ModContent.TileType<BlackLatexTile>();
            var spawnTileIsWhiteLatexTile = tileType == ModContent.TileType<WhiteLatexTile>();
            var spawnTileIsDryDirt = tileType == ModContent.TileType<DryDirt>();
            var spawnTileIsWater = spawnInfo.Water;
            var correctTileType = false;

            // Player in Changed biome checks
            var playerIsInBlackGooZone = InBlackLatexUndergroundBiome(player);
            var playerIsInWhiteGooZone = InWhiteLatexUndergroundBiome(player);
            var inBiome = false;

            if (gooType == GooType.None)
            {
                inBiome = playerIsInBlackGooZone || playerIsInWhiteGooZone;
                correctTileType = spawnTileIsBlackLatexTile || spawnTileIsWhiteLatexTile || spawnTileIsDryDirt;
            }
            else if (gooType == GooType.Black)
            {
                inBiome = playerIsInBlackGooZone;
                correctTileType = spawnTileIsBlackLatexTile;
            }
            else if (gooType == GooType.BlackOnly)
            {
                inBiome = playerIsInBlackGooZone;
                correctTileType = spawnTileIsBlackLatexTile;
            }
            else if (gooType == GooType.White)
            {
                inBiome = playerIsInWhiteGooZone;
                correctTileType = spawnTileIsWhiteLatexTile;
            }
            else if (gooType == GooType.WhiteOnly)
            {
                inBiome = playerIsInWhiteGooZone;
                correctTileType = spawnTileIsWhiteLatexTile;
            }

            // Custom check for fish
            // It has to be in the water and inside the biome and doesn't have a reduced change to appear elsewhere
            if (npc.IsFish)
            {
                if (inBiome && spawnTileIsWater)
                    return 4.0f;//1
                return 0.0f;
            }

            // Normal chance if inside biome
            if (inBiome)
                return 1.0f;
            // Lower chance if standing at the edge or if one of the biome blocks is close
            else if (correctTileType)
                return 0.8f;

            return 0.0f;
        }

        public static float GetSurfaceEnvironmentSpawnChance(NPCSpawnInfo spawnInfo, ChangedNPC npc)
        {
            var player = spawnInfo.Player;
            var gooType = npc.GooType;

            // Spawn tile checks
            var tileType = GetTile(spawnInfo).TileType;
            var spawnTileIsBlackLatexTile = tileType == ModContent.TileType<BlackLatexTile>();
            var spawnTileIsWhiteLatexTile = tileType == ModContent.TileType<WhiteLatexTile>();
            var spawnTileIsDryDirt = tileType == ModContent.TileType<DryDirt>();
            var spawnTileIsWater = spawnInfo.Water;
            var correctTileType = false;

            // Player in Changed biome checks
            var playerIsInBlackGooZone =  InBlackLatexBiome(player);
            var playerIsInWhiteGooZone = InWhiteLatexBiome(player);
            var playerIsInCityRuinsZone = InCityRuinsBiome(player);
            var playerIsInChangedBiome = InChangedSurfaceBiome(player);
            var inBiome = false;

            if (gooType == GooType.None)
            {
                inBiome = playerIsInChangedBiome;
                correctTileType = spawnTileIsBlackLatexTile || spawnTileIsWhiteLatexTile || spawnTileIsDryDirt;
            }
            else if (gooType == GooType.Black)
            {
                inBiome = playerIsInBlackGooZone || playerIsInCityRuinsZone;
                correctTileType = spawnTileIsBlackLatexTile;
            }
            else if (gooType == GooType.BlackOnly)
            {
                inBiome = playerIsInBlackGooZone;
                correctTileType = spawnTileIsBlackLatexTile;
            }
            else if (gooType == GooType.White)
            {
                inBiome = playerIsInWhiteGooZone || playerIsInCityRuinsZone;
                correctTileType = spawnTileIsWhiteLatexTile;
            }
            else if (gooType == GooType.WhiteOnly)
            {
                inBiome = playerIsInWhiteGooZone;
                correctTileType = spawnTileIsWhiteLatexTile;
            }

            // Custom check for fish
            // It has to be in the water and inside the biome and doesn't have a reduced change to appear elsewhere
            if (npc.IsFish)
            {
                if (inBiome && spawnTileIsWater)
                    return 4.0f;//1
                return 0.0f;
            }

            // Normal chance if inside biome
            if (inBiome)
                return 1.0f;
            // Lower chance if standing at the edge or if one of the biome blocks is close
            else if (correctTileType)
                return 0.8f;

            return 0.0f;
        }

        public static int GetBestiaryKillCount(int NPCID)
        {
            if (ContentSamples.NpcPersistentIdsByNetIds.ContainsKey(NPCID))
            {
                var persistentId = ContentSamples.NpcPersistentIdsByNetIds[NPCID];
                return Main.BestiaryTracker.Kills.GetKillCount(persistentId);
            }

            return 0;
        }

        public static void HideFromBestiary(ModNPC n)
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(n.Type, value);
        }

        #region DoorChecks

        public static bool IsDoorAtTile(int tileCheckX, int tileCheckY)
        {
            var tile = Main.tile[tileCheckX, tileCheckY];
            return tile.HasUnactuatedTile && (tile.TileType == TileID.ClosedDoor || tile.TileType == ModContent.TileType<LabDoorClosed>());
        }

        public static bool IsOpenDoorAtTile(int tileCheckX, int tileCheckY)
        {
            var tile = Main.tile[tileCheckX, tileCheckY];
            return tile.HasUnactuatedTile && (tile.TileType == TileID.OpenDoor || tile.TileType == ModContent.TileType<LabDoorOpen>());
        }

        public static bool IsTallGateAtTile(int tileCheckX, int tileCheckY)
        {
            var tile = Main.tile[tileCheckX, tileCheckY];
            return tile.HasUnactuatedTile && tile.TileType == TileID.TallGateClosed;
        }

        public static bool IsCrystalAtTile(int tileCheckX, int tileCheckY)
        {
            var tile = Main.tile[tileCheckX, tileCheckY];
            return tile.TileType == ModContent.TileType<CrystalWhite>() ||
                tile.TileType == ModContent.TileType<CrystalGreen>() ||
                tile.TileType == ModContent.TileType<CrystalRed>();
        }

        #endregion

        public static bool IsPlayerNearPuro(Player player)
        {
            return IsPlayerNearNPC(player, ModContent.NPCType<Puro>());
        }

        public static bool IsPlayerNearPrototype(Player player)
        {
            return IsPlayerNearNPC(player, ModContent.NPCType<Prototype>());
        }

        public static bool IsPlayerNearNPC(Player player, int npcType)
        {
            if (player == null)
                return false;

            NPC tmpNpc = null;
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == npcType)
                {
                    tmpNpc = npc;
                    break;
                }
            }

            if (tmpNpc != null)
            {
                var minDist = 200 * 16;
                // Not sure if distance can be negative, and this is faster anyways because it doesnt have to calculate the square root
                if (Vector2.DistanceSquared(player.position, tmpNpc.position) < minDist * minDist)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the player is grappling with a hook
        /// </summary>
        /// <param name="player">The player to check</param>
        /// <returns></returns>
        public static bool IsPlayerGrappling(Player player)
        {
            if (player == null) 
                return false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active &&
                    proj.owner == player.whoAmI &&
                    proj.aiStyle == ProjAIStyleID.Hook)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Cleaned up version of the decompiled code, with lighting effects and other unnecessary parts removed
        /// </summary>
        /// <param name="npc"></param>
        public static void AI_018_JellyFish(NPC npc)
        {
            bool flag12 = false;
            if (npc.wet && npc.ai[1] == 1f)
            {
                flag12 = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }
            float num271 = 1f;
            if (flag12)
            {
                num271 += 0.5f;
            }
            if (npc.direction == 0)
            {
                npc.TargetClosest();
            }
            if (flag12)
            {
                return;
            }
            if (npc.wet)
            {
                int num272 = (int)npc.Center.X / 16;
                int num273 = (int)(npc.position.Y + (float)npc.height) / 16;
                if (Main.tile[num272, num273].TopSlope)
                {
                    if (Main.tile[num272, num273].LeftSlope)
                    {
                        npc.direction = -1;
                        npc.velocity.X = Math.Abs(npc.velocity.X) * -1f;
                    }
                    else
                    {
                        npc.direction = 1;
                        npc.velocity.X = Math.Abs(npc.velocity.X);
                    }
                }
                else if (Main.tile[num272, num273 + 1].TopSlope)
                {
                    if (Main.tile[num272, num273 + 1].LeftSlope)
                    {
                        npc.direction = -1;
                        npc.velocity.X = Math.Abs(npc.velocity.X) * -1f;
                    }
                    else
                    {
                        npc.direction = 1;
                        npc.velocity.X = Math.Abs(npc.velocity.X);
                    }
                }
                if (npc.collideX)
                {
                    npc.velocity.X *= -1f;
                    npc.direction *= -1;
                }
                if (npc.collideY)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y = Math.Abs(npc.velocity.Y) * -1f;
                        npc.directionY = -1;
                        npc.ai[0] = -1f;
                    }
                    else if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y = Math.Abs(npc.velocity.Y);
                        npc.directionY = 1;
                        npc.ai[0] = 1f;
                    }
                }
                bool flag13 = false;
                if (!npc.friendly)
                {
                    npc.TargetClosest(faceTarget: false);
                    if (Main.player[npc.target].wet && !Main.player[npc.target].dead && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    {
                        flag13 = true;
                    }
                }
                if (flag13)
                {
                    npc.localAI[2] = 1f;
                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                    npc.velocity *= 0.98f;
                    float num274 = 0.2f;
                    if (npc.velocity.X > 0f - num274 && npc.velocity.X < num274 && npc.velocity.Y > 0f - num274 && npc.velocity.Y < num274)
                    {
                        npc.TargetClosest();
                        float num275 = 7f;
                        Vector2 vector31 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float num276 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector31.X;
                        float num277 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector31.Y;
                        float num278 = (float)Math.Sqrt(num276 * num276 + num277 * num277);
                        num278 = num275 / num278;
                        num276 *= num278;
                        num277 *= num278;
                        npc.velocity.X = num276;
                        npc.velocity.Y = num277;
                    }
                    return;
                }
                npc.localAI[2] = 0f;
                npc.velocity.X += (float)npc.direction * 0.02f;
                npc.rotation = npc.velocity.X * 0.4f;
                if (npc.velocity.X < -1f || npc.velocity.X > 1f)
                {
                    npc.velocity.X *= 0.95f;
                }
                if (npc.ai[0] == -1f)
                {
                    npc.velocity.Y -= 0.01f;
                    if (npc.velocity.Y < -1f)
                    {
                        npc.ai[0] = 1f;
                    }
                }
                else
                {
                    npc.velocity.Y += 0.01f;
                    if (npc.velocity.Y > 1f)
                    {
                        npc.ai[0] = -1f;
                    }
                }
                int num279 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
                int num280 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
                /*
                if (Main.tile[num279, num280 - 1] == null)
                {
                    Main.tile[num279, num280 - 1] = default(Tile);
                }
                if (Main.tile[num279, num280 + 1] == null)
                {
                    Main.tile[num279, num280 + 1] = default(Tile);
                }
                if (Main.tile[num279, num280 + 2] == null)
                {
                    Main.tile[num279, num280 + 2] = default(Tile);
                }
                */
                if (Main.tile[num279, num280 - 1].LiquidAmount > 128)
                {
                    if (Main.tile[num279, num280 + 1].HasTile)
                    {
                        npc.ai[0] = -1f;
                    }
                    else if (Main.tile[num279, num280 + 2].HasTile)
                    {
                        npc.ai[0] = -1f;
                    }
                }
                else
                {
                    npc.ai[0] = 1f;
                }
                if ((double)npc.velocity.Y > 1.2 || (double)npc.velocity.Y < -1.2)
                {
                    npc.velocity.Y *= 0.99f;
                }
                return;
            }
            npc.rotation += npc.velocity.X * 0.1f;
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X *= 0.98f;
                if ((double)npc.velocity.X > -0.01 && (double)npc.velocity.X < 0.01)
                {
                    npc.velocity.X = 0f;
                }
            }
            npc.velocity.Y += 0.2f;
            if (npc.velocity.Y > 10f)
            {
                npc.velocity.Y = 10f;
            }
            npc.ai[0] = 1f;
            return;
        }

        public static void AI_067_FreakingPirates(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            bool isTabbyOrPurrpurr = 
                projectile.type == ModContent.ProjectileType<PurrpurrStaffProjectile>() || 
                projectile.type == ModContent.ProjectileType<TabbyStaffProjectile>();
            int targetingRange = 450;

            var playerPosition = player.position;
            var playerCenter = player.Center;

            bool testFlag = false;

            if (isTabbyOrPurrpurr)
            {
                targetingRange = 600;
                // Fix so they won't keep spinning inside the terrain
                playerPosition.Y -= 30;
                playerCenter.Y -= 30;
            }

            float flyDistance = 500f;
            float walkDistance = 300f;

            // vector is the position when standing next to the player
            Vector2 vector = playerCenter;
            vector.X -= (45 + player.width / 2) * player.direction;
            vector.X -= projectile.minionPos * 30 * player.direction;


            projectile.shouldFallThrough = playerPosition.Y + (float)player.height - 12f > projectile.position.Y + (float)projectile.height;
            projectile.friendly = false;
            int num8 = 0;
            int num9 = 15;
            int attackTarget = -1;
            projectile.friendly = true;

            bool walkState = projectile.ai[0] == 0f;
            bool flyState = projectile.ai[0] == 1f;

            if (walkState)
            {
                projectile.Minion_FindTargetInRange(targetingRange, ref attackTarget, skipIfCannotHitWithOwnBody: true, delegate { return true; });
            }

            float playerDistance;
            float myDistance;
            bool closerIsMe;
            
            // Flying towards player
            if (flyState)
            {
                projectile.tileCollide = false;
                float acceleration = 0.2f;
                float num18 = 10f;
                int num19 = 200;
                if (num18 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                {
                    num18 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                }
                Vector2 distanceToPlayerVector = playerCenter - projectile.Center;
                float distanceToPlayer = distanceToPlayerVector.Length();
                // Teleport to player if very far away
                if (distanceToPlayer > 2000f)
                {
                    projectile.position = playerCenter - new Vector2(projectile.width, projectile.height) / 2f;
                }

                if (distanceToPlayer < (float)num19 && player.velocity.Y == 0f && 
                    projectile.position.Y + (float)projectile.height <= player.position.Y + (float)player.height && 
                    !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    //testFlag = true;
                    // Switch to walk state
                    projectile.ai[0] = 0f;
                    projectile.netUpdate = true;
                    if (projectile.velocity.Y < -6f)
                    {
                        projectile.velocity.Y = -6f;
                    }
                }
                if (distanceToPlayer >= 60f)
                {
                    distanceToPlayerVector.Normalize();
                    distanceToPlayerVector *= num18;
                    if (projectile.velocity.X < distanceToPlayerVector.X)
                    {
                        projectile.velocity.X += acceleration;
                        if (projectile.velocity.X < 0f)
                        {
                            projectile.velocity.X += acceleration * 1.5f;
                        }
                    }
                    if (projectile.velocity.X > distanceToPlayerVector.X)
                    {
                        projectile.velocity.X -= acceleration;
                        if (projectile.velocity.X > 0f)
                        {
                            projectile.velocity.X -= acceleration * 1.5f;
                        }
                    }
                    if (projectile.velocity.Y < distanceToPlayerVector.Y)
                    {
                        projectile.velocity.Y += acceleration;
                        if (projectile.velocity.Y < 0f)
                        {
                            projectile.velocity.Y += acceleration * 1.5f;
                        }
                    }
                    if (projectile.velocity.Y > distanceToPlayerVector.Y)
                    {
                        projectile.velocity.Y -= acceleration;
                        if (projectile.velocity.Y > 0f)
                        {
                            projectile.velocity.Y -= acceleration * 1.5f;
                        }
                    }
                }
                // Set sprite direction
                if (projectile.velocity.X != 0f)
                {
                    projectile.spriteDirection = Math.Sign(projectile.velocity.X);
                }
                projectile.frameCounter++;
                if (projectile.frameCounter > 3)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame < 2 || projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 2;
                }
                // Spin
                projectile.rotation = projectile.rotation.AngleTowards(projectile.rotation + 0.25f * (float)projectile.spriteDirection, 0.25f);
            }
            if (projectile.ai[0] == 2f && projectile.ai[1] < 0f)
            {
                projectile.friendly = false;
                projectile.ai[1] += 1f;
                if (num9 >= 0)
                {
                    projectile.ai[1] = 0f;
                    // Switch to walk state
                    projectile.ai[0] = 0f;
                    projectile.netUpdate = true;
                    return;
                }
            }
            else if (projectile.ai[0] == 2f)
            {
                projectile.spriteDirection = projectile.direction;
                projectile.rotation = 0f;

                projectile.velocity.Y += 0.4f;
                if (projectile.velocity.Y > 10f)
                {
                    projectile.velocity.Y = 10f;
                }
                projectile.ai[1] -= 1f;
                if (projectile.ai[1] <= 0f)
                {
                    if (num8 <= 0)
                    {
                        projectile.ai[1] = 0f;
                        // Switch to walk state
                        projectile.ai[0] = 0f;
                        projectile.netUpdate = true;
                        return;
                    }
                    projectile.ai[1] = -num8;
                }
            }

            // Has a target
            if (attackTarget >= 0)
            {
                float maxDistance2 = targetingRange;

                NPC nPC2 = Main.npc[attackTarget];
                Vector2 targetCenter = nPC2.Center;
                vector = targetCenter;
                if (projectile.IsInRangeOfMeOrMyOwner(nPC2, maxDistance2, out myDistance, out playerDistance, out closerIsMe))
                {
                    projectile.shouldFallThrough = nPC2.Center.Y > projectile.Bottom.Y;
                    bool grounded = projectile.velocity.Y == 0f;
                    if (projectile.wet && projectile.velocity.Y > 0f && !projectile.shouldFallThrough)
                    {
                        grounded = true;
                    }
                    // Jump if grounded
                    if (targetCenter.Y < projectile.Center.Y - 30f && grounded)
                    {
                        float num26 = (targetCenter.Y - projectile.Center.Y) * -1f;
                        float num27 = 0.4f;
                        float num28 = (float)Math.Sqrt(num26 * 2f * num27);
                        // Clamp jump height
                        if (num28 > 26f)
                        {
                            num28 = 26f;
                        }
                        projectile.velocity.Y = 0f - num28;
                    }
                }
            }
            // Walk state, no target
            if (projectile.ai[0] == 0f && attackTarget < 0)
            {
                // Fly if the player is using rocket boots
                if (Main.player[projectile.owner].rocketDelay2 > 0)
                {
                    // Switch to fly state
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                }
                Vector2 vectorToPlayer = playerCenter - projectile.Center;
                // Teleport to player if very far away
                if (vectorToPlayer.Length() > 2000f)
                {
                    projectile.position = playerCenter - new Vector2(projectile.width, projectile.height) / 2f;
                }
                else if (vectorToPlayer.Length() > flyDistance || Math.Abs(vectorToPlayer.Y) > walkDistance)
                {
                    // Switch to fly state
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                    if (projectile.velocity.Y > 0f && vectorToPlayer.Y < 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    if (projectile.velocity.Y < 0f && vectorToPlayer.Y > 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                }
            }
            // Walk state
            if (projectile.ai[0] == 0f)
            {
                // No target
                if (attackTarget < 0)
                {
                    if (projectile.Distance(playerCenter) > 60f && projectile.Distance(vector) > 60f && 
                        Math.Sign(vector.X - playerCenter.X) != Math.Sign(projectile.Center.X - playerCenter.X))
                    {
                        vector = playerCenter;
                    }
                    Rectangle r = Utils.CenteredRectangle(vector, projectile.Size);
                    for (int i = 0; i < 20; i++)
                    {
                        if (Collision.SolidCollision(r.TopLeft(), r.Width, r.Height))
                        {
                            break;
                        }
                        r.Y += 16;
                        vector.Y += 16f;
                    }
                    Vector2 vector8 = Collision.TileCollision(playerCenter - projectile.Size / 2f, vector - playerCenter, projectile.width, projectile.height);
                    vector = playerCenter - projectile.Size / 2f + vector8;
                    if (projectile.Distance(vector) < 32f)
                    {
                        float num32 = playerCenter.Distance(vector);
                        if (playerCenter.Distance(projectile.Center) < num32)
                        {
                            vector = projectile.Center;
                        }
                    }
                    Vector2 vector9 = playerCenter - vector;
                    if (vector9.Length() > flyDistance || Math.Abs(vector9.Y) > walkDistance)
                    {
                        Rectangle r2 = Utils.CenteredRectangle(playerCenter, projectile.Size);
                        Vector2 vector10 = vector - playerCenter;
                        Vector2 vector11 = r2.TopLeft();
                        for (float num33 = 0f; num33 < 1f; num33 += 0.05f)
                        {
                            Vector2 vector12 = r2.TopLeft() + vector10 * num33;
                            if (Collision.SolidCollision(r2.TopLeft() + vector10 * num33, r.Width, r.Height))
                            {
                                break;
                            }
                            vector11 = vector12;
                        }
                        vector = vector11 + projectile.Size / 2f;
                    }
                }
                projectile.tileCollide = true;
                float num34 = 0.5f;
                float num35 = 4f;
                float num36 = 4f;
                float num37 = 0.1f;
                if (attackTarget != -1)
                {
                    num34 = 0.65f;
                    num35 = 5.5f;
                    num36 = 5.5f;
                }
                if (num36 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                {
                    num36 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                    num34 = 0.7f;
                }

                int num39 = 0;
                bool flag13 = false;
                float num40 = vector.X - projectile.Center.X;
                Vector2 vector13 = vector - projectile.Center;
                if (Math.Abs(num40) > 5f)
                {
                    if (num40 < 0f)
                    {
                        num39 = -1;
                        if (projectile.velocity.X > 0f - num35)
                        {
                            projectile.velocity.X -= num34;
                        }
                        else
                        {
                            projectile.velocity.X -= num37;
                        }
                    }
                    else
                    {
                        num39 = 1;
                        if (projectile.velocity.X < num35)
                        {
                            projectile.velocity.X += num34;
                        }
                        else
                        {
                            projectile.velocity.X += num37;
                        }
                    }

                    bool flag14 = true;
                    flag14 = attackTarget > -1 && Main.npc[attackTarget].Hitbox.Intersects(projectile.Hitbox);

                    if (flag14)
                    {
                        flag13 = true;
                    }
                }
                else
                {
                    projectile.velocity.X *= 0.9f;
                    if (Math.Abs(projectile.velocity.X) < num34 * 2f)
                    {
                        projectile.velocity.X = 0f;
                    }
                }
                bool flag15 = Math.Abs(vector13.X) >= 64f || (vector13.Y <= -48f && Math.Abs(vector13.X) >= 8f);
                if (num39 != 0 && flag15)
                {
                    int num41 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                    int num42 = (int)projectile.position.Y / 16;
                    num41 += num39;
                    num41 += (int)projectile.velocity.X;
                    for (int j = num42; j < num42 + projectile.height / 16 + 1; j++)
                    {
                        if (WorldGen.SolidTile(num41, j))
                        {
                            flag13 = true;
                        }
                    }
                }

                Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
                float num43 = Utils.GetLerpValue(0f, 100f, vector13.Y, clamped: true) * Utils.GetLerpValue(-2f, -6f, projectile.velocity.Y, clamped: true);
                if (projectile.velocity.Y == 0f)
                {
                    if (flag13)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            int num44 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                            if (k == 0)
                            {
                                num44 = (int)projectile.position.X / 16;
                            }
                            if (k == 2)
                            {
                                num44 = (int)(projectile.position.X + (float)projectile.width) / 16;
                            }
                            int num45 = (int)(projectile.position.Y + (float)projectile.height) / 16;
                            if (!WorldGen.SolidTile(num44, num45) && !Main.tile[num44, num45].IsHalfBlock && Main.tile[num44, num45].Slope <= 0 && 
                                (!TileID.Sets.Platforms[Main.tile[num44, num45].TileType] || !Main.tile[num44, num45].HasTile || Main.tile[num44, num45].IsActuated))
                            {
                                continue;
                            }
                            try
                            {
                                num44 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                                num45 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
                                num44 += num39;
                                num44 += (int)projectile.velocity.X;
                                if (!WorldGen.SolidTile(num44, num45 - 1) && !WorldGen.SolidTile(num44, num45 - 2))
                                {
                                    projectile.velocity.Y = -5.1f;
                                }
                                else if (!WorldGen.SolidTile(num44, num45 - 2))
                                {
                                    projectile.velocity.Y = -7.1f;
                                }
                                else if (WorldGen.SolidTile(num44, num45 - 5))
                                {
                                    projectile.velocity.Y = -11.1f;
                                }
                                else if (WorldGen.SolidTile(num44, num45 - 4))
                                {
                                    projectile.velocity.Y = -10.1f;
                                }
                                else
                                {
                                    projectile.velocity.Y = -9.1f;
                                }
                            }
                            catch
                            {
                                projectile.velocity.Y = -9.1f;
                            }
                        }
                        if (vector.Y - projectile.Center.Y < -48f)
                        {
                            float num46 = vector.Y - projectile.Center.Y;
                            num46 *= -1f;
                            if (num46 < 60f)
                            {
                                projectile.velocity.Y = -6f;
                            }
                            else if (num46 < 80f)
                            {
                                projectile.velocity.Y = -7f;
                            }
                            else if (num46 < 100f)
                            {
                                projectile.velocity.Y = -8f;
                            }
                            else if (num46 < 120f)
                            {
                                projectile.velocity.Y = -9f;
                            }
                            else if (num46 < 140f)
                            {
                                projectile.velocity.Y = -10f;
                            }
                            else if (num46 < 160f)
                            {
                                projectile.velocity.Y = -11f;
                            }
                            else if (num46 < 190f)
                            {
                                projectile.velocity.Y = -12f;
                            }
                            else if (num46 < 210f)
                            {
                                projectile.velocity.Y = -13f;
                            }
                            else if (num46 < 270f)
                            {
                                projectile.velocity.Y = -14f;
                            }
                            else if (num46 < 310f)
                            {
                                projectile.velocity.Y = -15f;
                            }
                            else
                            {
                                projectile.velocity.Y = -16f;
                            }
                        }
                        if (projectile.wet && num43 == 0f)
                        {
                            projectile.velocity.Y *= 2f;
                        }
                    }
                }
                if (projectile.velocity.X > num36)
                {
                    projectile.velocity.X = num36;
                }
                if (projectile.velocity.X < 0f - num36)
                {
                    projectile.velocity.X = 0f - num36;
                }
                if (projectile.velocity.X < 0f)
                {
                    projectile.direction = -1;
                }
                if (projectile.velocity.X > 0f)
                {
                    projectile.direction = 1;
                }
                if (projectile.velocity.X == 0f)
                {
                    projectile.direction = ((playerCenter.X > projectile.Center.X) ? 1 : (-1));
                }
                if (projectile.velocity.X > num34 && num39 == 1)
                {
                    projectile.direction = 1;
                }
                if (projectile.velocity.X < 0f - num34 && num39 == -1)
                {
                    projectile.direction = -1;
                }
                projectile.spriteDirection = projectile.direction;

                if (projectile.velocity.Y == 0f)
                {
                    projectile.rotation = projectile.rotation.AngleTowards(0f, 0.3f);
                    if (projectile.velocity.X == 0f)
                    {
                        projectile.frame = 0;
                        projectile.frameCounter = 0;
                    }
                    else if (Math.Abs(projectile.velocity.X) >= 0)//0.5f)
                    {
                        projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
                        //projectile.frameCounter++;
                        if (projectile.frameCounter > 10)
                        {
                            projectile.frame++;
                            projectile.frameCounter = 0;
                        }
                        if (isTabbyOrPurrpurr && projectile.frame >= 4)
                        {
                            projectile.frame = 0;
                        }
                        else if (/*projectile.frame < 2 ||*/ projectile.frame >= Main.projFrames[projectile.type])
                        {
                            projectile.frame = 0;// 2;
                        }
                    }
                }
                // Jumping or falling
                else if (projectile.velocity.Y != 0f)
                {
                    projectile.frameCounter = 0;
                    if (isTabbyOrPurrpurr)
                    {
                        projectile.frame = 4;
                    }
                    else
                    {
                        projectile.frame = 1;
                    }

                    projectile.rotation = Math.Min(4f, projectile.velocity.Y) * -0.1f;
                    // Fix rotation when falling facing left
                    if (projectile.spriteDirection == -1)
                    {
                        projectile.rotation -= (float)Math.PI * 2f;
                    }
                }

                projectile.velocity.Y += 0.4f + num43 * 1f;

                // Clamp falling velocity
                if (projectile.velocity.Y > 10f)
                {
                    projectile.velocity.Y = 10f;
                }
            }
        }

        public static void SpawnWolfKing(int tmpX, int tmpY, Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            var leftPos = -1;
            for (int iL = 0; iL < 50; iL++)
            {
                var tileL = Main.tile[tmpX - iL, tmpY];
                if (!ChangedUtils.IsBlackLatexWall(tileL))
                {
                    leftPos = tmpX - iL + 1;
                    break;
                }
            }

            var rightPos = -1;
            for (int iR = 0; iR < 50; iR++)
            {
                var tileR = Main.tile[tmpX + iR, tmpY];
                if (!ChangedUtils.IsBlackLatexWall(tileR))
                {
                    rightPos = tmpX + iR - 1;
                    break;
                }
            }

            var topPos = -1;
            for (int iT = 0; iT < 30; iT++)
            {
                var tileT = Main.tile[tmpX, tmpY - iT];
                if (!ChangedUtils.IsBlackLatexWall(tileT))
                {
                    topPos = tmpY - iT + 1;
                    break;
                }
            }

            var bottomPos = -1;
            for (int iB = 0; iB < 30; iB++)
            {
                var tileR = Main.tile[tmpX, tmpY + iB];
                if (!ChangedUtils.IsBlackLatexWall(tileR))
                {
                    bottomPos = tmpY + iB - 1;
                    break;
                }
            }

            tmpX = (int)(leftPos + 0.5f * (rightPos - leftPos) - 2) + 3;
            tmpY = topPos + 3;

            int type = ModContent.NPCType<WolfKingSpawn>();

            var npcIndex = NPC.NewNPC(new EntitySource_WorldEvent(), tmpX * 16, tmpY * 16, type);

            if (Main.netMode == NetmodeID.Server && npcIndex != -1)
                NetMessage.SendData(MessageID.SyncNPC, number: npcIndex);
        }

        public static void SpawnBehemoth(int tmpX, int tmpY, Player player)
        {
            var leftPos = -1;
            for (int iL = 0; iL < 50; iL++)
            {
                var tileL = Main.tile[tmpX - iL, tmpY];
                if (!ChangedUtils.IsWhiteLatexWall(tileL))
                {
                    leftPos = tmpX - iL + 1;
                    break;
                }
            }

            var rightPos = -1;
            for (int iR = 0; iR < 50; iR++)
            {
                var tileR = Main.tile[tmpX + iR, tmpY];
                if (!ChangedUtils.IsWhiteLatexWall(tileR))
                {
                    rightPos = tmpX + iR - 1;
                    break;
                }
            }

            var topPos = -1;
            for (int iT = 0; iT < 30; iT++)
            {
                var tileT = Main.tile[tmpX, tmpY - iT];
                if (!ChangedUtils.IsWhiteLatexWall(tileT))
                {
                    topPos = tmpY - iT + 1;
                    break;
                }
            }

            var bottomPos = -1;
            for (int iB = 0; iB < 30; iB++)
            {
                var tileR = Main.tile[tmpX, tmpY + iB];
                if (!ChangedUtils.IsWhiteLatexWall(tileR))
                {
                    bottomPos = tmpY + iB - 1;
                    break;
                }
            }

            // he is about 4 blocks wide, no 12
            tmpX = (int)(leftPos + 0.5f * (rightPos - leftPos) - 6 + 6);
            tmpY = bottomPos - 1;//(int)(topPos + 0.5f * (bottomPos - topPos));

            int type = ModContent.NPCType<BehemothSpawn>();
            var npcIndex = NPC.NewNPC(new EntitySource_WorldEvent(), tmpX * 16, tmpY * 16, type);

            // Server sync
            if (Main.netMode == NetmodeID.Server && npcIndex != -1)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: npcIndex);
            }
        }

        public static void TeleportPlayer(int playerIndex, int xPos, int yPos)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ChangedSpecialMod.Instance.GetPacket();
                packet.Write((byte)MessageType.TeleportPlayer);
                packet.Write((short)playerIndex);
                packet.Write((short)xPos);
                packet.Write((short)yPos);
                packet.Send();
            }
            else
                DoTeleportPlayer(playerIndex, xPos, yPos);
            /*
            if (playerIndex != -1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                var player = Main.player[playerIndex];
                player.position = new Vector2(xPos * 16, yPos * 16);

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)player.whoAmI, xPos * 16, yPos * 16, 1, 0, 0);
            }
            */
        }

        public static void DoTeleportPlayer(int playerIndex, int xPos, int yPos)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient || playerIndex == -1)
                return;

            var player = Main.player[playerIndex];
            player.position = new Vector2(xPos * 16, yPos * 16);
            
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)player.whoAmI, xPos * 16, yPos * 16, TeleportationStyleID.DebugTeleport, 0, 0);
        }

        public static void PlayerSpawnsWolfKing(int playerIndex)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ChangedSpecialMod.Instance.GetPacket();
                packet.Write((byte)MessageType.PlayerSpawnsWolfKing);
                packet.Write((short)playerIndex);
                packet.Send();
            }

            else
                NPCSpawnCheckSystem.WolfKingSpawnCheck(true, playerIndex);
        }



        public static void SpawnCheerleaders(NPC boss, bool removeActive = false)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) 
                return;

            int count = 4;
            int distFromBoss = 6;
            int distBetween = 4;

            var npcType = ModContent.NPCType<Cheerleader>();

            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == npcType)
                    DespawnNPC(npc);
            }

            for (int i = 0; i < count; i++)
            {
                //0,1,2,3
                int xPos = (int)boss.Center.X;

                switch (i)
                {
                    case 0:
                        xPos = xPos - 16 * (distFromBoss + distBetween * 1);
                        break;
                    case 1:
                        xPos = xPos - 16 * (distFromBoss + distBetween * 0);
                        break;
                    case 2:
                        xPos = xPos + 16 * (distFromBoss + distBetween * 0);
                        break;
                    case 3:
                        xPos = xPos + 16 * (distFromBoss + distBetween * 1);
                        break;
                    default:
                        break;
                }

                int yPos = (int)boss.Center.Y;
                var npcIndex = NPC.NewNPC(new EntitySource_WorldEvent(), xPos, yPos, npcType);

                if (npcIndex != -1)
                {
                    var npc = Main.npc[npcIndex];

                    // npcs left of him
                    if (i < count / 2)
                        npc.direction = 1;
                    // npcs right of him
                    else
                        npc.direction = -1;

                    npc.spriteDirection = npc.direction;

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.SyncNPC, number: npcIndex);
                }
            }
        }

        // Same as vanilla, but with tileframe y != 0 removed
        // No idea why it was there. It makes deerclops fall through all platforms except the normal wooden one
        // And we don't want that with the behemoth hands
        public static bool SolidCollision(Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
        {
            int value = (int)(Position.X / 16f) - 1;
            int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
            int value3 = (int)(Position.Y / 16f) - 1;
            int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
            int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
            value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
            value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
            value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
            Vector2 vector = default(Vector2);
            for (int i = num; i < value2; i++)
            {
                for (int j = value3; j < value4; j++)
                {
                    Tile tile = Main.tile[i, j];
                    // active inactive
                    if (tile == null || !tile.HasTile || tile.IsActuated)
                        continue;

                    bool flag = Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
                    if (acceptTopSurfaces)
                        flag |= Main.tileSolidTop[tile.TileType];

                    if (flag)
                    {
                        vector.X = i * 16;
                        vector.Y = j * 16;
                        int num2 = 16;
                        if (tile.IsHalfBlock)
                        {
                            vector.Y += 8f;
                            num2 -= 8;
                        }

                        if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num2)
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Cancels any currently active sign or chest name edits, and closes any chest the player may have open.
        /// </summary>
        /// <param name="player">The player to act upon. Should basically always be Main.LocalPlayer.</param>
        public static void CancelSignsAndChests(Player player)
        {
            Main.mouseRightRelease = false;

            // If a sign or chest was in use previously, close those GUIs.
            if (player.sign >= 0)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = "";
            }
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = "";
            }
            if (player.chest >= 0)
                player.chest = -1;
        }

        public static void DespawnNPC(NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            npc.active = false;

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
        }
    }
}
