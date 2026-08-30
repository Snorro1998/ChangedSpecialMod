using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class RedCrystalMagicWeaponProjectile : ModProjectile
	{
        private const int OutwardTime = 45;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 1200;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 40;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();

                if (Projectile.localAI[0] >= OutwardTime)
                {
                    Projectile.ai[0] = 1;
                    Projectile.localAI[0] = 0f;

                    if (Main.myPlayer == Projectile.owner)
                    {
                        SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
                    }
                }
            }
            else
            {
                Vector2 direction = Projectile.DirectionTo(owner.Center);

                float distance = Vector2.Distance(
                    Projectile.Center,
                    owner.Center
                );

                float speed = MathHelper.Lerp(8f, 22f, 1f - MathHelper.Clamp(distance / 500f, 0f, 1f));

                Projectile.velocity = Vector2.Lerp(
                    Projectile.velocity,
                    direction * speed,
                    0.08f
                );

                Projectile.rotation = Projectile.velocity.ToRotation();

                if (distance < 24f)
                {
                    Projectile.Kill();
                }
            }

            Projectile.localAI[0]++;

            for (int i = 0; i < 4; i++)
            {
                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.RedTorch
                    );

                    dust.noGravity = true;
                    dust.velocity *= 0.2f;
                    dust.scale = Main.rand.NextFloat(0.7f, 1.2f);
                }

                if (Main.rand.NextBool(5))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.GemAmethyst
                    );

                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                    dust.scale = 0.6f;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;

            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
                Projectile.localAI[0] = 0f;

                SoundEngine.PlaySound(
                    SoundID.Dig,
                    Projectile.Center
                );
            }

            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(220, 150, 255, 220);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.8f);
        }
    }
}