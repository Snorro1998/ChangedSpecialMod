using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class WateringCanWeaponProjectile : ModProjectile
	{
		public override void SetDefaults() 
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 240;
		}

		public override void AI() 
		{
			Projectile.velocity.Y += 0.35f;
            Projectile.velocity.X *= 0.99f;

            var rotationSpeed = Projectile.velocity.X * 0.05f;
            Projectile.rotation += rotationSpeed;
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
                    Projectile.velocity.Y *= 0.6f;
            }

            var vec = oldVelocity + Projectile.velocity;

            if (Projectile.velocity.Length() > 0.5f && vec.Length() > 0.5f)
            {
                //SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
            else
                Projectile.velocity.Y = 0;

            Projectile.velocity.X *= 0.99f;

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item178, Projectile.Center);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft) 
		{
			
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
		}

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }
	}
}