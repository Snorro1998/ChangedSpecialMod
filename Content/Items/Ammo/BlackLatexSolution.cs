using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex;
using ChangedSpecialMod.Content.Walls.Latex;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Ammo
{
    public class BlackLatexSolution : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
            ItemID.Sets.SortingPriorityTerraforming[Type] = 101; // One past dirt solution
        }

        public override void SetDefaults()
        {
            Item.DefaultToSolution(ModContent.ProjectileType<BlackLatexSolutionProjectile>());
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Solutions;
        }
    }

    public class BlackLatexSolutionProjectile : ModProjectile
    {
        public static int ConversionType;

        public ref float Progress => ref Projectile.ai[0];
        // Solutions shot by the terraformer get an increase in conversion area size, indicated by the second AI parameter being set to 1
        public bool ShotFromTerraformer => Projectile.ai[1] == 1f;

        public override void SetStaticDefaults()
        {
            // Cache the conversion type here instead of repeately fetching it every frame
            ConversionType = ModContent.GetInstance<BlackLatexSolutionConversion>().Type;
        }

        public override void SetDefaults()
        {
            // This method quickly sets the projectile properties to match other sprays.
            Projectile.DefaultToSpray();
            Projectile.aiStyle = 0; // Here we set aiStyle back to 0 because we have custom AI code
        }

        public override bool? CanDamage() => false;

        public override void AI()
        {

            if (Projectile.timeLeft > 133)
                Projectile.timeLeft = 133;

            if (Projectile.owner == Main.myPlayer)
            {
                int size = ShotFromTerraformer ? 3 : 2;
                Point tileCenter = Projectile.Center.ToTileCoordinates();
                WorldGen.Convert(tileCenter.X, tileCenter.Y, ConversionType, size);
            }

            int spawnDustTreshold = 7;
            if (ShotFromTerraformer)
                spawnDustTreshold = 3;

            if (Progress > (float)spawnDustTreshold)
            {
                float dustScale = 1f;
                int dustType = DustID.Asphalt;

                if (Progress == spawnDustTreshold + 1)
                    dustScale = 0.2f;
                else if (Progress == spawnDustTreshold + 2)
                    dustScale = 0.4f;
                else if (Progress == spawnDustTreshold + 3)
                    dustScale = 0.6f;
                else if (Progress == spawnDustTreshold + 4)
                    dustScale = 0.8f;

                int dustArea = 0;
                if (ShotFromTerraformer)
                {
                    dustScale *= 1.2f;
                    dustArea = (int)(12f * dustScale);
                }

                Dust sprayDust = Dust.NewDustDirect(new Vector2(Projectile.position.X - dustArea, Projectile.position.Y - dustArea), Projectile.width + dustArea * 2, Projectile.height + dustArea * 2, dustType, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 100);
                sprayDust.noGravity = true;
                sprayDust.scale *= 1.75f * dustScale;
            }

            Progress++;
            Projectile.rotation += 0.3f * Projectile.direction;
        }
    }

    public class BlackLatexSolutionConversion : ModBiomeConversion
    {
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
        /*
        public override void PostSetupContent()
        {
            // Normal to black
            TileLoader.RegisterConversion(TileID.Dirt, Type, CreateConversion(ModContent.TileType<BlackLatexTile>()));
            TileLoader.RegisterConversion(TileID.Grass, Type, CreateConversion(ModContent.TileType<BlackLatexGrassTile>()));
            TileLoader.RegisterConversion(TileID.Mud, Type, CreateConversion(ModContent.TileType<BlackLatexMudTile>()));
            TileLoader.RegisterConversion(TileID.JungleGrass, Type, CreateConversion(ModContent.TileType<BlackLatexJungleGrassTile>()));
            TileLoader.RegisterConversion(TileID.Sand, Type, CreateConversion(ModContent.TileType<BlackLatexSandTile>()));
            TileLoader.RegisterConversion(TileID.Stone, Type, CreateConversion(ModContent.TileType<BlackLatexStoneTile>()));
            TileLoader.RegisterConversion(TileID.IceBlock, Type, CreateConversion(ModContent.TileType<BlackLatexIceTile>()));
            TileLoader.RegisterConversion(TileID.SnowBlock, Type, CreateConversion(ModContent.TileType<BlackLatexSnowTile>()));
            TileLoader.RegisterConversion(TileID.LivingWood, Type, CreateConversion(ModContent.TileType<BlackLatexLivingWoodTile>()));
            TileLoader.RegisterConversion(TileID.Stalactite, Type, CreateConversion(ModContent.TileType<BlackLatexStalactiteTile>()));

            WallLoader.RegisterConversion(WallID.DirtUnsafe, Type, ModContent.WallType<BlackLatexDirtWallUnsafe>());
            WallLoader.RegisterConversion(WallID.DirtUnsafe1, Type, ModContent.WallType<BlackLatexDirtWallUnsafe1>());
            WallLoader.RegisterConversion(WallID.Stone, Type, ModContent.WallType<BlackLatexStoneWall>());

            // White to black
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexGrassTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexGrassTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexMudTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexMudTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexJungleGrassTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexJungleGrassTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSandTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexSandTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexStoneTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexStoneTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexIceTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexIceTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSnowTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexSnowTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexLivingWoodTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexLivingWoodTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexStalactiteTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexStalactiteTile>()));

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), Type, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), Type, DestroyTile);

            WallLoader.RegisterConversion(ModContent.WallType<WhiteLatexDirtWallUnsafe>(), Type, ModContent.WallType<BlackLatexDirtWallUnsafe>());
            WallLoader.RegisterConversion(ModContent.WallType<WhiteLatexDirtWallUnsafe1>(), Type, ModContent.WallType<BlackLatexDirtWallUnsafe1>());
            WallLoader.RegisterConversion(ModContent.WallType<WhiteLatexStoneWall>(), Type, ModContent.WallType<BlackLatexStoneWall>());

            // Dry dirt to black
            TileLoader.RegisterConversion(ModContent.TileType<DryDirt>(), Type, CreateConversion(ModContent.TileType<BlackLatexTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<DryDirtGrassTile>(), Type, CreateConversion(ModContent.TileType<BlackLatexGrassTile>()));

            TileLoader.RegisterConversion(ModContent.TileType<DryDirtPlant>(), Type, DestroyTile);

            // Black to purity
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexTile>(), BiomeConversionID.Purity, TileID.Dirt);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexGrassTile>(), BiomeConversionID.Purity, TileID.Grass);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexMudTile>(), BiomeConversionID.Purity, TileID.Mud);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexJungleGrassTile>(), BiomeConversionID.Purity, TileID.JungleGrass);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSandTile>(), BiomeConversionID.Purity, TileID.Sand);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexStoneTile>(), BiomeConversionID.Purity, TileID.Stone);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexIceTile>(), BiomeConversionID.Purity, TileID.IceBlock);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSnowTile>(), BiomeConversionID.Purity, TileID.SnowBlock);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexLivingWoodTile>(), BiomeConversionID.Purity, TileID.LivingWood);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexStalactiteTile>(), BiomeConversionID.Purity, TileID.Stalactite);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), BiomeConversionID.Purity, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), BiomeConversionID.Purity, DestroyTile);

            WallLoader.RegisterConversion(ModContent.WallType<BlackLatexDirtWallUnsafe>(), BiomeConversionID.Purity, WallID.DirtUnsafe);
            WallLoader.RegisterConversion(ModContent.WallType<BlackLatexDirtWallUnsafe1>(), BiomeConversionID.Purity, WallID.DirtUnsafe1);
            WallLoader.RegisterConversion(ModContent.WallType<BlackLatexStoneWall>(), BiomeConversionID.Purity, WallID.Stone);

            // Black to corruption
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexTile>(), BiomeConversionID.Corruption, TileID.Dirt);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexGrassTile>(), BiomeConversionID.Corruption, TileID.CorruptGrass);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSandTile>(), BiomeConversionID.Corruption, TileID.Ebonsand);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexStoneTile>(), BiomeConversionID.Corruption, TileID.Ebonstone);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexIceTile>(), BiomeConversionID.Corruption, TileID.CorruptIce);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSnowTile>(), BiomeConversionID.Corruption, TileID.SnowBlock);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), BiomeConversionID.Corruption, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), BiomeConversionID.Corruption, DestroyTile);

            // Black to crimson
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexTile>(), BiomeConversionID.Crimson, TileID.Dirt);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexGrassTile>(), BiomeConversionID.Crimson, TileID.CrimsonGrass);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSandTile>(), BiomeConversionID.Crimson, TileID.Crimsand);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexStoneTile>(), BiomeConversionID.Crimson, TileID.Crimstone);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexIceTile>(), BiomeConversionID.Crimson, TileID.FleshIce);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSnowTile>(), BiomeConversionID.Crimson, TileID.SnowBlock);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), BiomeConversionID.Crimson, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), BiomeConversionID.Crimson, DestroyTile);

            // Black to hallow
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexTile>(), BiomeConversionID.Hallow, TileID.Dirt);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexGrassTile>(), BiomeConversionID.Hallow, TileID.HallowedGrass);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSandTile>(), BiomeConversionID.Hallow, TileID.Pearlsand);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexStoneTile>(), BiomeConversionID.Hallow, TileID.Pearlstone);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexIceTile>(), BiomeConversionID.Hallow, TileID.HallowedIce);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSnowTile>(), BiomeConversionID.Hallow, TileID.SnowBlock);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), BiomeConversionID.Hallow, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), BiomeConversionID.Hallow, DestroyTile);
        }
        */
    }
}
