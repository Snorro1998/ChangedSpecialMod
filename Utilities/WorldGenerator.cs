using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Common.WorldGeneration;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Content.Tiles.Furniture.Paintings;
using ChangedSpecialMod.Content.Tiles.Latex;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Utilities
{
    public static class WorldGenerator
    {
        public static int RoomWidthMin = 11;
        public static int RoomWidthMax = 21;
        public static int RoomHeightMin = 9;
        public static int RoomHeightMax = 15;

        private static int[] globalWallDecorations = new int[]
        {
            // Paintings
            ModContent.TileType<Painting1>(),
            ModContent.TileType<Painting2>(),
            ModContent.TileType<Painting3>(),
            ModContent.TileType<Painting4>(),
            ModContent.TileType<Painting5>(), 
            ModContent.TileType<Painting8>(),
            ModContent.TileType<Painting9>(),
            ModContent.TileType<Painting10>(),
            ModContent.TileType<Painting11>(),
            ModContent.TileType<Painting16>(),
            ModContent.TileType<Painting17>(),

            // Pictures
            ModContent.TileType<Pictures1>(),
            ModContent.TileType<Pictures2>(),
            ModContent.TileType<Pictures3>(),
            ModContent.TileType<Pictures4>(),
            ModContent.TileType<Pictures5>(),
            ModContent.TileType<Pictures6>(),

            // Others
            ModContent.TileType<IrisScanner>(),
        };

        private static int[] globalWallDecorationsDrunk = new int[]
        {
            // Paintings
            ModContent.TileType<Painting5>(),       // Laughing
            ModContent.TileType<Painting7>(),       // Cuddly
            ModContent.TileType<DrunkPainting1>(),  // Puro closeup
            ModContent.TileType<DrunkPainting2>(),  // Shark derp
            ModContent.TileType<DrunkPainting3>(),  // Squid Dog derp
            ModContent.TileType<DrunkPainting4>(),  // Puro derp

            // Pictures
            ModContent.TileType<Pictures1>()
        };

        private static Room[] globalRooms = new Room[]
        {
            new RoomLibrary(),
            new RoomLab(),
            new RoomGreenhouse(),
            new RoomToy(),
            new RoomBathroom()
        };

        private static Room[] globalRoomsLow = new Room[]
        {
            new RoomGenerator(),
            new RoomGas(),
        };

        // Shuffled versions of the global arrays
        private static int[] wallDecorations = new int[] { };
        private static int[] wallDecorationsDrunk = new int[] { };
        public static Room[] Rooms = new Room[] { };
        public static Room[] RoomsLow = new Room[] { };

        public static int WallDecorationIndex = 0;
        public static int RoomIndex = 0;
        public static int RoomIndexLow = 0;

        // Shuffle the global arrays based on the seed when generating a new world
        public static void OnGenerateNewWorld()
        {
            WallDecorationIndex = 0;
            RoomIndex = 0;
            RoomIndexLow = 0;

            wallDecorations = (int[])globalWallDecorations.Clone();
            wallDecorations = wallDecorations.ToList().OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToArray();
            
            wallDecorationsDrunk = (int[])globalWallDecorationsDrunk.Clone();
            wallDecorationsDrunk = wallDecorationsDrunk.ToList().OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToArray();

            Rooms = (Room[])globalRooms.Clone();
            Rooms = Rooms.ToList().OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToArray();

            RoomsLow = (Room[])globalRoomsLow.Clone();
            RoomsLow = RoomsLow.ToList().OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToArray();
        }

        public static void GrowCrystal(int i, int j, GooType gooType)
        {
            if (gooType == GooType.Black)
            {
                var topTile = Main.tile[i, j - 1];
                var bottomTile = Main.tile[i, j + 1];

                if (!topTile.HasTile && topTile.TileType != ModContent.TileType<CrystalGreen>() && topTile.TileType != ModContent.TileType<CrystalRed>())
                {
                    var tileType = ChangedUtils.Choose(ModContent.TileType<CrystalGreen>(), ModContent.TileType<CrystalRed>());
                    ChangedUtils.PlaceRandomTile(i, j, tileType);
                }
            }
            else if (gooType == GooType.White)
            {
                var topTile = Main.tile[i, j - 1];
                var bottomTile = Main.tile[i, j + 1];

                if (!topTile.HasTile && topTile.TileType != ModContent.TileType<CrystalWhite>() && topTile.TileType != ModContent.TileType<PillarWhite>())
                {
                    var tileType = ChangedUtils.Choose(ModContent.TileType<PillarWhite>(), ModContent.TileType<CrystalWhite>());
                    ChangedUtils.PlaceRandomTile(i, j, tileType);
                }
            }
        }


        public static void Corrupt(int i, int j, GooType gooType)
        {
            var shouldSpread = Main.rand.Next(ChangedSpecialModClientConfig.Instance.LatexSpreadChance) == 0;
                var journeyModWorldSpreadDisabled = CreativePowerManager.Instance.GetPower<CreativePowers.StopBiomeSpreadPower>().Enabled;
            
            if (journeyModWorldSpreadDisabled || j > Main.worldSurface /*|| !Main.hardMode*/ || !shouldSpread)
                return;

            var spreadRange = 3;
            var possibleTilesX = new List<int>();
            var possibleTilesY = new List<int>();
            var possibleTileConversionType = new List<int>();

            for (var y = j - spreadRange; y < j + spreadRange; y++)
            {
                for (var x = i - spreadRange; x < i + spreadRange; x++)
                {
                    if (!WorldGen.InWorld(x, j))
                        return;

                    Tile target = Main.tile[x, y];
                    if (target == null)
                        return;

                    var conversionType = -1;
                    conversionType = GetTileType(target, gooType, true);

                    if (conversionType != -1)
                    {
                        possibleTilesX.Add(x);
                        possibleTilesY.Add(y);
                        possibleTileConversionType.Add(conversionType);
                    }
                }
            }

            if (possibleTilesX.Any())
            {
                var index = ChangedUtils.MainRandNext(0, possibleTilesX.Count);
                var targetX = possibleTilesX[index];
                var targetY = possibleTilesY[index];
                var conversionType = possibleTileConversionType[index];
                Main.tile[targetX, targetY].TileType = (ushort)conversionType;
                WorldGen.SquareTileFrame(targetX, targetY);
            }
        }

        public static void DestroyChestAtPosition(int x, int y)
        {
            if (!WorldGen.InWorld(x, y))
                return;

            var tile = Main.tile[x, y];
            if (tile == null || !tile.HasTile)
                return;

            (int xTop, int yTop) = TileObjectData.TopLeft(x, y);

            var chestId = Chest.FindChest(xTop, yTop);
            if (chestId != -1)
                Chest.DestroyChestDirect(xTop, yTop, chestId);
        }

        public static void FloodRoom(int xPos, int yPos, int width, int height)
        {
            var left = xPos + 1;
            var right = xPos + width - 2;
            var top = yPos + 1;
            var bottom = yPos + height - 1;
            var nWaterfalls = ChangedUtils.WorldGenRandNext(1, 4);

            // Add waterfalls along the wall
            for (var i = 0; i < nWaterfalls; i++)
            {
                var x = ChangedUtils.WorldGenRandNext(left, right + 1);
                for (var y = top; y < bottom; y++)
                {
                    WorldGen.KillWall(x, y);
                    WorldGen.PlaceWall(x, y, WallID.Waterfall, true);
                }
            }

            // Fill the bottom 3 blocks with water
            for (var y = bottom; y > bottom - 3; y--)
            {
                for (var x = xPos + 1; x < xPos + width - 1; x++)
                {
                    WorldGen.PlaceLiquid(x, y, (byte)LiquidID.Water, 255);
                }
            }
        }

        public static void MakeBox(int xPos, int yPos, int width, int height, int tType, int wType, byte tPaint = PaintID.None, byte wPaint = PaintID.None, bool leftSideOpen = false, bool isLeft = false, bool isTop = false)
        {
            var right = xPos + width - 1;
            var bottom = yPos + height - 1;

            var xLeftWall = xPos + 1;
            if (leftSideOpen) xLeftWall--;

            var blockRemoveX = xPos + 1;
            var blockRemoveY = yPos + 1;

            for (var y = yPos; y < yPos + height; y++)
            {
                for (var x = xPos; x < xPos + width; x++)
                {
                    if (x >= xLeftWall && x < right && y > yPos && y < bottom)
                    {
                        WorldGen.KillWall(x, y);
                        WorldGen.PlaceWall(x, y, wType, true);
                        if (wPaint != PaintID.None)
                        {
                            var tile = Main.tile[x, y];
                            tile.WallColor = wPaint;
                        }
                    }

                    // Remove wall from the room to the left
                    if (leftSideOpen && x == xPos && y > yPos && y < bottom)
                    {
                        DestroyChestAtPosition(x, y);
                        WorldGen.KillTile(x, y);
                    }

                    else if (!((x > xPos && x < right) && (y > yPos && y < bottom)))
                    {
                        WorldGen.PlaceTile(x, y, tType, true);
                    }
                }
            }
        }

        public static void AddWire(int xPos, int yPos, int width, int height, bool addSwitch = false)
        {
            var right = xPos + width - 1;
            var bottom = yPos + height - 1;

            for (var y = yPos; y < yPos + height; y++)
            {
                for (var x = xPos; x < xPos + width; x++)
                {
                    if (y == yPos || x == xPos + width / 2)
                    {
                        WorldGen.PlaceWire(x, y);
                    }

                    // Wire for the switch
                    if (addSwitch)
                    {
                        if (x > xPos + 1 && x < right - 1 && y == bottom - 3)
                        {
                            WorldGen.PlaceWire(x, y);
                        }
                    }
                }
            }

            // The switch itself
            if (addSwitch)
            {
                var x = ChangedUtils.WorldGenRandNext(xPos + 2, right - 2);
                WorldGen.PlaceTile(x, bottom - 3, TileID.Switches, true, true);
            }
        }

        public static void AddDoors(int xPos, int yPos, int width, int height, bool closed = false)
        {
            var left = xPos;
            var right = xPos + width - 1;
            var bottom = yPos + height - 1;

            var doorClosed = ModContent.TileType<LabDoorClosed>();
            var doorOpen = ModContent.TileType<LabDoorOpen>();
            var doorType = ChangedUtils.Choose(doorClosed, doorOpen);
            if (closed) doorType = doorClosed;

            // Check if there is a left door and add one if there isn't
            if (Main.tile[left, bottom - 3].TileType != doorClosed &&
                Main.tile[left, bottom - 3].TileType != doorOpen)
            {
                DestroyChestAtPosition(left, bottom - 1);
                WorldGen.KillTile(left, bottom - 1, false, false, true);
                DestroyChestAtPosition(left, bottom - 2);
                WorldGen.KillTile(left, bottom - 2, false, false, true);
                DestroyChestAtPosition(left, bottom - 3);
                WorldGen.KillTile(left, bottom - 3, false, false, true);
                WorldGen.PlaceTile(left, bottom - 3, doorType, true, true);
            }

            // Check if there is a right door and add one if there isn't
            if (Main.tile[right, bottom - 3].TileType != doorClosed &&
                Main.tile[right, bottom - 3].TileType != doorOpen)
            {
                DestroyChestAtPosition(right, bottom - 1);
                WorldGen.KillTile(right, bottom - 1, false, false, true);
                DestroyChestAtPosition(right, bottom - 2);
                WorldGen.KillTile(right, bottom - 2, false, false, true);
                DestroyChestAtPosition(right, bottom - 3);
                WorldGen.KillTile(right, bottom - 3, false, false, true);
                WorldGen.PlaceTile(right, bottom - 3, doorType, true, true);
            }
        }

        public static void AddPainting(int xCur, int w, int yCur, int h)
        {
            var ww = xCur + 2;
            var tmp = new List<int>();
            for (var j = xCur + 2; j < xCur + w - 3; j++)
            {
                tmp.Add(j);
            }
            
            var xPositions = tmp.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToArray();

            for (var x = 0; x < xPositions.Length; x++)
            {
                var xPos = xPositions[x];

                var decorList = wallDecorations;
                if (Main.drunkWorld)
                    decorList = wallDecorationsDrunk;

                var decor = decorList[WallDecorationIndex % decorList.Length];
                WorldGen.PlaceTile(xPos, yCur + h - 5, decor, true, true, -1, 0);
                // Successfully placed
                if (Main.tile[xPos, yCur + h - 5].TileType == decor)
                {
                    WallDecorationIndex++;
                    break;
                }
            }
        }

        public static void AddFans(int xCur, int w, int yCur, int h)
        {
            var ww = xCur + 2;
            var tmp = new List<int>();
            for (var j = xCur + 2; j < xCur + w - 3; j++)
            {
                tmp.Add(j);
            }

            var right = xCur + w - 1;
            var bottom = yCur + h - 1;

            var xPositions = tmp.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToArray();
            var placedAnything = false;

            for (var x = 0; x < xPositions.Length; x++)
            {
                var xPos = xPositions[x];

                var decor = ModContent.TileType<Fan>();
                WorldGen.PlaceTile(xPos, yCur + h - 5, decor, true, true, -1, 0);
                // Successfully placed
                if (Main.tile[xPos, yCur + h - 5].TileType == decor)
                {
                    placedAnything = true;
                    break;
                }
            }

            if (placedAnything)
            {
                for (var x = xCur + 2; x < right - 1; x++)
                {
                    WorldGen.PlaceWire(x, yCur + h - 5);
                }
            }
        }

        public static bool AddDecor(Room room, int xStart, int roomWidth, int yCur, int h)
        {
            if (room.Decor == null) return true;
            var placedAnything = false;
            var xCurrent = xStart + 3;
            for (var i = 0; i < 20; i++)
            {
                if (xCurrent > xStart + roomWidth - 3) 
                    break;

                var decor = room.GetDecor();
                if (room.SingleDecor != -1 && !room.PlacedSingleDecor && ChangedUtils.WorldGenRandNext(0, 3) == 0)
                {
                    room.PlacedSingleDecor = true;
                    decor = room.SingleDecor;
                }
                var nStyles = ChangedUtils.GetNumberOfStylesPerItem(decor);
                var style = ChangedUtils.WorldGenRandNext(0, nStyles);
                var placementResult = WorldGen.PlaceObject(xCurrent, yCur + h - 2, decor, true, style, 1, -1, ChangedUtils.WorldGenRandNext(1, 3));
                if (!placedAnything) 
                    placedAnything = placementResult;
                xCurrent += ChangedUtils.WorldGenRandNext(3, 7);
            }
            return placedAnything;
        }

        // We don't have many useful items, but we still add a chest to the lab so it won't get destroyed by meteors
        // During testing one fell right next to it, but they don't do that if there is a chest nearby
        public static void AddChest(int xStart, int roomWidth, int yCur, int h)
        {
            var ww = xStart + 2;
            var tmp = new List<int>();
            for (var j = xStart + 2; j < xStart + roomWidth - 3; j++)
            {
                tmp.Add(j);
            }

            var xPositions = tmp.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToArray();
            var decor = ModContent.TileType<PackingBox>();

            for (var x = 0; x < xPositions.Length; x++)
            {
                var xPos = xPositions[x];
                var chestIndex = WorldGen.PlaceChest(xPos, yCur + h - 2, (ushort)decor, false, 0);
                
                if (chestIndex >= 0)
                {
                    var chest = Main.chest[chestIndex];
                    var item = ModContent.ItemType<Orange>();
                    var amount = WorldGen.genRand.Next(1, 4);
                    
                    chest.item[0].SetDefaults(item, false);
                    chest.item[0].stack = amount;

                    item = Utils.SelectRandom(WorldGen.genRand, ItemID.IronBar, ItemID.LeadBar);
                    amount = WorldGen.genRand.Next(3, 11);

                    chest.item[1].SetDefaults(item, false);
                    chest.item[1].stack = amount;

                    break;
                }
            }
        }

        private static int GetTileTypeBlackLatex(Tile tile)
        {
            int tileType = -1;

            switch (tile.TileType)
            {
                case TileID.Sand:
                    tileType = ModContent.TileType<BlackLatexSandTile>();
                    break;

                case TileID.Grass:
                case TileID.Dirt:
                case TileID.ClayBlock:

                // Desert
                case TileID.HardenedSand:

                // Corruption
                case TileID.CorruptGrass:
                case TileID.Ebonsand:
                case TileID.Ebonstone:

                // Crimson
                case TileID.CrimsonGrass:
                case TileID.Crimsand:
                case TileID.Crimstone:

                // Jungle
                case TileID.Mud:
                case TileID.JungleGrass:

                // Snow
                case TileID.SnowBlock:
                case TileID.IceBlock:
                    tileType = ModContent.TileType<BlackLatexTile>();
                    break;
                default:
                    break;
            }

            return tileType;
        }

        private static int GetTileTypeWhiteLatex(Tile tile)
        {
            int tileType = -1;

            switch (tile.TileType)
            {
                case TileID.Sand:
                    tileType = ModContent.TileType<WhiteLatexSandTile>();
                    break;

                case TileID.Grass:
                case TileID.Dirt:
                case TileID.ClayBlock:

                // Desert
                case TileID.HardenedSand:

                // Corruption
                case TileID.CorruptGrass:
                case TileID.Ebonsand:
                case TileID.Ebonstone:

                // Crimson
                case TileID.CrimsonGrass:
                case TileID.Crimsand:
                case TileID.Crimstone:

                // Jungle
                case TileID.Mud:
                case TileID.JungleGrass:

                // Snow
                case TileID.SnowBlock:
                case TileID.IceBlock:
                    tileType = ModContent.TileType<WhiteLatexTile>();
                    break;
                default:
                    break;
            }

            return tileType;
        }

        private static int GetTileTypeDryDirt(Tile tile)
        {
            int tileType = -1;

            switch (tile.TileType)
            {
                // TODO add white sand
                case TileID.Sand:
                //    tileType = ModContent.TileType<BlackLatexSandTile>();
                //    break;

                case TileID.Grass:
                case TileID.Dirt:
                case TileID.ClayBlock:

                // Desert
                case TileID.HardenedSand:

                // Corruption
                case TileID.CorruptGrass:
                case TileID.Ebonsand:
                case TileID.Ebonstone:

                // Crimson
                case TileID.CrimsonGrass:
                case TileID.Crimsand:
                case TileID.Crimstone:

                // Jungle
                case TileID.Mud:
                case TileID.JungleGrass:

                // Snow
                case TileID.SnowBlock:
                case TileID.IceBlock:
                    tileType = ModContent.TileType<DryDirt>();
                    break;
                default:
                    break;
            }

            return tileType;
        }

        public static int GetTileType(Tile tile, GooType gooType, bool onlySpread = false)
        {
            int tileType = -1;

            if (tile == null || !tile.HasTile)
                return tileType;

            // If not during worldgen, only allow a few types
            if (onlySpread)
            {
                switch (tile.TileType)
                {
                    case TileID.Grass:
                    case TileID.Dirt:
                    case TileID.Sand:
                        break;
                    default:
                        return tileType;
                }
            }

            if (gooType == GooType.Black)
                tileType = GetTileTypeBlackLatex(tile);
            else if (gooType == GooType.White)
                tileType = GetTileTypeWhiteLatex(tile);
            else if (gooType == GooType.None)
                tileType = GetTileTypeDryDirt(tile);
            return tileType;
        }

        public static bool ShouldReplaceWall(int wallId)
        {
            switch (wallId)
            {
                case WallID.DirtUnsafe:
                case WallID.GrassUnsafe:
                case WallID.FlowerUnsafe:
                case WallID.MudUnsafe:
                case WallID.JungleUnsafe:
                case WallID.Sandstone:
                case WallID.CorruptGrassEcho:
                case WallID.Corruption1Echo:
                case WallID.EbonstoneEcho:
                    return true;
            }
            return false;
        }

        public static void CreateLatexBiome(int xPos, int yPos, int width, int height, int xPadding = 100, int yPadding = 50, LabType labType = LabType.CityRuins)
        {
            int distFallOff = 10;

            var left = xPos - xPadding;
            var top = yPos - yPadding;
            var right = left + width + 2 * xPadding;
            var bottom = top + height + yPadding;

            // Ellipse center
            double centerX = (left + right) / 2.0;
            double centerY = (top + bottom) / 2.0;

            // Radii
            double radiusX = (right - left) / 2.0;
            double radiusY = (bottom - top) / 2.0;

            for (var x = left; x < right; x++)
            {
                for (var y = top; y < bottom; y++)
                {
                    // Normalize position inside ellipse
                    double dx = (x - centerX) / radiusX;
                    double dy = (y - centerY) / radiusY;

                    double distance = dx * dx + dy * dy;
                    if (distance > 1.0)
                        continue;

                    // Distance from ellipse edge in tiles
                    double edgeDistance = (1.0 - distance) * Math.Min(radiusX, radiusY);

                    // Normalize falloff (0 at edge -> 1 inside)
                    double falloffFactor = Math.Clamp(edgeDistance / distFallOff, 0.0, 1.0);

                    // Convert to integer chance
                    var chance = (int)(falloffFactor * distFallOff);

                    if (chance <= 0)
                        continue;

                    if (ChangedUtils.WorldGenRandNext(0, distFallOff - chance) != 0)
                        continue;

                    var tile = Main.tile[x, y];
                    GooType gooType = GooType.None;
                    if (labType == LabType.Black)
                        gooType = GooType.Black;
                    else if (labType == LabType.White)
                        gooType = GooType.White;
                    var tileType = GetTileType(tile, gooType);

                    if (tileType != -1)
                    {
                        tile.TileType = (ushort)tileType;
                    }
                }
            }
        }
    }
}
