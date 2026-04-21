using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class BookProjectile : ModProjectile
	{
        private const float moveSpeed = 15;
        private const float rotationSpeed = 0.05f; //radians per tick
        private const int nBlockDetectionRange = 20;
        private float bookRotationSpeed = 0;

        public override void SetStaticDefaults()
        {
			Main.projFrames[Type] = 9;
        }

        public override void SetDefaults() 
        {
			//Projectile.CloneDefaults(ProjectileID.BookOfSkullsSkull);
			//AIType = ProjectileID.BookOfSkullsSkull;

            Projectile.noDropItem = true;
            Projectile.timeLeft = 600;
			Projectile.alpha = 0;
            Projectile.width = 26;
            Projectile.height = 26;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.noDropItem = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var nFrames = Main.projFrames[Type];
            Projectile.localAI[0] += 1;

            if (Projectile.localAI[0] > 2)
            {
                Projectile.localAI[0] = 0;
                Projectile.frame = (Projectile.frame + 1) % nFrames;
            }

			var texture = TextureAssets.Projectile[Projectile.type].Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;
            float scale = Projectile.scale;
            float rotation = Projectile.rotation;
			var position = Projectile.Center;

            /*
            Projectile.spriteDirection = 1;

            // Normalize the rotation to be between 0 and 2 PI
			if (rotation < 0)
			{
				float tmp = rotation * -1;
				tmp = (float)(tmp % (Math.PI * 2));
				rotation = (float)(Math.PI * 2 - tmp);
			}
			else
			{
                rotation = (float)(rotation % (Math.PI * 2));
            }



            // Flip the sprite vertically if facing left, otherwise the paper plane would fly upside down
            if (rotation >= 0.5 * Math.PI && rotation < 1.5 * Math.PI)
			{
				spriteEffects = SpriteEffects.FlipVertically;
			}

            */

            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;
            SpriteEffects spriteEffects = SpriteEffects.None;
            var velocity = Math.Sign(Projectile.velocity.X);

            if (velocity < 0)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Projectile.rotation += (float)(velocity * 2.0f / (Math.PI * 2));

            Main.spriteBatch.Draw(texture, position - Main.screenPosition, rectangle, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, scale, spriteEffects, 0f);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Dust.NewDustDirect(Projectile.Center, 10, 10, DustID.Torch, 0, 0, 0, default(Color), 1.25f).noGravity = true;
            //Dust.NewDustPerfect(Projectile.Center, DustID.Lava, Vector2.Zero, 100, default(Color), 1.25f).noGravity = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            bookRotationSpeed = MathHelper.ToRadians(Main.rand.Next(-10, 10));
        }

        public override void AI()
        {
            Projectile.rotation += bookRotationSpeed;

            //Projectile.rotation = Projectile.velocity.ToRotation();
            var target = Projectile.FindTargetWithinRange(nBlockDetectionRange * 16, false); 
            if (target != null)
            {
                var currentDirection = Projectile.velocity;
                float speed = currentDirection.Length();
                currentDirection.Normalize();

                var targetDirection = target.Center - Projectile.Center;
                targetDirection.Normalize();

                // Max radians to rotate per update
                float maxTurn = MathHelper.ToRadians(4f);

                float angleDifference = currentDirection.ToRotation().AngleTowards(targetDirection.ToRotation(), maxTurn);
                var newDirection = angleDifference.ToRotationVector2();

                // Prevent flying upward too much (minimum Y)
                if (newDirection.Y < 0.1f)
                    newDirection.Y = 0.1f;

                // Re-normalize to preserve direction accuracy
                newDirection.Normalize();

                Projectile.velocity = newDirection * speed;
                /*
                float dirX = target.Center.X - Projectile.Center.X; 
                float dirY = target.Center.Y - Projectile.Center.Y; 
                var targetRotation = (float)Math.Atan2(dirY, dirX);

                float rotationDifference = MathHelper.WrapAngle(targetRotation - Projectile.rotation);
                Projectile.rotation += MathHelper.Clamp(rotationDifference, -rotationSpeed, rotationSpeed);

                // We only update the velocity, because rotation is purely visually and will be updated during the next tick
                Projectile.velocity = new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation)) * moveSpeed; 
                */
            } 
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(BuffID.OnFire, 180);
        }

        public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            base.OnKill(timeLeft);
        }
	}
}