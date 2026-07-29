using ChangedSpecialMod.Content.Items.Ammo;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using ChangedSpecialMod.Content.Tiles.Latex.White;
using ChangedSpecialMod.Content.Walls.Latex.Black;
using ChangedSpecialMod.Content.Walls.Latex.White;
using System.Collections.Generic;
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

    public class BiomeConversionSystem : ModSystem
    {
        private List<BiomeConversion> conversions;
        private List<BiomeConversion> wallConversions;

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

                // Dirt
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
