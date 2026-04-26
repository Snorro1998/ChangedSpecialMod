using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public class NPCSwordSwing : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = false;
            Projectile.hostile = true; // NPC attack
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 20; // duration of swing
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void AI()
        {
            NPC owner = Main.npc[(int)Projectile.ai[0]];

            if (!owner.active)
            {
                Projectile.Kill();
                return;
            }

            // Stick to NPC center
            Projectile.Center = owner.Center;

            float progress = 1f - Projectile.timeLeft / 20f;

            // Swing arc (adjust angles as needed)
            float startAngle = -1.5f;
            float endAngle = 1.5f;

            
            if (owner.spriteDirection == -1)
            {
                startAngle *= -1;
                endAngle *= -1;

                //startAngle -= (float)Math.PI;
                //endAngle -= (float)Math.PI;
                Projectile.spriteDirection = -1;
            }
            
            

            float rotation = MathHelper.Lerp(startAngle, endAngle, progress);

            // Flip based on NPC direction
            //rotation *= owner.direction;

            Projectile.rotation = rotation;

            // Offset outward so it looks like a sword
            float distance = 50f * owner.spriteDirection;
            Projectile.Center += rotation.ToRotationVector2() * distance;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }
    }
}
