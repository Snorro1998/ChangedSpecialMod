using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex;
using ChangedSpecialMod.Utilities;
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
        private static bool NormalConversion(int i, int j, int type, int conversionType)
        {
            WorldGen.ConvertTile(i, j, ModContent.TileType<BlackLatexTile>());
            return false;
        }

        private static bool SandConversion(int i, int j, int type, int conversionType)
        {
            WorldGen.ConvertTile(i, j, ModContent.TileType<BlackLatexSandTile>());
            return false;
        }

        private static bool PurityDirtConversion(int i, int j, int type, int conversionType)
        {
            WorldGen.ConvertTile(i, j, TileID.Dirt);
            return false;
        }

        private static bool PuritySandConversion(int i, int j, int type, int conversionType)
        {
            WorldGen.ConvertTile(i, j, TileID.Sand);
            return false;
        }

        public override void PostSetupContent()
        {
            // Black latex solution
            TileLoader.RegisterConversion(TileID.Dirt, Type, NormalConversion);
            TileLoader.RegisterConversion(TileID.Grass, Type, NormalConversion);
            TileLoader.RegisterConversion(TileID.Sand, Type, SandConversion);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexTile>(), Type, NormalConversion);
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexSandTile>(), Type, SandConversion);
            TileLoader.RegisterConversion(ModContent.TileType<DryDirt>(), Type, NormalConversion);

            // Green solution
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexTile>(), BiomeConversionID.Purity, PurityDirtConversion);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexSandTile>(), BiomeConversionID.Purity, PuritySandConversion);
        }
    }
}
