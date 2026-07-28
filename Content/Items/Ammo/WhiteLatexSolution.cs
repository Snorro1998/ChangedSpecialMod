using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Ammo
{
    public class WhiteLatexSolution : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
            ItemID.Sets.SortingPriorityTerraforming[Type] = 101; // One past dirt solution
        }

        public override void SetDefaults()
        {
            Item.DefaultToSolution(ModContent.ProjectileType<WhiteLatexSolutionProjectile>());
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Solutions;
        }
    }

    public class WhiteLatexSolutionProjectile : ModProjectile
    {
        public static int ConversionType;

        public ref float Progress => ref Projectile.ai[0];
        // Solutions shot by the terraformer get an increase in conversion area size, indicated by the second AI parameter being set to 1
        public bool ShotFromTerraformer => Projectile.ai[1] == 1f;

        public override void SetStaticDefaults()
        {
            // Cache the conversion type here instead of repeately fetching it every frame
            ConversionType = ModContent.GetInstance<WhiteLatexSolutionConversion>().Type;
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
                int dustType = DustID.SnowSpray;

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

    public class WhiteLatexSolutionConversion : ModBiomeConversion
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
            // Normal to white
            TileLoader.RegisterConversion(TileID.Dirt, Type, CreateConversion(ModContent.TileType<WhiteLatexTile>()));
            TileLoader.RegisterConversion(TileID.Grass, Type, CreateConversion(ModContent.TileType<WhiteLatexGrassTile>()));
            TileLoader.RegisterConversion(TileID.Mud, Type, CreateConversion(ModContent.TileType<WhiteLatexMudTile>()));
            TileLoader.RegisterConversion(TileID.JungleGrass, Type, CreateConversion(ModContent.TileType<WhiteLatexJungleGrassTile>()));
            TileLoader.RegisterConversion(TileID.Sand, Type, CreateConversion(ModContent.TileType<WhiteLatexSandTile>()));
            TileLoader.RegisterConversion(TileID.Stone, Type, CreateConversion(ModContent.TileType<WhiteLatexStoneTile>()));
            TileLoader.RegisterConversion(TileID.IceBlock, Type, CreateConversion(ModContent.TileType<WhiteLatexIceTile>()));
            TileLoader.RegisterConversion(TileID.SnowBlock, Type, CreateConversion(ModContent.TileType<WhiteLatexSnowTile>()));
            TileLoader.RegisterConversion(TileID.LivingWood, Type, CreateConversion(ModContent.TileType<WhiteLatexLivingWoodTile>()));
            TileLoader.RegisterConversion(TileID.Stalactite, Type, CreateConversion(ModContent.TileType<WhiteLatexStalactiteTile>()));

            // Black to white
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexGrassTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexGrassTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexMudTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexMudTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexJungleGrassTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexJungleGrassTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSandTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexSandTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexStoneTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexStoneTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexIceTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexIceTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSnowTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexSnowTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexLivingWoodTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexLivingWoodTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexStalactiteTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexStalactiteTile>()));

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>(), Type, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), Type, DestroyTile);

            // Dry dirt to white
            TileLoader.RegisterConversion(ModContent.TileType<DryDirt>(), Type, CreateConversion(ModContent.TileType<WhiteLatexTile>()));
            TileLoader.RegisterConversion(ModContent.TileType<DryDirtGrassTile>(), Type, CreateConversion(ModContent.TileType<WhiteLatexGrassTile>()));

            TileLoader.RegisterConversion(ModContent.TileType<DryDirtPlant>(), Type, DestroyTile);

            // White to purity
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexTile>(), BiomeConversionID.Purity, TileID.Dirt);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexGrassTile>(), BiomeConversionID.Purity, TileID.Grass);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexMudTile>(), BiomeConversionID.Purity, TileID.Mud);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexJungleGrassTile>(), BiomeConversionID.Purity, TileID.JungleGrass);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSandTile>(), BiomeConversionID.Purity, TileID.Sand);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexStoneTile>(), BiomeConversionID.Purity, TileID.Stone);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexIceTile>(), BiomeConversionID.Purity, TileID.IceBlock);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSnowTile>(), BiomeConversionID.Purity, TileID.SnowBlock);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexLivingWoodTile>(), BiomeConversionID.Purity, TileID.LivingWood);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexStalactiteTile>(), BiomeConversionID.Purity, TileID.Stalactite);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), BiomeConversionID.Purity, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), BiomeConversionID.Purity, DestroyTile);

            // White to corruption
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexTile>(), BiomeConversionID.Corruption, TileID.Dirt);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexGrassTile>(), BiomeConversionID.Corruption, TileID.CorruptGrass);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSandTile>(), BiomeConversionID.Corruption, TileID.Ebonsand);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexStoneTile>(), BiomeConversionID.Corruption, TileID.Ebonstone);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexIceTile>(), BiomeConversionID.Corruption, TileID.CorruptIce);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSnowTile>(), BiomeConversionID.Corruption, TileID.SnowBlock);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), BiomeConversionID.Corruption, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), BiomeConversionID.Corruption, DestroyTile);

            // White to crimson
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexTile>(), BiomeConversionID.Crimson, TileID.Dirt);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexGrassTile>(), BiomeConversionID.Crimson, TileID.CrimsonGrass);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSandTile>(), BiomeConversionID.Crimson, TileID.Crimsand);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexStoneTile>(), BiomeConversionID.Crimson, TileID.Crimstone);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexIceTile>(), BiomeConversionID.Crimson, TileID.FleshIce);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSnowTile>(), BiomeConversionID.Crimson, TileID.SnowBlock);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), BiomeConversionID.Crimson, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalRed>(), BiomeConversionID.Crimson, DestroyTile);

            // White to hallow
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexTile>(), BiomeConversionID.Hallow, TileID.Dirt);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexGrassTile>(), BiomeConversionID.Hallow, TileID.HallowedGrass);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSandTile>(), BiomeConversionID.Hallow, TileID.Pearlsand);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexStoneTile>(), BiomeConversionID.Hallow, TileID.Pearlstone);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexIceTile>(), BiomeConversionID.Hallow, TileID.HallowedIce);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSnowTile>(), BiomeConversionID.Hallow, TileID.SnowBlock);

            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>(), BiomeConversionID.Hallow, DestroyTile);
            TileLoader.RegisterConversion(ModContent.TileType<Content.Tiles.Furniture.PillarWhite>(), BiomeConversionID.Hallow, DestroyTile);
        }
        */
    }
}
