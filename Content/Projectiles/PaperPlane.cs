using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class PaperPlane : ModProjectile
	{
		public override void SetDefaults() 
		{
			Projectile.CloneDefaults(ProjectileID.PaperAirplaneB);
			Projectile.noDropItem = true;
            Projectile.timeLeft = 600;
        }
		
		
        public override bool PreDraw(ref Color lightColor)
        {
			var texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;
            float scale = Projectile.scale;
            float rotation = Projectile.rotation;
			var position = Projectile.Center;

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

            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
			Vector2 origin = rectangle.Size() / 2f;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (rotation >= 0.5 * Math.PI && rotation < 1.5 * Math.PI)
			{
				spriteEffects = SpriteEffects.FlipVertically;
			}

			Main.spriteBatch.Draw(texture, position - Main.screenPosition, rectangle, Projectile.GetAlpha(lightColor), rotation, origin, scale, spriteEffects, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Snow, 0, 0, 1, default(Color), Main.rand.NextFloat(1.0f, 2.0f));
                dust.noGravity = true;
            }
        }
	}
}