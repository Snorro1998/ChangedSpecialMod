using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Content.Tiles.Furniture.Paintings;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Utilities
{
    public class Room
    {
        public int FloorType { get; }
        public int FloorPaint { get; }
        public int[] WallType { get; }
        public int WallPaint { get; }
        public bool HasPainting { get; }
        public int LanternStyle { get; }
        public int TorchStyle { get; }
        public int ChandelierStyle { get; }
        public bool HasLightSwitch { get; }
        public bool HasFans { get; }
        public int[] Decor {  get; }
        public bool PlacedSingleDecor = false;
        public int SingleDecor { get; }
        public bool HasChest { get; }
        public int Width = 1;
        public int Height = 1;

        protected Room(int floorType, int floorPaint, int[] wallType, int wallPaint, bool hasPainting, bool hasChest, int[] decor, int singleDecor, int lanternStyle, int torchStyle, int chandelierStyle, bool hasLightSwitch, bool hasFans)
        {
            FloorType = floorType;
            FloorPaint = floorPaint;
            WallType = wallType;
            WallPaint = wallPaint;
            HasPainting = hasPainting;
            HasChest = hasChest;
            Decor = decor;
            SingleDecor = singleDecor;
            LanternStyle = lanternStyle;
            TorchStyle = torchStyle;
            ChandelierStyle = chandelierStyle;
            HasLightSwitch = hasLightSwitch;
            HasFans = hasFans;
        }

        public int GetWallType()
        {
            return WallType[ChangedUtils.WorldGenRandNext(0, WallType.Length)];
        }

        public int GetDecor()
        {
            return Decor[ChangedUtils.WorldGenRandNext(0, Decor.Length)];
        }
    }

    public class RoomDefault : Room
    {
        public RoomDefault() : base(
            floorType:      ModContent.TileType<Lab_TileTile>(),
            floorPaint:     PaintID.None,
            wallType:       [WallID.GoldBrick],
            wallPaint:      PaintID.PurplePaint,
            hasPainting:    true,
            hasChest:       false,
            decor:          [ModContent.TileType<WhiteLatexBookcases>()],
            singleDecor:    -1,
            lanternStyle:   0,
            torchStyle:     -1,
            chandelierStyle:-1,
            hasLightSwitch: false,
            hasFans: false
        ){}
    }

    public class RoomGenerator : Room
    {
        public RoomGenerator() : base(
            floorType:      ModContent.TileType<Lab_TileTile>(),
            floorPaint:     PaintID.None,
            wallType:       [WallID.LeadBrick],
            wallPaint:      PaintID.None,
            hasPainting:    false,
            hasChest:       false,
            decor:          [ModContent.TileType<Generator>()],
            singleDecor:    -1,
            lanternStyle:   0,
            torchStyle:     -1,
            chandelierStyle:-1,
            hasLightSwitch: true,
            hasFans: false
        )
        {}
    }

    public class RoomGas : Room
    {
        public RoomGas() : base(
            floorType:      ModContent.TileType<Lab_TileTile>(),
            floorPaint:     PaintID.None,
            wallType:       [WallID.LeadBrick],
            wallPaint:      PaintID.None,
            hasPainting:    true,
            hasChest:       false,
            decor:          [ModContent.TileType<RedGasTank>(), ModContent.TileType<BlueGasTank>()],
            singleDecor:    -1,
            lanternStyle:   0,
            torchStyle:     -1,
            chandelierStyle:-1,
            hasLightSwitch: false,
            hasFans: true
        )
        { }
    }

    public class RoomGreenhouse : Room
    {
        public RoomGreenhouse() : base(
            floorType:      ModContent.TileType<Lab_TileTile>(),
            floorPaint:     PaintID.None,
            wallType:       [WallID.Glass],
            wallPaint:      PaintID.None,
            hasPainting:    false,
            hasChest:       false,
            decor:          [TileID.PottedPlants1, TileID.PottedPlants2],
            singleDecor:    ModContent.TileType<WateringCan>(),
            lanternStyle:   -1,
            torchStyle:     -1,
            chandelierStyle:-1,
            hasLightSwitch: false,
            hasFans: false
        )
        {}
    }

    public class RoomLibrary : Room
    {
        public RoomLibrary() : base(
            floorType:      TileID.WoodBlock,
            floorPaint:     PaintID.None,
            wallType:       [WallID.Planked],
            wallPaint:      PaintID.None,
            hasPainting:    true,
            hasChest:       false,
            decor:          [ModContent.TileType<MountBookest>(), TileID.Bookcases],
            singleDecor:    -1,
            lanternStyle:   0,
            torchStyle:     -1,
            chandelierStyle:-1,
            hasLightSwitch: false,
            hasFans: false
        )
        {}
    }

    public class RoomToy : Room
    {
        public RoomToy() : base(
            floorType:      TileID.WoodBlock,
            floorPaint:     PaintID.None,
            wallType:       [WallID.SquigglesWallpaper],
            wallPaint:      PaintID.None,
            hasPainting:    true,
            hasChest:       false,
            decor:          [ModContent.TileType<Basketball>(), ModContent.TileType<PuroPlush>(), ModContent.TileType<SharkPlush>(), ModContent.TileType<FennecPlush>(), ModContent.TileType<Blocks>()],
            singleDecor:    -1,
            lanternStyle:   -1, //20
            torchStyle:     -1,
            chandelierStyle:47,
            hasLightSwitch: false,
            hasFans: false
        )
        { }
    }

    // For now this is more like a storage room, so add a chest in here until we make a proper storage room
    public class RoomLab : Room
    {
        public RoomLab() : base(
            ModContent.TileType<Lab_TileTile>(),
            PaintID.None,
            [WallID.Planked],
            PaintID.None,
            true,
            true,
            // Don't add chairs or else it can be a valid house and NPCs will move in before you have found it
            // We could also remove the light from the room
            [ModContent.TileType<LabTable>(), ModContent.TileType<StackOfBoxes>(), ModContent.TileType<Locker>()/*, ModContent.TileType<BlueOfficeChair>()*/],
            singleDecor: -1,
            lanternStyle: 0,
            torchStyle: -1,
            chandelierStyle: -1,
            false,
            hasFans: true
        )
        { }
    }

    public class RoomBlackGoo : Room
    {
        public RoomBlackGoo() : base(
            ModContent.TileType<BlackLatexTile>(),
            PaintID.None,
            [WallID.Slime/*, WallID.Honeyfall*/],
            PaintID.BlackPaint,
            true,
            false,
            [ModContent.TileType<CrystalGreen>(), ModContent.TileType<CrystalRed>()],
            singleDecor: -1,
            lanternStyle: -1,
            torchStyle: 3,
            chandelierStyle: -1,
            false,
            hasFans: false
        )
        {}
    }

    public class RoomWhiteGoo : Room
    {
        public RoomWhiteGoo() : base(
            ModContent.TileType<WhiteLatexTile>(),
            PaintID.None,
            [WallID.Slime/*, WallID.Honeyfall*/],
            PaintID.WhitePaint,
            true,
            false,
            [ModContent.TileType<CrystalWhite>(), ModContent.TileType<PillarWhite>(), ModContent.TileType<WhiteLatexBookcases>()],
            singleDecor: -1,
            lanternStyle: -1,
            torchStyle: 5,
            chandelierStyle: -1,
            false,
            hasFans: false
        )
        {}
    }

    public class RoomBathroom : Room
    {
        public RoomBathroom() : base(
            ModContent.TileType<Lab_TileTile>(),
            PaintID.None,
            [WallID.GrayBrick],
            PaintID.DeepLimePaint,
            false,
            false,
            [TileID.Bathtubs, TileID.Dressers, TileID.Sinks],
            singleDecor: -1,
            0,
            -1,
            chandelierStyle: -1,
            false,
            hasFans: false
        )
        {}
    }

    // Currently no stairs and just an elevator with plants near it
    public class RoomStairs : Room
    {
        public RoomStairs() : base(
            ModContent.TileType<Lab_TileTile>(),
            PaintID.None,
            [WallID.Wood],
            PaintID.None,
            false,
            false,
            [TileID.PottedPlants1],
            singleDecor: -1,
            0,
            -1,
            chandelierStyle: -1,
            false,
            hasFans: false
        )
        { }
    }

    public static class WorldGenerator
    {
        public static int RoomWidthMin = 11;
        public static int RoomWidthMax = 21;
        public static int RoomHeightMin = 9;
        public static int RoomHeightMax = 15;

        private static int[] globalWallDecorations = new int[]
        {
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
            ModContent.TileType<IrisScanner>(),

            ModContent.TileType<Pictures1>(),
            ModContent.TileType<Pictures2>(),
            ModContent.TileType<Pictures3>(),
            ModContent.TileType<Pictures4>(),
            ModContent.TileType<Pictures5>(),
            ModContent.TileType<Pictures6>(),
        };

        private static int[] globalWallDecorationsDrunk = new int[]
        {
            ModContent.TileType<Painting5>(),       // Laughing
            ModContent.TileType<Painting7>(),       // Cuddly
            ModContent.TileType<DrunkPainting1>(),  // Puro closeup
            ModContent.TileType<DrunkPainting2>(),  // Shark derp
            ModContent.TileType<DrunkPainting3>(),  // Squid Dog derp
            ModContent.TileType<DrunkPainting4>(),  // Puro derp

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
                WorldGen.KillTile(left, bottom - 1, false, false, true);
                WorldGen.KillTile(left, bottom - 2, false, false, true);
                WorldGen.KillTile(left, bottom - 3, false, false, true);
                WorldGen.PlaceTile(left, bottom - 3, doorType, true, true);
            }

            // Check if there is a right door and add one if there isn't
            if (Main.tile[right, bottom - 3].TileType != doorClosed &&
                Main.tile[right, bottom - 3].TileType != doorOpen)
            {
                WorldGen.KillTile(right, bottom - 1, false, false, true);
                WorldGen.KillTile(right, bottom - 2, false, false, true);
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

        // Add a chest to the lab so it won't get destroyed by meteors
        // during testing one fell right next to it, but they don't do that if there is a chest nearby
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

        public static int GetTileType(Tile tile, LabType labType)
        {
            int tileType = -1;

            // Pick biome tile
            var tmpTileType = ModContent.TileType<DryDirt>();
            switch (labType)
            {
                case LabType.Black:
                    tmpTileType = ModContent.TileType<BlackLatexTile>();
                    break;
                case LabType.White:
                    tmpTileType = ModContent.TileType<WhiteLatexTile>();
                    break;
            }

            switch (tile.TileType)
            {
                case TileID.Grass:
                case TileID.Dirt:
                case TileID.ClayBlock:

                // Desert
                case TileID.Sand:
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
                    tileType = tmpTileType;
                    break;
                default:
                    break;
            }

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

                    // Normalize falloff (0 at edge → 1 inside)
                    double falloffFactor = Math.Clamp(edgeDistance / distFallOff, 0.0, 1.0);

                    // Convert to integer chance
                    var chance = (int)(falloffFactor * distFallOff);

                    if (chance <= 0)
                        continue;

                    if (ChangedUtils.WorldGenRandNext(0, distFallOff - chance) != 0)
                        continue;

                    var tile = Main.tile[x, y];
                    var tileType = GetTileType(tile, labType);

                    if (tileType != -1)
                    {
                        tile.TileType = (ushort)tileType;
                    }
                }
            }
        }


        public static void KillAll(int xPos, int yPos)
        {
            for (int y = yPos; y < yPos + 100; y++)
            {
                for (int x = xPos; x < xPos + 100; x++)
                {
                    WorldGen.KillWire(x, y);
                    WorldGen.KillWall(x, y);
                    WorldGen.KillTile(x, y);
                    Main.tile[x, y].LiquidAmount = 0;
                }
            }
        }
    }
}
