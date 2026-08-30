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
        public bool Loop = false;
        public double imageCounter = 0D;
        public double ImageSpeed = 120D;

        public int[] animation = new int[] { 0 };
        public int[] animIdle = new int[] { 0 };
        public int[] animUp = new int[] { 0, 1, 2 };
        public int[] animDown = new int[] { 2, 1, 0 };

        public ref float AIState => ref Projectile.ai[0];
        public ref float AITimer => ref Projectile.ai[1];
        public ref float AITimeIdle => ref Projectile.ai[2];

        public int SpikeDamage = 20;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
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
            FindFrame();
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

        public void FindFrame()
        {
            imageCounter += imageSpeed;
            if (imageCounter >= ImageLength * 60)
            {
                if (Loop)
                {
                    imageCounter %= ImageLength * 60;
                }
                else
                {
                    imageCounter = ImageLength * 60 - 1;
                }
            }

            var arrayIndex = (int)(imageCounter / 60D);
            imageIndex = animation[arrayIndex];
            Projectile.frame = imageIndex;
        }
        /*
        public void FindFrame()
        {
            int maxFrames = ImageLength * 60;
            Projectile.frameCounter += (int)ImageSpeed;

            if (Loop)
            {
                Projectile.frameCounter %= maxFrames;
            }
            else if (Projectile.frameCounter >= maxFrames)
            {
                Projectile.frameCounter = maxFrames - 1;
            }

            int index = (int)(Projectile.frameCounter / 60d);
            Projectile.frame = animation[index];
        }
        */

        private void SwitchState(ActionState newState)
        {
            AIState = (float)newState;
            AITimer = 0;
        }

        private void SwitchAnimation(int[] newAnimation)
        {
            imageCounter = 0;
            animation = newAnimation;
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
                SwitchAnimation(animIdle);
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
                SwitchAnimation(animUp);
                SoundEngine.PlaySound(Sounds.SoundSpike, Projectile.Center);
            }

            if (AITimer > 30)
            {
                SwitchState(ActionState.Down);
            }
        }

        public void StateDown()
        {
            AITimer++;

            if (AITimer == 1)
            {
                Projectile.damage = 0;
                SwitchAnimation(animDown);
            }

            if (AITimer > 30)
            {
                Projectile.active = false;
            }
            //Projectile.active = false;
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