using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class WateringCanProjectile : ModProjectile
	{
		public static float RotationSpeed = 10;

		public override void SetDefaults() 
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 600;
		}

		public override void AI() 
		{
			Projectile.velocity.Y += 0.3f;// Projectile.ai[0];
			Projectile.rotation += Math.Sign(Projectile.velocity.X) * MathHelper.ToRadians(RotationSpeed);
		}

		public override bool OnTileCollide(Vector2 oldVelocity) 
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			else {
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				Projectile.velocity *= 0.75f;
				SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			}
			return false;
		}

		public override void OnKill(int timeLeft) {
			
			for (int k = 0; k < 40; k++) 
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, 
					Projectile.width, 
					Projectile.height, 
					Terraria.ID.DustID.Pot, 
					Projectile.oldVelocity.X * 0.5f, 
					Projectile.oldVelocity.Y * 0.5f,
					0,
					Color.White);
			}
			//shatter
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) 
		{
            SoundEngine.PlaySound(SoundID.Item178, Projectile.Center);
            //Projectile.ai[0] += 0.1f;
            Projectile.velocity *= 0.75f;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

    }
}