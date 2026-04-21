using ChangedSpecialMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class BottleProjectile : ModProjectile
	{
		public static float RotationSpeed = 10;

		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 600;
		}

		public override void AI() 
		{
			Projectile.velocity.Y += Projectile.ai[0];
			Projectile.rotation += MathHelper.ToRadians(RotationSpeed);
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
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
					Terraria.ID.DustID.Glass, 
					Projectile.oldVelocity.X * 0.5f, 
					Projectile.oldVelocity.Y * 0.5f,
					0,
					Color.White);
			}
			SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Projectile.ai[0] += 0.1f;
			Projectile.velocity *= 0.75f;
		}
	}
}