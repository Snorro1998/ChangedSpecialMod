using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChangedSpecialMod.Content.Projectiles.Latex;

namespace ChangedSpecialMod.Content.Items.Placeable.Latex
{
    public class BlackLatexSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SandgunAmmoProjectileData[Type] = new(ModContent.ProjectileType<BlackLatexSandBallGunProjectile>(), 10);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Latex.BlackLatexSandTile>());
            Item.width = 12;
            Item.height = 12;
            Item.ammo = AmmoID.Sand;
            Item.notAmmo = true;
        }
    }
}
