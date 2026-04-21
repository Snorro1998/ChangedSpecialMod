using ChangedSpecialMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class BasketballProjectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			Main.projFrames[Type] = 1;
        }

        public override void SetDefaults() 
        {
            Projectile.CloneDefaults(ProjectileID.BeachBall);
            Projectile.width = 24;
            Projectile.height = 24;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
                if (Projectile.velocity.Y < 0)
                    Projectile.velocity.Y *= 0.8f;
            }

            var vec = oldVelocity + Projectile.velocity;

            if (Projectile.velocity.Length() > 0.5f && vec.Length() > 0.5f)
            {
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
            else
            {
                Projectile.velocity.Y = 0;
            }

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Center, ModContent.ItemType<BasketballWeapon>(), 1);
        }
    }
}