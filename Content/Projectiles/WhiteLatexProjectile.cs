using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class WhiteLatexProjectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

		public override void SetDefaults() 
		{
			Projectile.CloneDefaults(ProjectileID.DeerclopsRangedProjectile);
            Projectile.timeLeft = 600;
            Projectile.width = 40;
            Projectile.height = 40;
			Projectile.damage = 1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ArmorPenetration = 0;
        }

        public override void AI()
        {
            Animate();
        }

        private void Animate()
        {
            var nFrames = Main.projFrames[Type];
            var i = (int)((600 - Projectile.timeLeft) / 8) % nFrames;
            Projectile.frame = i;
        }
	}
}