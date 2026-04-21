using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles.SharkBossFight
{
    public abstract class BaseFishProjectile : ModProjectile
    {
        protected float GravityStrength = 0.4f;
        protected float MaxVelocity = 20f;

        // Force derived classes to define these
        public abstract int ProjectileWidth { get; }
        public abstract int ProjectileHeight { get; }
        public abstract int ProjectileFrames {  get; }
        public abstract string ProjectileTexture { get; }
        public abstract bool SpinningRotation { get; }

        // 0 falldown from tornado
        // 1 fly in straight line
        // 2 circle
        public ref float AIAttackPattern => ref Projectile.ai[2];
        public override string Texture => ProjectileTexture;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = ProjectileFrames;
        }

        public override void SetDefaults()
        {
            Projectile.width = ProjectileWidth;
            Projectile.height = ProjectileHeight;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }

        private void Animate()
        {
            var nFrames = Main.projFrames[Type];
            if (nFrames < 2)
                return;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % nFrames;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

        public override void AI()
        {
            Animate();
            if (AIAttackPattern == 0)
            {
                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
                DoGravity();
                if (SpinningRotation)
                    Projectile.rotation += 0.1f * Projectile.spriteDirection * (float)Math.PI;
                else
                    Projectile.rotation = Projectile.spriteDirection * Math.Clamp(Projectile.velocity.Y / 10, -1, 1) * 0.25f * (float)Math.PI;
            }
            else if (AIAttackPattern == 1)
            {
                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            }
            else if (AIAttackPattern == 2)
            {
                Projectile.rotation += 0.008f * (float)Math.PI;
                Projectile.velocity = new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation)) * 10;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        protected void DoGravity()
        {
            float speed = Projectile.velocity.Y;
            if (speed < MaxVelocity)
                speed = MathF.Min(speed + GravityStrength, MaxVelocity);

            Projectile.velocity.Y = speed;
        }
    }
}
