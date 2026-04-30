using ChangedSpecialMod.Content.Items.Placeable.Latex;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Tiles.Latex;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles.Latex
{
    public class BlackLatexSandBallFallingProjectile : BaseSandBallProjectile
    {
        public override string Texture => "ChangedSpecialMod/Content/Projectiles/Latex/BlackLatexSandBallProjectile";
        public override GooType GooType => GooType.Black;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<BlackLatexSandTile>(), ModContent.ItemType<BlackLatexSand>());
        }

        public override void SetDefaults()
        {
            // The falling projectile when compared to the sandgun projectile is hostile.
            Projectile.CloneDefaults(ProjectileID.EbonsandBallFalling);
        }
    }

    public class BlackLatexSandBallGunProjectile : BaseSandBallProjectile
    {
        public override string Texture => "ChangedSpecialMod/Content/Projectiles/Latex/BlackLatexSandBallProjectile";
        public override GooType GooType => GooType.Black;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<BlackLatexSandTile>());
        }

        public override void SetDefaults()
        {
            // The sandgun projectile when compared to the falling projectile has a ranged damage type, isn't hostile, and has extraupdates = 1.
            // Note that EbonsandBallGun has infinite penetration, unlike SandBallGun
            Projectile.CloneDefaults(ProjectileID.EbonsandBallGun);
            AIType = ProjectileID.EbonsandBallGun; // This is needed for some logic in the ProjAIStyleID.FallingTile code.
        }
    }
}
