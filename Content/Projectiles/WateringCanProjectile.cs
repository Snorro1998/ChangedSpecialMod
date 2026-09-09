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
			Projectile.penetrate = 2;
			Projectile.timeLeft = 180;
		}

		public override void AI() 
		{
            Projectile.velocity.Y += 0.30f;
            Projectile.velocity.X *= 0.99f;

            var rotationSpeed = Projectile.velocity.X * 0.05f;
            Projectile.rotation += rotationSpeed;
            //Projectile.velocity.Y += 0.3f;// Projectile.ai[0];
            //Projectile.rotation += Math.Sign(Projectile.velocity.X) * MathHelper.ToRadians(RotationSpeed);
        }

		public override bool OnTileCollide(Vector2 oldVelocity) 
		{
            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                Projectile.velocity.X = -oldVelocity.X;

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
                if (Projectile.velocity.Y < 0)
                    Projectile.velocity.Y *= 0.75f;
            }

			/*
            var vec = oldVelocity + Projectile.velocity;

            if (Projectile.velocity.Length() > 0.5f && vec.Length() > 0.5f)
            {
                //SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
            else
                Projectile.velocity.Y = 0;
			*/

            //Projectile.velocity.X *= 0.99f;

            return false;
            /*
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
			*/
        }

		public override void OnKill(int timeLeft) {
			
			for (int k = 0; k < 10; k++) 
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, 
					Projectile.width, 
					Projectile.height, 
					Terraria.ID.DustID.Iron, 
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
            base.OnHitNPC(target, hit, damageDone);
        }
        /*
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) 
		{
            SoundEngine.PlaySound(SoundID.Item178, Projectile.Center);
            //Projectile.ai[0] += 0.1f;
            Projectile.velocity *= 0.75f;
		}
		*/

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

    }
}