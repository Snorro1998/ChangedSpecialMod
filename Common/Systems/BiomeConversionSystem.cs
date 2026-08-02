using ChangedSpecialMod.Content.Items.Ammo;
using ChangedSpecialMod.Content.Items.Placeable.Latex.Black;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using ChangedSpecialMod.Content.Tiles.Latex.White;
using ChangedSpecialMod.Content.Tiles.Plants;
using ChangedSpecialMod.Content.Walls.Latex.Black;
using ChangedSpecialMod.Content.Walls.Latex.White;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class BiomeConversion
    {
        public int normalBlockType;
        public int corruptionBlockType;
        public int crimsonBlockType;
        public int hallowBlockType;
        public int blackLatexBlockType;
        public int whiteLatexBlockType;
        public int dryDirtBlockType;

        public BiomeConversion(int normalBlockType, int corruptionBlockType, int crimsonBlockType, int hallowBlockType, int blackLatexBlockType, int whiteLatexBlockType, int dryDirtBlockType)
        {
            this.normalBlockType = normalBlockType;
            this.corruptionBlockType = corruptionBlockType;
            this.crimsonBlockType = crimsonBlockType;
            this.hallowBlockType = hallowBlockType;
            this.blackLatexBlockType = blackLatexBlockType;
            this.whiteLatexBlockType = whiteLatexBlockType;
            this.dryDirtBlockType = dryDirtBlockType;
        }
    }

    public enum BiomeType
    {
        Normal,
        Desert,
        Jungle,
        Snow
    }

    public class BiomeConversionSystem : ModSystem
    {
        private static List<BiomeConversion> conversions;
        private static List<BiomeConversion> wallConversions;

        private sealed class MergeableTileGlobalTile : GlobalTile
        {
            public override bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak)
            {
                /*
                // Custom plant framing
                for (int k = 0; k < PlantTypes.Length; k++)
                    if (type == PlantTypes[k])
                    {
                        PlantFrame(i, j);
                        return false;
                    }
                */
                
                if (type == ModContent.TileType<DryDirtPlant>())
                {
                    PlantFrame(i, j);
                    return false;
                }

                // Custom vine framing
                if (type == TileID.Vines || type == TileID.CrimsonVines || type == TileID.HallowedVines || type == ModContent.TileType<BlackLatexVines>() || type == ModContent.TileType<WhiteLatexVines>())
                    VineFrame(i, j);

                return base.TileFrame(i, j, type, ref resetFrame, ref noBreak);
            }

            internal static void PlantFrame(int x, int y)
            {
                if (x < 0 || x >= Main.maxTilesX)
                    return;

                if (y < 0 || y >= Main.maxTilesY)
                    return;

                // If the tile below is off the bottom of the map, then assume it's invalid placement
                var tile = Main.tile[x, y];
                int plantType = tile.TileType;
                if (y + 1 >= Main.maxTilesY)
                {
                    WorldGen.KillTile(x, y);
                    return;
                }

                // If tile below is not elligible for growing plants, we kill the tile immediately
                var below = Main.tile[x, y + 1];
                if (!below.HasTile || !below.HasUnactuatedTile || below.IsHalfBlock || below.Slope != SlopeType.Solid)
                {
                    WorldGen.KillTile(x, y);
                    return;
                }

                // Check if tile below is valid for given grass type, If so we don't need to update this
                var belowTileType = (int)below.TileType;
                if (belowTileType == ModContent.TileType<DryDirtGrassTile>())
                    return;
                //if (PlantValidGrounds[plantType] is not null && PlantValidGrounds[plantType].Contains(belowTileType))
                //    return;

                var latexTiles = GetLatexBlocks();
                if (latexTiles.Contains(belowTileType))
                {
                    WorldGen.KillTile(x, y);
                    return;
                }

                var newPlantType = plantType;

                
                if ((plantType == TileID.Plants || plantType == TileID.Plants2) && belowTileType != TileID.Grass && tile.TileFrameX >= 162)
                {
                    Main.tile[x, y].TileFrameX = 126;
                }
                if (plantType == TileID.JunglePlants2 && belowTileType != TileID.JungleGrass && tile.TileFrameX >= 162)
                {
                    Main.tile[x, y].TileFrameX = 126;
                }

                #region Biome Grass Replacements
                if (belowTileType == TileID.CorruptGrass)
                {
                    newPlantType = TileID.CorruptPlants;
                    if (tile.TileFrameX >= 162)
                    {
                        Main.tile[x, y].TileFrameX = 126;
                    }
                }
                else if (belowTileType == TileID.Grass)
                {
                    newPlantType = (plantType == TileID.HallowedPlants2 ? TileID.Plants2 : TileID.Plants);
                }
                else if (belowTileType == TileID.HallowedGrass)
                {
                    newPlantType = (plantType == TileID.Plants2 ? TileID.HallowedPlants2 : TileID.HallowedPlants);
                }
                else if (belowTileType == TileID.CrimsonGrass)
                {
                    newPlantType = TileID.CrimsonPlants;
                }
                else if (belowTileType == TileID.MushroomGrass)
                {
                    newPlantType = TileID.MushroomPlants;
                    while (Main.tile[x, y].TileFrameX > 72)
                    {
                        Main.tile[x, y].TileFrameX -= 72;
                    }
                }
                else if (belowTileType == ModContent.TileType<DryDirtPlant>())
                {
                    newPlantType = ModContent.TileType<DryDirtPlant>();
                    /*
                    var isShortPlant = plantType == TileID.Plants ||
                        plantType == TileID.CorruptPlants ||
                        plantType == TileID.CrimsonPlants ||
                        plantType == TileID.HallowedPlants ||
                        plantType == TileID.MushroomPlants ||
                        plantType == TileID.JunglePlants;
                    newPlantType = isShortPlant ? ModContent.TileType<DryDirtPlant>() : ModContent.TileType<DryDirtPlant>();
                    */
                }
                #endregion

                // If the tile type is not the same as the plant type, then set it equal. Otherwise, destroy it.
                if (plantType != newPlantType)
                {
                    Main.tile[x, y].TileType = (ushort)newPlantType;
                }
            }

            internal static void VineFrame(int x, int y)
            {
                if (x < 0 || x >= Main.maxTilesX)
                    return;
                if (y < 0 || y >= Main.maxTilesY)
                    return;

                var VineToGrass = new Dictionary<ushort, ushort>
                {
                    [TileID.Vines] = TileID.Grass,
                    [TileID.Vines] = TileID.LeafBlock,
                    [TileID.CrimsonVines] = TileID.CrimsonGrass,
                    [TileID.HallowedVines] = TileID.HallowedGrass,
                    [(ushort)ModContent.TileType<BlackLatexVines>()] = (ushort)ModContent.TileType<BlackLatexGrassTile>(),
                    [(ushort)ModContent.TileType<WhiteLatexVines>()] = (ushort)ModContent.TileType<WhiteLatexGrassTile>(),
                };

                var tile = Main.tile[x, y];
                int myType = tile.TileType;

                // Get the type of the tile above this vine. If that tile doesn't exist, just assume it's another vine.
                var north = y <= 0 ? default : Main.tile[x, y - 1];
                var northType = north == default(Tile) ? myType : !north.HasTile || north.BottomSlope ? -1 : north.TileType;

                // Make this vine match the tile above it if that's another vine or a grass tile.
                var vines = VineToGrass.Keys.ToArray();
                for (var i = 0; i < vines.Length; ++i)
                {
                    var correspondingGrass = VineToGrass[vines[i]];
                    if (myType != vines[i] && (northType == correspondingGrass || northType == vines[i]))
                    {
                        Main.tile[x, y].TileType = vines[i];
                        WorldGen.SquareTileFrame(x, y, true);
                        return;
                    }
                }

                // If the tile above is an identical vine, nothing else needs to be done.
                if (northType == myType)
                    return;

                // If the tile above isn't sloped correctly or otherwise isn't a valid anchor for this vine, check whether the vine must die.
                var tileMustDie = northType == -1;
                if (northType != -1)
                {
                    // Vanilla vines can hang from vanilla grass and vanilla leaf blocks.
                    if (myType == TileID.Vines && northType != TileID.Grass && northType != TileID.LeafBlock)
                    {
                        tileMustDie = true;
                    }
                    else if (myType != TileID.Vines)
                    {
                        for (var i = 0; i < vines.Length; ++i)
                        {
                            // Not matching grass? Die.
                            if (myType == vines[i] && northType != VineToGrass[vines[i]])
                            {
                                tileMustDie = true;
                                break;
                            }
                        }
                    }
                }

                if (tileMustDie)
                    WorldGen.KillTile(x, y, false, false, false);
            }
        }

        private static TileLoader.ConvertTile CreateConversion(int targetTile) =>
        (i, j, type, conversionType) =>
        {
            WorldGen.ConvertTile(i, j, targetTile);
            return false;
        };

        private static bool DestroyTile(int i, int j, int type, int conversionType)
        {
            WorldGen.KillTile(i, j);
            return false;
        }

        public static List<int> GetLatexBlocks()
        {
            var blockTypes = new List<int>();

            foreach (var conversion in conversions)
            {
                blockTypes.Add(conversion.blackLatexBlockType);
                blockTypes.Add(conversion.whiteLatexBlockType);
            }

            return blockTypes;
        }

        public static List<int> GetBlackLatexBlocks()
        {
            return conversions.Select(x => x.blackLatexBlockType).ToList();
        }

        public static List<int> GetWhiteLatexBlocks()
        {
            return conversions.Select(x => x.whiteLatexBlockType).ToList();
        }

        public static List<int> GetDryDirtBlocks()
        {
            return new List<int>
            {
                ModContent.TileType<DryDirtGrassTile>(),
                ModContent.TileType<DryDirt>()
            };
        }

        public static int GetInfectedBlockType(int blockType, GooType infectionType, bool duringWorldGeneration)
        {
            var conversionType = conversions.FirstOrDefault(x => x.normalBlockType == blockType);

            if (conversionType == null)
            {
                if (!duringWorldGeneration)
                    return -1;

                conversionType = conversions.FirstOrDefault(x => x.corruptionBlockType == blockType);
                if (conversionType == null)
                    conversionType = conversions.FirstOrDefault(x => x.crimsonBlockType == blockType);
                if (conversionType == null)
                    conversionType = conversions.FirstOrDefault(x => x.hallowBlockType == blockType);
                if (conversionType == null)
                    return -1;
            }

            switch (infectionType)
            {
                case GooType.Black:
                    return conversionType.blackLatexBlockType;
                case GooType.White:
                    return conversionType.whiteLatexBlockType;
                default:
                    return -1;
            }
        }

        private void SetupConversions()
        {
            conversions = new List<BiomeConversion>()
            {
                // Grass
                new BiomeConversion(
                    TileID.Grass, 
                    TileID.CorruptGrass, 
                    TileID.CrimsonGrass, 
                    TileID.HallowedGrass,
                    ModContent.TileType<BlackLatexGrassTile>(), 
                    ModContent.TileType<WhiteLatexGrassTile>(), 
                    ModContent.TileType<DryDirtGrassTile>()),
                
                // Dirt
                new BiomeConversion(
                    TileID.Dirt, 
                    TileID.Dirt, 
                    TileID.Dirt, 
                    TileID.Dirt,
                    ModContent.TileType<BlackLatexTile>(), 
                    ModContent.TileType<WhiteLatexTile>(), 
                    ModContent.TileType<DryDirt>()),

                // Jungle grass
                new BiomeConversion(
                    TileID.JungleGrass,
                    TileID.CorruptJungleGrass,
                    TileID.CrimsonJungleGrass,
                    TileID.JungleGrass,
                    ModContent.TileType<BlackLatexJungleGrassTile>(),
                    ModContent.TileType<WhiteLatexJungleGrassTile>(),
                    TileID.JungleGrass),

                // Mud
                new BiomeConversion(
                    TileID.Mud,
                    TileID.Mud,
                    TileID.Mud,
                    TileID.Mud,
                    ModContent.TileType<BlackLatexMudTile>(),
                    ModContent.TileType<WhiteLatexMudTile>(),
                    TileID.Mud),

                // Stone
                new BiomeConversion(
                    TileID.Stone,
                    TileID.Ebonstone,
                    TileID.Crimstone,
                    TileID.Pearlstone,
                    ModContent.TileType<BlackLatexStoneTile>(),
                    ModContent.TileType<WhiteLatexStoneTile>(),
                    TileID.Stone),

                // Sand
                new BiomeConversion(
                    TileID.Sand,
                    TileID.Ebonsand,
                    TileID.Crimsand,
                    TileID.Pearlsand,
                    ModContent.TileType<BlackLatexSandTile>(),
                    ModContent.TileType<WhiteLatexSandTile>(),
                    TileID.Sand),

                // Hardened Sand
                new BiomeConversion(
                    TileID.HardenedSand,
                    TileID.CorruptHardenedSand,
                    TileID.CrimsonHardenedSand,
                    TileID.HallowHardenedSand,
                    ModContent.TileType<BlackLatexHardenedSandTile>(),
                    ModContent.TileType<WhiteLatexHardenedSandTile>(),
                    TileID.HardenedSand),

                // Sandstone
                new BiomeConversion(
                    TileID.Sandstone,
                    TileID.CorruptSandstone,
                    TileID.CrimsonSandstone,
                    TileID.HallowSandstone,
                    ModContent.TileType<BlackLatexSandstoneTile>(),
                    ModContent.TileType<WhiteLatexSandstoneTile>(),
                    TileID.Sandstone),

                // Snow
                new BiomeConversion(
                    TileID.SnowBlock,
                    TileID.SnowBlock,
                    TileID.SnowBlock,
                    TileID.SnowBlock,
                    ModContent.TileType<BlackLatexSnowTile>(),
                    ModContent.TileType<WhiteLatexSnowTile>(),
                    TileID.SnowBlock),

                // Ice
                new BiomeConversion(
                    TileID.IceBlock,
                    TileID.CorruptIce,
                    TileID.FleshIce,
                    TileID.HallowedIce,
                    ModContent.TileType<BlackLatexIceTile>(),
                    ModContent.TileType<WhiteLatexIceTile>(),
                    TileID.IceBlock),
                /*
                // Living wood
                new BiomeConversion(
                    TileID.LivingWood,
                    TileID.LivingWood,
                    TileID.LivingWood,
                    TileID.LivingWood,
                    ModContent.TileType<BlackLatexLivingWoodTile>(),
                    ModContent.TileType<WhiteLatexLivingWoodTile>(),
                    TileID.LivingWood)
                */
            };

            wallConversions = new List<BiomeConversion>()
            {
                // Dirt unsafe
                new BiomeConversion(
                    WallID.DirtUnsafe,
                    WallID.DirtUnsafe,
                    WallID.DirtUnsafe,
                    WallID.DirtUnsafe,
                    ModContent.WallType<BlackLatexDirtWallUnsafe>(),
                    ModContent.WallType<WhiteLatexDirtWallUnsafe>(),
                    WallID.DirtUnsafe),

                // Dirt unsafe 1
                new BiomeConversion(
                    WallID.DirtUnsafe1,
                    WallID.DirtUnsafe1,
                    WallID.DirtUnsafe1,
                    WallID.DirtUnsafe1,
                    ModContent.WallType<BlackLatexDirtWallUnsafe1>(),
                    ModContent.WallType<WhiteLatexDirtWallUnsafe1>(),
                    WallID.DirtUnsafe1),

                // Dirt unsafe 2
                new BiomeConversion(
                    WallID.DirtUnsafe2,
                    WallID.DirtUnsafe2,
                    WallID.DirtUnsafe2,
                    WallID.DirtUnsafe2,
                    ModContent.WallType<BlackLatexDirtWallUnsafe2>(),
                    ModContent.WallType<WhiteLatexDirtWallUnsafe2>(),
                    WallID.DirtUnsafe2),

                // Cave6 unsafe
                new BiomeConversion(
                    WallID.Cave6Unsafe,
                    WallID.Cave6Unsafe,
                    WallID.Cave6Unsafe,
                    WallID.Cave6Unsafe,
                    ModContent.WallType<BlackLatexCave6WallUnsafe>(),
                    ModContent.WallType<WhiteLatexCave6WallUnsafe>(),
                    WallID.Cave6Unsafe),

                // Mud unsafe
                new BiomeConversion(
                    WallID.MudUnsafe,
                    WallID.MudUnsafe,
                    WallID.MudUnsafe,
                    WallID.MudUnsafe,
                    ModContent.WallType<BlackLatexMudWallUnsafe>(),
                    ModContent.WallType<WhiteLatexMudWallUnsafe>(),
                    WallID.MudUnsafe),

                // Grass unsafe
                new BiomeConversion(
                    WallID.GrassUnsafe,
                    WallID.CorruptGrassUnsafe,
                    WallID.CrimsonGrassUnsafe,
                    WallID.HallowedGrassUnsafe,
                    ModContent.WallType<BlackLatexGrassWallUnsafe>(),
                    ModContent.WallType<WhiteLatexGrassWallUnsafe>(),
                    WallID.GrassUnsafe),

                // Flower unsafe
                new BiomeConversion(
                    WallID.FlowerUnsafe,
                    WallID.FlowerUnsafe,
                    WallID.FlowerUnsafe,
                    WallID.FlowerUnsafe,
                    ModContent.WallType<BlackLatexFlowerWallUnsafe>(),
                    ModContent.WallType<WhiteLatexFlowerWallUnsafe>(),
                    WallID.FlowerUnsafe),

                // Stone
                new BiomeConversion(
                    WallID.Stone,
                    WallID.EbonstoneUnsafe,
                    WallID.CrimstoneUnsafe,
                    WallID.PearlstoneBrickUnsafe,
                    ModContent.WallType<BlackLatexStoneWall>(),
                    ModContent.WallType<WhiteLatexStoneWall>(),
                    WallID.Stone),

                // Sandstone
                new BiomeConversion(
                    WallID.Sandstone,
                    WallID.CorruptSandstone,
                    WallID.CrimsonSandstone,
                    WallID.HallowSandstone,
                    ModContent.WallType<BlackLatexSandstoneWall>(),
                    ModContent.WallType<WhiteLatexSandstoneWall>(),
                    WallID.Sandstone),

                // Snow
                new BiomeConversion(
                    WallID.SnowWallUnsafe,
                    WallID.SnowWallUnsafe,
                    WallID.SnowWallUnsafe,
                    WallID.SnowWallUnsafe,
                    ModContent.WallType<BlackLatexSnowWallUnsafe>(),
                    ModContent.WallType<WhiteLatexSnowWallUnsafe>(),
                    WallID.SnowWallUnsafe),

                // Ice
                new BiomeConversion(
                    WallID.IceUnsafe,
                    WallID.IceUnsafe,
                    WallID.IceUnsafe,
                    WallID.IceUnsafe,
                    ModContent.WallType<BlackLatexIceWallUnsafe>(),
                    ModContent.WallType<WhiteLatexIceWallUnsafe>(),
                    WallID.IceUnsafe),
            };
        }

        public override void PostSetupContent()
        {
            if (conversions == null || conversions.Count == 0)
                SetupConversions();

            var conversionBlackLatex = ModContent.GetInstance<BlackLatexSolutionConversion>().Type;
            var conversionWhiteLatex = ModContent.GetInstance<WhiteLatexSolutionConversion>().Type;
            var conversionDryDirt = ModContent.GetInstance<DryDirtSolutionConversion>().Type;

            foreach (var conversion in conversions)
            {
                // Normal to...
                if (conversion.normalBlockType != -1)
                {
                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.normalBlockType, conversionBlackLatex, CreateConversion(conversion.blackLatexBlockType));

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.normalBlockType, conversionWhiteLatex, CreateConversion(conversion.whiteLatexBlockType));

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        TileLoader.RegisterConversion(conversion.normalBlockType, conversionDryDirt, CreateConversion(conversion.dryDirtBlockType));
                }

                // Corruption to...
                if (conversion.corruptionBlockType != -1)
                {
                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.corruptionBlockType, conversionBlackLatex, CreateConversion(conversion.blackLatexBlockType));

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.corruptionBlockType, conversionWhiteLatex, CreateConversion(conversion.whiteLatexBlockType));

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        TileLoader.RegisterConversion(conversion.corruptionBlockType, conversionDryDirt, CreateConversion(conversion.dryDirtBlockType));
                }

                // Crimson to...
                if (conversion.crimsonBlockType != -1)
                {
                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.crimsonBlockType, conversionBlackLatex, CreateConversion(conversion.blackLatexBlockType));

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.crimsonBlockType, conversionWhiteLatex, CreateConversion(conversion.whiteLatexBlockType));

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        TileLoader.RegisterConversion(conversion.crimsonBlockType, conversionDryDirt, CreateConversion(conversion.dryDirtBlockType));
                }

                // Hallow to...
                if (conversion.hallowBlockType != -1)
                {
                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.hallowBlockType, conversionBlackLatex, CreateConversion(conversion.blackLatexBlockType));

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.hallowBlockType, conversionWhiteLatex, CreateConversion(conversion.whiteLatexBlockType));

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        TileLoader.RegisterConversion(conversion.hallowBlockType, conversionDryDirt, CreateConversion(conversion.dryDirtBlockType));
                }

                // Black to...
                if (conversion.blackLatexBlockType != -1)
                {
                    // Normal
                    if (conversion.normalBlockType != -1)
                        TileLoader.RegisterConversion(conversion.blackLatexBlockType, BiomeConversionID.Purity, CreateConversion(conversion.normalBlockType));

                    // Corruption
                    if (conversion.corruptionBlockType != -1)
                        TileLoader.RegisterConversion(conversion.blackLatexBlockType, BiomeConversionID.Corruption, CreateConversion(conversion.corruptionBlockType));

                    // Crimson
                    if (conversion.crimsonBlockType != -1)
                        TileLoader.RegisterConversion(conversion.blackLatexBlockType, BiomeConversionID.Crimson, CreateConversion(conversion.crimsonBlockType));

                    // Hallow
                    if (conversion.hallowBlockType != -1)
                        TileLoader.RegisterConversion(conversion.blackLatexBlockType, BiomeConversionID.Hallow, CreateConversion(conversion.hallowBlockType));

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.blackLatexBlockType, conversionWhiteLatex, CreateConversion(conversion.whiteLatexBlockType));

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        TileLoader.RegisterConversion(conversion.blackLatexBlockType, conversionDryDirt, CreateConversion(conversion.dryDirtBlockType));
                }

                // White to...
                if (conversion.whiteLatexBlockType != -1)
                {
                    // Normal
                    if (conversion.normalBlockType != -1)
                        TileLoader.RegisterConversion(conversion.whiteLatexBlockType, BiomeConversionID.Purity, CreateConversion(conversion.normalBlockType));

                    // Corruption
                    if (conversion.corruptionBlockType != -1)
                        TileLoader.RegisterConversion(conversion.whiteLatexBlockType, BiomeConversionID.Corruption, CreateConversion(conversion.corruptionBlockType));

                    // Crimson
                    if (conversion.crimsonBlockType != -1)
                        TileLoader.RegisterConversion(conversion.whiteLatexBlockType, BiomeConversionID.Crimson, CreateConversion(conversion.crimsonBlockType));

                    // Hallow
                    if (conversion.hallowBlockType != -1)
                        TileLoader.RegisterConversion(conversion.whiteLatexBlockType, BiomeConversionID.Hallow, CreateConversion(conversion.hallowBlockType));

                    // Black
                    if (conversion.whiteLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.whiteLatexBlockType, conversionBlackLatex, CreateConversion(conversion.blackLatexBlockType));

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        TileLoader.RegisterConversion(conversion.whiteLatexBlockType, conversionDryDirt, CreateConversion(conversion.dryDirtBlockType));
                }

                // Dry dirt to...
                if (conversion.dryDirtBlockType != -1)
                {
                    // Normal
                    if (conversion.normalBlockType != -1)
                        TileLoader.RegisterConversion(conversion.dryDirtBlockType, BiomeConversionID.Purity, CreateConversion(conversion.normalBlockType));

                    // Corruption
                    if (conversion.corruptionBlockType != -1)
                        TileLoader.RegisterConversion(conversion.dryDirtBlockType, BiomeConversionID.Corruption, CreateConversion(conversion.corruptionBlockType));

                    // Crimson
                    if (conversion.crimsonBlockType != -1)
                        TileLoader.RegisterConversion(conversion.dryDirtBlockType, BiomeConversionID.Crimson, CreateConversion(conversion.crimsonBlockType));

                    // Hallow
                    if (conversion.hallowBlockType != -1)
                        TileLoader.RegisterConversion(conversion.dryDirtBlockType, BiomeConversionID.Hallow, CreateConversion(conversion.hallowBlockType));

                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.dryDirtBlockType, conversionBlackLatex, CreateConversion(conversion.blackLatexBlockType));

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        TileLoader.RegisterConversion(conversion.dryDirtBlockType, conversionWhiteLatex, CreateConversion(conversion.whiteLatexBlockType));
                }
            }

            foreach (var conversion in wallConversions)
            {
                // Normal to...
                if (conversion.normalBlockType != -1)
                {
                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.normalBlockType, conversionBlackLatex, conversion.blackLatexBlockType);

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.normalBlockType, conversionWhiteLatex, conversion.whiteLatexBlockType);

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        WallLoader.RegisterConversion(conversion.normalBlockType, conversionDryDirt, conversion.dryDirtBlockType);
                }

                // Corruption to...
                if (conversion.corruptionBlockType != -1)
                {
                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.corruptionBlockType, conversionBlackLatex, conversion.blackLatexBlockType);

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.corruptionBlockType, conversionWhiteLatex, conversion.whiteLatexBlockType);

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        WallLoader.RegisterConversion(conversion.corruptionBlockType, conversionDryDirt, conversion.dryDirtBlockType);
                }

                // Crimson to...
                if (conversion.crimsonBlockType != -1)
                {
                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.crimsonBlockType, conversionBlackLatex, conversion.blackLatexBlockType);

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.crimsonBlockType, conversionWhiteLatex, conversion.whiteLatexBlockType);

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        WallLoader.RegisterConversion(conversion.crimsonBlockType, conversionDryDirt, conversion.dryDirtBlockType);
                }

                // Hallow to...
                if (conversion.hallowBlockType != -1)
                {
                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.hallowBlockType, conversionBlackLatex, conversion.blackLatexBlockType);

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.hallowBlockType, conversionWhiteLatex, conversion.whiteLatexBlockType);

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        WallLoader.RegisterConversion(conversion.hallowBlockType, conversionDryDirt, conversion.dryDirtBlockType);
                }

                // Black to...
                if (conversion.blackLatexBlockType != -1)
                {
                    // Normal
                    if (conversion.normalBlockType != -1)
                        WallLoader.RegisterConversion(conversion.blackLatexBlockType, BiomeConversionID.Purity, conversion.normalBlockType);

                    // Corruption
                    if (conversion.corruptionBlockType != -1)
                        WallLoader.RegisterConversion(conversion.blackLatexBlockType, BiomeConversionID.Corruption, conversion.corruptionBlockType);

                    // Crimson
                    if (conversion.crimsonBlockType != -1)
                        WallLoader.RegisterConversion(conversion.blackLatexBlockType, BiomeConversionID.Crimson, conversion.crimsonBlockType);

                    // Hallow
                    if (conversion.hallowBlockType != -1)
                        WallLoader.RegisterConversion(conversion.blackLatexBlockType, BiomeConversionID.Hallow, conversion.hallowBlockType);

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.blackLatexBlockType, conversionWhiteLatex, conversion.whiteLatexBlockType);

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        WallLoader.RegisterConversion(conversion.blackLatexBlockType, conversionDryDirt, conversion.dryDirtBlockType);
                }

                // White to...
                if (conversion.whiteLatexBlockType != -1)
                {
                    // Normal
                    if (conversion.normalBlockType != -1)
                        WallLoader.RegisterConversion(conversion.whiteLatexBlockType, BiomeConversionID.Purity, conversion.normalBlockType);

                    // Corruption
                    if (conversion.corruptionBlockType != -1)
                        WallLoader.RegisterConversion(conversion.whiteLatexBlockType, BiomeConversionID.Corruption, conversion.corruptionBlockType);

                    // Crimson
                    if (conversion.crimsonBlockType != -1)
                        WallLoader.RegisterConversion(conversion.whiteLatexBlockType, BiomeConversionID.Crimson, conversion.crimsonBlockType);

                    // Hallow
                    if (conversion.hallowBlockType != -1)
                        WallLoader.RegisterConversion(conversion.whiteLatexBlockType, BiomeConversionID.Hallow, conversion.hallowBlockType);

                    // Black
                    if (conversion.whiteLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.whiteLatexBlockType, conversionBlackLatex, conversion.blackLatexBlockType);

                    // Dry dirt
                    if (conversion.dryDirtBlockType != -1)
                        WallLoader.RegisterConversion(conversion.whiteLatexBlockType, conversionDryDirt, conversion.dryDirtBlockType);
                }

                // Dry dirt to...
                if (conversion.dryDirtBlockType != -1)
                {
                    // Normal
                    if (conversion.normalBlockType != -1)
                        WallLoader.RegisterConversion(conversion.dryDirtBlockType, BiomeConversionID.Purity, conversion.normalBlockType);

                    // Corruption
                    if (conversion.corruptionBlockType != -1)
                        WallLoader.RegisterConversion(conversion.dryDirtBlockType, BiomeConversionID.Corruption, conversion.corruptionBlockType);

                    // Crimson
                    if (conversion.crimsonBlockType != -1)
                        WallLoader.RegisterConversion(conversion.dryDirtBlockType, BiomeConversionID.Crimson, conversion.crimsonBlockType);

                    // Hallow
                    if (conversion.hallowBlockType != -1)
                        WallLoader.RegisterConversion(conversion.dryDirtBlockType, BiomeConversionID.Hallow, conversion.hallowBlockType);

                    // Black
                    if (conversion.blackLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.dryDirtBlockType, conversionBlackLatex, conversion.blackLatexBlockType);

                    // White
                    if (conversion.whiteLatexBlockType != -1)
                        WallLoader.RegisterConversion(conversion.dryDirtBlockType, conversionWhiteLatex, conversion.whiteLatexBlockType);
                }
            }

            // Manually add these tiles to get destroyed

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), BiomeConversionID.Purity, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), BiomeConversionID.Corruption, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), BiomeConversionID.Crimson, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), BiomeConversionID.Hallow, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), conversionWhiteLatex, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), conversionDryDirt, DestroyTile);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), BiomeConversionID.Purity, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), BiomeConversionID.Corruption, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), BiomeConversionID.Crimson, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), BiomeConversionID.Hallow, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), conversionWhiteLatex, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), conversionDryDirt, DestroyTile);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), BiomeConversionID.Purity, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), BiomeConversionID.Corruption, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), BiomeConversionID.Crimson, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), BiomeConversionID.Hallow, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), conversionBlackLatex, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), conversionDryDirt, DestroyTile);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), BiomeConversionID.Purity, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), BiomeConversionID.Corruption, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), BiomeConversionID.Crimson, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), BiomeConversionID.Hallow, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), conversionBlackLatex, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), conversionDryDirt, DestroyTile);
        }
    }
}
