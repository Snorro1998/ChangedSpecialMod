using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class FartGunProjectile : ModProjectile
	{
        float extraRotation = 0;

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
        }

        public override void OnSpawn(IEntitySource source)
        {
            extraRotation = Main.rand.NextFloat((float)Math.PI * 2);
            Projectile.position += Projectile.velocity.SafeNormalize(Vector2.Zero) * 32;
        }

        public override void AI()
        {
            if (Projectile.alpha >= 255)
                Projectile.Kill();

            Projectile.rotation = Projectile.velocity.SafeNormalize(Vector2.Zero).ToRotation() + extraRotation;
            Projectile.scale += 0.02f;

            Projectile.alpha += 2;
            if (Projectile.alpha > 255)
                Projectile.alpha = 255;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 3 * 60);
            target.AddBuff(BuffID.Stinky, 3 * 60);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}