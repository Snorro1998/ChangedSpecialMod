using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class SpikeProjectile : ModProjectile
	{
        private enum ActionState
        {
            Idle,
            Up,
            Down
        }

        // We are gonna do it just like GameMaker, but we have to do it all ourselves
        public double imageSpeed = 10D;
        public int imageIndex = 0;
        public int ImageLength { get { return animation.Length; } }
        public bool Loop = true;
        public double imageCounter = 0D;

        public int[] animation = new int[] { 0, 1 };
        public int[] animIdle = new int[] { 0 };
        public int[] animUp = new int[] { 1 };

        public ref float AIState => ref Projectile.ai[0];
        public ref float AITimer => ref Projectile.ai[1];
        public ref float AITimeIdle => ref Projectile.ai[2];

        public int SpikeDamage = 20;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 76;
            Projectile.damage = 0;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            AIType = ProjectileID.None;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

        public void DrawProjectileCentered(Projectile proj, Color lightColor, Texture2D texture = null, bool drawCentered = true)
        {
            if (texture is null)
                texture = TextureAssets.Projectile[proj.type].Value;

            int frameHeight = texture.Height / Main.projFrames[proj.type];
            int frameY = frameHeight * proj.frame;
            float scale = proj.scale;
            float rotation = proj.rotation;

            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (proj.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Vector2 startPos = drawCentered ? proj.Center : proj.position;
            Vector2 drawPos = startPos - Main.screenPosition + new Vector2(0f, proj.gfxOffY);

            Main.spriteBatch.Draw(texture, drawPos, rectangle, proj.GetAlpha(lightColor), rotation, origin, scale, spriteEffects, 0f);

        }

        private void SwitchState(ActionState newState)
        {
            AIState = (float)newState;
            AITimer = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void StateIdle()
        {
            AITimer++;

            if (AITimer == 1)
            {
                Projectile.damage = 0;
                Projectile.frame = 0;
            }

            if (AITimer >= AITimeIdle)
            {
                SwitchState(ActionState.Up);
            }
        }

        public void StateUp()
        {
            AITimer++;

            if (AITimer == 1)
            {
                Projectile.damage = SpikeDamage;
                Projectile.frame = 1;
                SoundEngine.PlaySound(Sounds.SoundSpike, Projectile.Center);
            }

            if (AITimer > 30)
            {
                SwitchState(ActionState.Down);
            }
        }

        public void StateDown()
        {
            Projectile.active = false;
        }

        public override void AI()
        {
            switch (AIState)
            {
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.Up:
                    StateUp();
                    break;
                case (float)ActionState.Down:
                    StateDown();
                    break;
            }
        }
    }
}