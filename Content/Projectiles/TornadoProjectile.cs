using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Collections.Specialized.BitVector32;

namespace ChangedSpecialMod.Content.Projectiles
{
	public class TornadoProjectile : ModProjectile
	{
        // In nearly all other projectiles and NPCs this is the state for the state machine, but we don't need that here
        public ref float Interval => ref Projectile.ai[0];
        public ref float AITimer => ref Projectile.ai[1];
        public ref float NSections => ref Projectile.ai[2];
        private int ProjectileDamage = 15;
        private int sectionHeight = 32;
        private float timeLeftMultiplier = 1.0f;

        public override void SetDefaults() 
		{
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
			Projectile.hostile = false;
            Projectile.noDropItem = true;
            Projectile.timeLeft = 600;
        }

        public override void OnSpawn(IEntitySource source)
        {
            AITimer = 0;
        }

        public override void AI()
        {
            AITimer++;

            if (AITimer == 1 || AITimer % Interval == 0)
            {
                if (NSections == -25)
                {
                    Interval = 5;
                    timeLeftMultiplier = 0.25f;
                }
                var entitySource = Projectile.GetSource_FromAI();

                int currentSection = (int)AITimer / (int)Interval;
                int xPos = (int)Projectile.Center.X;
                var heightDifference = -currentSection * sectionHeight;
                if (NSections < 0)
                    heightDifference *= -1;
                int yPos = (int)Projectile.Center.Y + heightDifference;
                var projectileId = Projectile.NewProjectile(entitySource, new Vector2(xPos, yPos), Vector2.Zero, ProjectileID.Sharknado, 0, 0, -1, 0f, 0f);
                if (projectileId != -1)
                {
                    var projectile = Main.projectile[projectileId];
                    projectile.friendly = false;
                    projectile.damage = ProjectileDamage;
                    projectile.timeLeft = (int)(projectile.timeLeft * timeLeftMultiplier);
                }
            }

            if (AITimer >= Math.Abs(NSections) * Interval)
            {
                Projectile.active = false;
            }
        }
	}
}