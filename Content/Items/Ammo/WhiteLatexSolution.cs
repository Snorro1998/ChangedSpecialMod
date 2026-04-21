using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Utilities;
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
                int dustType = DustID.SnowSpray;//ModContent.DustType<ExampleSolutionDust>();

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
        private static bool NormalConversion(int i, int j, int type, int conversionType)
        {
            WorldGen.ConvertTile(i, j, ModContent.TileType<WhiteLatexTile>());
            return false;
        }

        private static bool ConvertCrystalRed(int i, int j, int type, int conversionType)
        {
            var xPos = i;
            var yPos = j;
            var sourceTile = ModContent.TileType<Tiles.Furniture.CrystalRed>();
            var targetTile = ModContent.TileType<Tiles.Furniture.CrystalWhite>();
            var tileWidth = 3;
            var tileHeight = 3;
            for (var k = 0; k < tileWidth; k++)
            {
                var tmpXPos = i - k;
                if (tmpXPos < 0)
                {
                    xPos = 0;
                    break;
                }
                var tmpTile = Main.tile[tmpXPos, j];
                if (!tmpTile.HasTile || tmpTile.TileType != (ushort)sourceTile)
                {
                    xPos = tmpXPos + 1;
                    break;
                }
            }

            for (var k = 0; k < tileHeight; k++)
            {
                var tmpYPos = j - k;
                if (tmpYPos < 0)
                {
                    yPos = 0;
                    break;
                }
                var tmpTile = Main.tile[i, tmpYPos];
                if (!tmpTile.HasTile || tmpTile.TileType != (ushort)sourceTile)
                {
                    yPos = tmpYPos + 1;
                    break;
                }
            }

            for (int k = xPos; k < xPos + tileWidth; k++)
            {
                for (int l = yPos; l < yPos + tileHeight; l++)
                {
                    var tile = Main.tile[k, l];
                    tile.TileType = (ushort)targetTile;
                }
            }

            return false;
        }

        private static bool ConvertCrystalGreen(int i, int j, int type, int conversionType)
        {
            var xPos = i;
            var yPos = j;
            var sourceTile = ModContent.TileType<Tiles.Furniture.CrystalGreen>();
            var targetTile = ModContent.TileType<Tiles.Furniture.CrystalWhite>();
            var tileWidth = 3;
            var tileHeight = 3;
            for (var k = 0; k < tileWidth; k++)
            {
                var tmpXPos = i - k;
                if (tmpXPos < 0)
                {
                    xPos = 0;
                    break;
                }
                var tmpTile = Main.tile[tmpXPos, j];
                if (!tmpTile.HasTile || tmpTile.TileType != (ushort)sourceTile)
                {
                    xPos = tmpXPos + 1;
                    break;
                }
            }

            for (var k = 0; k < tileHeight; k++)
            {
                var tmpYPos = j - k;
                if (tmpYPos < 0)
                {
                    yPos = 0;
                    break;
                }
                var tmpTile = Main.tile[i, tmpYPos];
                if (!tmpTile.HasTile || tmpTile.TileType != (ushort)sourceTile)
                {
                    yPos = tmpYPos + 1;
                    break;
                }
            }

            for (int k = xPos; k < xPos + tileWidth; k++)
            {
                for (int l = yPos; l < yPos + tileHeight; l++)
                {
                    var tile = Main.tile[k, l];
                    tile.TileType = (ushort)targetTile;
                }
            }

            return false;
        }

        private static bool PurityConversion(int i, int j, int type, int conversionType)
        {
            WorldGen.ConvertTile(i, j, TileID.Dirt);
            return false;
        }

        public override void PostSetupContent()
        {
            TileLoader.RegisterConversion(TileID.Dirt, Type, NormalConversion);
            TileLoader.RegisterConversion(TileID.Grass, Type, NormalConversion);
            TileLoader.RegisterConversion(ModContent.TileType<BlackLatexTile>(), Type, NormalConversion);
            TileLoader.RegisterConversion(ModContent.TileType<DryDirt>(), Type, NormalConversion);
            //TileLoader.RegisterConversion(ModContent.TileType<Tiles.Furniture.CrystalGreen>(), Type, ConvertCrystalGreen);
            //TileLoader.RegisterConversion(ModContent.TileType<Tiles.Furniture.CrystalRed>(), Type, ConvertCrystalRed);

            // Green solution
            TileLoader.RegisterConversion(ModContent.TileType<WhiteLatexTile>(), BiomeConversionID.Purity, PurityConversion);
        }
    }
}
