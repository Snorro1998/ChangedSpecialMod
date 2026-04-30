using ChangedSpecialMod.Content.Items.Placeable.Latex;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Tiles.Latex;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles.Latex
{
    public class WhiteLatexSandBallFallingProjectile : BaseSandBallProjectile
    {
        public override string Texture => "ChangedSpecialMod/Content/Projectiles/Latex/WhiteLatexSandBallProjectile";
        public override GooType GooType => GooType.White;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<WhiteLatexSandTile>(), ModContent.ItemType<WhiteLatexSand>());
        }

        public override void SetDefaults()
        {
            // The falling projectile when compared to the sandgun projectile is hostile.
            Projectile.CloneDefaults(ProjectileID.EbonsandBallFalling);
        }
    }

    public class WhiteLatexSandBallGunProjectile : BaseSandBallProjectile
    {
        public override string Texture => "ChangedSpecialMod/Content/Projectiles/Latex/WhiteLatexSandBallProjectile";
        public override GooType GooType => GooType.White;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<WhiteLatexSandTile>());
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
