using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.WorldGeneration.Passes
{
    public class PassOrangeShrines : WorldGenerationPass
    {
        public PassOrangeShrines()
        {
            PassName = "ChangedOrangeShrines";
            GenerationMessage = "OrangeShrines";
        }

        public override void Generate()
        {
            var nSucces = 0;
            var nShrines = 5;

            if (Main.maxTilesX >= WorldGen.WorldSizeLargeX)
                nShrines = 15;
            else if (Main.maxTilesX >= WorldGen.WorldSizeMediumX)
                nShrines = 10;

            var nMaxAttempts = 500;
            var placedPositions = new List<(int, int)>();

            for (int i = 0; i < nMaxAttempts; i++)
            {
                var succes = MakeOrangeShrine(ref placedPositions);
                if (succes)
                    nSucces++;
                if (nSucces >= nShrines)
                    break;
            }
        }

        private void CreateChest(int xx, int yy)
        {
            // Place a dead man's chest
            var chestIndex = WorldGen.PlaceChest(xx, yy, TileID.Containers2, false, 4);
            if (chestIndex >= 0)
            {
                var chest = Main.chest[chestIndex];
                var items = new List<int>();
                var itemAmounts = new List<int>();

                var primaryItems = new List<int>()
                                        {
                                            ItemID.Extractinator,
                                            ItemID.BandofRegeneration,
                                            ItemID.MagicMirror,
                                            ItemID.CloudinaBottle,
                                            ItemID.HermesBoots,
                                            ItemID.Mace,
                                            ItemID.ShoeSpikes
                                        };

                items.Add(Utils.SelectRandom(WorldGen.genRand, primaryItems.ToArray()));
                itemAmounts.Add(1);

                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.SuspiciousLookingEye, 5, 1, 1);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.Dynamite, 3, 1, 1);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.JestersArrow, 4, 25, 50);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.Topaz, 3, 1, 4);
                WorldGenerator.AddRandomToPool(ref items, ref itemAmounts, new int[] { ItemID.SilverBar, ItemID.TungstenBar }, 2, 3, 10);
                WorldGenerator.AddRandomToPool(ref items, ref itemAmounts, new int[] { ItemID.GoldBar, ItemID.PlatinumBar }, 2, 3, 10);
                WorldGenerator.AddRandomToPool(ref items, ref itemAmounts, new int[] { ItemID.FlamingArrow, ItemID.ThrowingKnife }, 2, 25, 50);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.HealingPotion, 2, 3, 5);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.OrangeTorch, 2, 15, 30);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ModContent.ItemType<BasketballWeapon>(), 3, 1, 1);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.GolfBallDyedOrange, 3, 1, 1);

                // Set this to a lower value if other mods also add things to chests
                // Spirit reforged doesn't check and just crashes the world generation
                // So we just lower the number of items if that mod is enabled.
                var nItemsChest = 40;
                if (ModSupportSystem.IsModActive("SpiritReforged"))
                    nItemsChest = 30;


                for (int j = items.Count; j < nItemsChest; j++)
                {
                    if (Main.rand.NextBool(3))
                    {
                        items.Add(ItemID.BloodOrange);
                        itemAmounts.Add(1);
                    }
                    else
                    {
                        items.Add(ModContent.ItemType<Orange>());
                        itemAmounts.Add(1);
                    }
                }

                for (int j = 0; j < nItemsChest; j++)
                {
                    var item = items[j];
                    chest.item[j].SetDefaults(item, false);
                    chest.item[j].stack = itemAmounts[j];
                }
            }
        }

        private bool MakeOrangeShrine(ref List<(int, int)> placedPositions)
        {
            List<string> structureBackground = new List<string>()
            {
                "      1111      ",
                "    11111111    ",
                "   1111111111   ",
                "  111111111111  ",
                "  111111111111  ",
                " 11111111111111 ",
                " 11111111111111 ",
                " 11111111111111 ",
                " 11111111111111 ",
                " 11111111111111 ",
                " 11111111111111 ",
                "  111111111111  ",
                "  111111111111  ",
                "   1111111111   ",
                "    11111111    ",
                "      1111      ",
            };

            List<string> structure = new List<string>()
            {
                "     111111     ",
                "   111....111   ",
                "  11........11  ",
                " 11..........11 ",
                " 1............1 ",
                "11............11",
                "1......66......1",
                "1......66......1",
                "1......55......1",
                "1.4..........4.1",
                "114..........411",
                " 14....33....41 ",
                " 11..2.33..2.11 ",
                "  11.2.11..211  ",
                "   1111111111   ",
                "     111111     ",
            };

            List<string> structureWires = new List<string>()
            {
                "                ",
                "                ",
                "                ",
                "                ",
                "                ",
                "                ",
                "                ",
                "     111111     ",
                "     1    1     ",
                "     1    1     ",
                "     1    1     ",
                "     1    1     ",
                "     1    1     ",
                "     1    1     ",
                "                ",
                "                ",
            };

            int distFromBorderX = 200;

            // Move further inward if Calamity is enabled so it won't end up in the abyss
            // We might have to also add something for the sunken sea under the desert
            if (ModSupportSystem.IsModActive("CalamityMod"))
                distFromBorderX = 400;

            int distFromBorderY = 300;

            int width = structure[0].Length;
            int height = structure.Count;

            int xPosMin = distFromBorderX;
            int xPosMax = Main.maxTilesX - distFromBorderX - width;
            int yPosMin = (int)(Main.maxTilesY * 0.4f);
            int yPosMax = Main.maxTilesY - distFromBorderY;

            int xPos = ChangedUtils.WorldGenRandNext(xPosMin, xPosMax);
            int yPos = ChangedUtils.WorldGenRandNext(yPosMin, yPosMax);

            if (!WorldGen.InWorld(xPos, yPos))
                return false;

            // Don't place it if too close to another shrine
            foreach (var position in placedPositions)
            {
                var dist = Vector2.DistanceSquared(new Vector2(position.Item1, position.Item2), new Vector2(xPos, yPos));
                var minDist = 300;
                if (dist < minDist * minDist)
                    return false;
            }

            List<int> blackListBlocks = new List<int>()
            {
                TileID.BlueDungeonBrick,
                TileID.GreenDungeonBrick,
                TileID.PinkDungeonBrick,
                TileID.LihzahrdBrick,
                TileID.MushroomGrass,
                TileID.Granite,
                TileID.Marble,
                TileID.Hive
            };

            List<int> blackListWalls = new List<int>()
            {
                WallID.BlueDungeon,
                WallID.BlueDungeonUnsafe,
                WallID.BlueDungeonSlab,
                WallID.BlueDungeonSlabUnsafe,
                WallID.BlueDungeonTile,
                WallID.BlueDungeonTileUnsafe,

                WallID.GreenDungeon,
                WallID.GreenDungeonUnsafe,
                WallID.GreenDungeonSlab,
                WallID.GreenDungeonSlabUnsafe,
                WallID.GreenDungeonTile,
                WallID.GreenDungeonTileUnsafe,

                WallID.PinkDungeon,
                WallID.PinkDungeonUnsafe,
                WallID.PinkDungeonSlab,
                WallID.PinkDungeonSlabUnsafe,
                WallID.PinkDungeonTile,
                WallID.PinkDungeonTileUnsafe,

                WallID.Hive,
                WallID.HiveUnsafe,
                WallID.Spider,
                WallID.SpiderEcho,
                WallID.SpiderUnsafe,
                WallID.LihzahrdBrick,
                WallID.LihzahrdBrickUnsafe
            };

            // Get blocks from other mods we should avoid
            blackListBlocks.AddRange(ModSupportSystem.GetAvoidTiles());

            List<int> blackListLiquids = new List<int>
            {
                LiquidID.Honey,
                LiquidID.Shimmer
            };

            if (!Terraria.WorldBuilding.GenVars.structures.CanPlace(new Rectangle(xPos, yPos, width, height)))
                return false;

            var anyTiles = false;

            // Check if in a good place
            for (int y = 0; y < height; y++)
            {
                string line = structure[y];

                for (int x = 0; x < width; x++)
                {
                    int xx = xPos + x;
                    int yy = yPos + y;

                    if (!WorldGen.InWorld(xx, yy))
                        continue;

                    Tile t = Framing.GetTileSafely(xx, yy);
                    if (t.HasTile)
                        anyTiles = true;
                    if (t.LiquidAmount > 0 && blackListLiquids.Contains(t.LiquidType))
                        return false;
                    if (t.HasTile && blackListBlocks.Contains(t.TileType))
                        return false;
                    if (blackListWalls.Contains(t.WallType))
                        return false;
                }
            }

            // Not a valid position if floating in air
            if (!anyTiles)
                return false;

            placedPositions.Add((xPos, yPos));

            // 0: clears space and places walls and wires
            // 1: place normal blocks
            // 2: place things that need to be anchored to blocks
            for (int i = 0; i < 3; i++)
            {
                var firstPass = i == 0;
                for (int y = 0; y < height; y++)
                {
                    string line = structure[y];

                    for (int x = 0; x < width; x++)
                    {
                        char tile = line[x];

                        int xx = xPos + x;
                        int yy = yPos + y;

                        if (!WorldGen.InWorld(xx, yy))
                            continue;

                        Tile t = Framing.GetTileSafely(xx, yy);

                        // Remove blocks, place walls and wires
                        if (firstPass)
                        {
                            if (tile != ' ')
                            {
                                WorldGen.KillTile(xx, yy, false, false, true);
                                t.LiquidAmount = 0;
                            }

                            var bgLine = structureBackground[y];
                            var bgTile = bgLine[x];

                            if (bgTile == '1')
                            {
                                WorldGen.KillWall(xx, yy);
                                WorldGen.PlaceWall(xx, yy, WallID.Slime, true);
                                t.WallColor = PaintID.YellowPaint;
                            }

                            var wireLine = structureWires[y];
                            var wireTile = wireLine[x];

                            if (wireTile == '1')
                                t.RedWire = true;
                        }
                        // Place the blocks
                        else
                        {
                            switch (tile)
                            {
                                case ' ':
                                    continue;
                                case '1':
                                    WorldGen.PlaceTile(xx, yy, TileID.SlimeBlock, true);
                                    t = Framing.GetTileSafely(xx, yy);
                                    t.TileColor = PaintID.OrangePaint;
                                    continue;

                                case '2':
                                    WorldGen.PlaceTile(xx, yy, (ushort)ModContent.TileType<OrangeStatue>(), true);
                                    continue;

                                case '3':
                                    WorldGen.PlaceTile(xx, yy, (ushort)ModContent.TileType<PuroStatue>(), true);
                                    continue;

                                case '4':
                                    WorldGen.PlaceTile(xx, yy, TileID.Lamps, true);
                                    continue;

                                case '5':
                                    WorldGen.PlaceTile(xx, yy, TileID.Platforms, true);
                                    continue;

                                case '6':
                                    CreateChest(xx, yy);
                                    continue;
                                case '7':
                                    WorldGen.PlaceTile(xx, yy, TileID.HangingLanterns, true);
                                    continue;
                            }
                        }

                        WorldGen.SquareTileFrame(xx, yy);
                    }
                }
            }
            return true;
        }
    }
}
