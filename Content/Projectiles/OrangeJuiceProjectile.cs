using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public class OrangeJuiceProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic; // Makes the projectile deal ranged damage. You can set in to DamageClass.Throwing, but that is not used by any vanilla items
            Projectile.penetrate = 2; // How many monsters the projectile can penetrate.
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.velocity.X < 0)
                Projectile.velocity.X = -5;
            else
                Projectile.velocity.X = 5;

            Projectile.velocity.Y = 0;
        }

        public override void AI()
        {
            bool hitWater = Collision.WetCollision(
                Projectile.position,
                Projectile.width,
                Projectile.height
            );

            bool topInsideTile = Collision.SolidCollision(
                Projectile.position + new Vector2(0, -33),
                Projectile.width,
                Projectile.height
            );

            bool insideTile = Collision.SolidCollision(
                Projectile.position,
                Projectile.width,
                Projectile.height
            );

            var checkDownDist = (int)Math.Abs(Projectile.velocity.Y);

            bool onGround = Collision.SolidCollision(
                Projectile.position + new Vector2(0, checkDownDist),
                Projectile.width,
                Projectile.height
            );

            if (topInsideTile && insideTile || hitWater)
            {
                Projectile.Kill();
            }
            if (insideTile)
            {
                Projectile.velocity.Y += -2f;// -0.2f;
            }
            else if (onGround)
            {
                Projectile.velocity.Y = 0f;
            }
            else
            {
                Projectile.velocity.Y += 1;// Math.Min(Projectile.velocity.Y + 0.2f, 3f);
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<OrangeJuiceDust>()
                );

                dust.noGravity = true;
                var xSpeed = Projectile.velocity.X + Math.Sign(Projectile.velocity.X) * 4;
                dust.velocity = new Vector2(xSpeed, 0);
                dust.scale = Main.rand.NextFloat(0.5f, 1.5f);
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
            }

            if (Projectile.velocity.X != 0)
            {
                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Splash, Projectile.Center);
            for (int i = 0; i < 40; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<OrangeJuiceDust>()
                );

                dust.scale = Main.rand.NextFloat(1f, 2f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }
    }
}
