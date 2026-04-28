using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.WorldGeneration
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
        public int[] Decor { get; }
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

        public int GetWallType() => WallType[ChangedUtils.WorldGenRandNext(0, WallType.Length)];
        public int GetDecor() => Decor[ChangedUtils.WorldGenRandNext(0, Decor.Length)];
    }

    public class RoomDefault : Room
    {
        public RoomDefault() : base(
            floorType: ModContent.TileType<Lab_TileTile>(),
            floorPaint: PaintID.None,
            wallType: [WallID.GoldBrick],
            wallPaint: PaintID.PurplePaint,
            hasPainting: true,
            hasChest: false,
            decor: [ModContent.TileType<WhiteLatexBookcases>()],
            singleDecor: -1,
            lanternStyle: 0,
            torchStyle: -1,
            chandelierStyle: -1,
            hasLightSwitch: false,
            hasFans: false
        )
        { }
    }

    public class RoomGenerator : Room
    {
        public RoomGenerator() : base(
            floorType: ModContent.TileType<Lab_TileTile>(),
            floorPaint: PaintID.None,
            wallType: [WallID.LeadBrick],
            wallPaint: PaintID.None,
            hasPainting: false,
            hasChest: false,
            decor: [ModContent.TileType<Generator>()],
            singleDecor: -1,
            lanternStyle: 0,
            torchStyle: -1,
            chandelierStyle: -1,
            hasLightSwitch: true,
            hasFans: false
        )
        { }
    }

    public class RoomGas : Room
    {
        public RoomGas() : base(
            floorType: ModContent.TileType<Lab_TileTile>(),
            floorPaint: PaintID.None,
            wallType: [WallID.LeadBrick],
            wallPaint: PaintID.None,
            hasPainting: true,
            hasChest: false,
            decor: [ModContent.TileType<RedGasTank>(), ModContent.TileType<BlueGasTank>()],
            singleDecor: -1,
            lanternStyle: 0,
            torchStyle: -1,
            chandelierStyle: -1,
            hasLightSwitch: false,
            hasFans: true
        )
        { }
    }

    public class RoomGreenhouse : Room
    {
        public RoomGreenhouse() : base(
            floorType: ModContent.TileType<Lab_TileTile>(),
            floorPaint: PaintID.None,
            wallType: [WallID.Glass],
            wallPaint: PaintID.None,
            hasPainting: false,
            hasChest: false,
            decor: [TileID.PottedPlants1, TileID.PottedPlants2],
            singleDecor: ModContent.TileType<WateringCan>(),
            lanternStyle: -1,
            torchStyle: -1,
            chandelierStyle: -1,
            hasLightSwitch: false,
            hasFans: false
        )
        { }
    }

    public class RoomLibrary : Room
    {
        public RoomLibrary() : base(
            floorType: TileID.WoodBlock,
            floorPaint: PaintID.None,
            wallType: [WallID.Planked],
            wallPaint: PaintID.None,
            hasPainting: true,
            hasChest: false,
            decor: [ModContent.TileType<MountBookest>(), TileID.Bookcases],
            singleDecor: -1,
            lanternStyle: 0,
            torchStyle: -1,
            chandelierStyle: -1,
            hasLightSwitch: false,
            hasFans: false
        )
        { }
    }

    public class RoomToy : Room
    {
        public RoomToy() : base(
            floorType: TileID.WoodBlock,
            floorPaint: PaintID.None,
            wallType: [WallID.SquigglesWallpaper],
            wallPaint: PaintID.None,
            hasPainting: true,
            hasChest: false,
            decor: [ModContent.TileType<Basketball>(), ModContent.TileType<PuroPlush>(), ModContent.TileType<SharkPlush>(), ModContent.TileType<FennecPlush>(), ModContent.TileType<Blocks>()],
            singleDecor: -1,
            lanternStyle: -1, //20
            torchStyle: -1,
            chandelierStyle: 47,
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
            [WallID.Slime],
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
        { }
    }

    public class RoomWhiteGoo : Room
    {
        public RoomWhiteGoo() : base(
            ModContent.TileType<WhiteLatexTile>(),
            PaintID.None,
            [WallID.Slime],
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
        { }
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
        { }
    }

    public class RoomElevator : Room
    {
        public RoomElevator() : base(
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
}
