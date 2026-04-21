using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public abstract class GasTankProjectile : ModProjectile
	{
        public ref float Velocity => ref Projectile.ai[2];

        public override void SetDefaults() 
        {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 600;
		}

        public override void OnSpawn(IEntitySource source)
        {
			SoundEngine.PlaySound(Sounds.SoundBalloon, Projectile.Center);
            base.OnSpawn(source);
        }

		public override void AI() 
        {
			Velocity += 0.07f;
			var amplitude = Velocity * 3;
            Projectile.rotation += MathHelper.ToRadians(Utils.SelectRandom(Main.rand, -amplitude, amplitude));
			
            var cos = (float)Math.Cos(Projectile.rotation - 0.5f * Math.PI);
			var sin = (float)Math.Sin(Projectile.rotation - 0.5f * Math.PI);
			Projectile.velocity = new Vector2(cos, sin) * Velocity;
			
            var particlePos = Projectile.position;
            particlePos.X -= 24 * cos;
            particlePos.Y -= 24 * sin;

			// Terraria names particles gores
			var particle = Utils.SelectRandom(Main.rand, GoreID.FartCloud1, GoreID.FartCloud2, GoreID.FartCloud3);
            Gore.NewGore(Projectile.GetSource_FromAI(), particlePos, Projectile.velocity * -1, particle, Main.rand.NextFloat(0.1f, 0.9f));

            if (Projectile.velocity.Y < -10)
			{
				Projectile.Kill();
			}
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                var particle = Utils.SelectRandom(Main.rand, GoreID.FartCloud1, GoreID.FartCloud2, GoreID.FartCloud3);
                Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.position, Vector2.Zero, particle, Main.rand.NextFloat(1f, 2f));
            }

            // Whoopie cushion sound
            SoundEngine.PlaySound(SoundID.Item16, Projectile.Center);
            // Queen Slime death sound
            SoundEngine.PlaySound(SoundID.NPCDeath64, Projectile.Center);
        }
    }
}