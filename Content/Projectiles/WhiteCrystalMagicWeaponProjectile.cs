using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class WhiteCrystalMagicWeaponProjectile : ModProjectile
	{
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 90;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            var time = Projectile.ai[0];
            var shift = Projectile.ai[1];
            Player owner = Main.player[Projectile.owner];

            var radius = 128;
            var startPosition = new Vector2(owner.position.X, owner.position.Y);

            var xPos = MathF.Cos((time + shift) * 0.05f);
            var yPos = MathF.Sin((time + shift) * 0.05f);

            var unitCirclePos = new Vector2(xPos, yPos);
            var position = unitCirclePos * radius;

            Projectile.rotation = unitCirclePos.ToRotation() + MathHelper.Pi * 0.5f;
            Projectile.scale = (float)Projectile.timeLeft / 90.0f;

            for (int i = 0; i < 4; i++)
            {
                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.WhiteTorch
                    );

                    dust.noGravity = true;
                    dust.velocity *= 0.2f;
                    dust.scale = Main.rand.NextFloat(0.7f, 1.2f) * Projectile.scale;
                }
            }

            Projectile.position = startPosition + position;
            Projectile.ai[0]++;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }
    }
}