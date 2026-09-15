using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles.Patterns
{
    public class WavePatternProjectile : ModProjectile
    {
        private const float Amplitude = 5f;
        private const float WaveFrequency = 0.05f;
        private const float MoveSpeed = 1.5f;

        public override string Texture => "ChangedSpecialMod/Content/NPCs/SquidDog";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            //Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            //Projectile.extraUpdates = 1;
            //Projectile.alpha = 0;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f && Projectile.localAI[1] == 0f)
            {
                Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX);
                Projectile.localAI[0] = direction.X;
                Projectile.localAI[1] = direction.Y;
                Projectile.ai[1] = 0f;
            }

            Vector2 forward = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
            Vector2 perpendicular = new Vector2(-forward.Y, forward.X);

            Projectile.ai[1] += Projectile.velocity.Length();
            Vector2 forwardPosition = Projectile.position + forward * Projectile.velocity.Length() * MoveSpeed;
            float wave = MathF.Sin(Projectile.ai[0] * WaveFrequency);

            Vector2 offset = perpendicular * wave * Amplitude;
            Projectile.position = forwardPosition + offset;
            Projectile.ai[0]++;
            Projectile.rotation = Projectile.velocity.ToRotation();

            for (int i = 0; i < 4; i++)
            {
                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.GreenTorch
                    );

                    dust.noGravity = true;
                    dust.velocity *= 0.15f;
                    dust.scale = Main.rand.NextFloat(0.7f, 1.2f);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(220, 160, 255, 230);
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