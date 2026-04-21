using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public class WhiskerStaffProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            Main.projPet[Type] = true;

            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            //ProjectileID.Sets.Minion[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;//20
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.WhiskerStaffBuff>());
                return;
            }

            if (player.HasBuff(ModContent.BuffType<Buffs.WhiskerStaffBuff>()))
                Projectile.timeLeft = 2;

            ChangedUtils.AI_067_FreakingPirates(Entity);
        }
        /*
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.CatSummonBuff>());
                return;
            }

            if (player.HasBuff(ModContent.BuffType<Buffs.CatSummonBuff>()))
                Projectile.timeLeft = 2;

            // Idle position
            Vector2 idlePosition = player.Center + new Vector2(-40f * player.direction, -40f);
            float distanceToIdle = Vector2.Distance(Projectile.Center, idlePosition);

            if (distanceToIdle > 600f)
            {
                Projectile.Center = idlePosition;
                Projectile.velocity *= 0.1f;
            }

            // Targeting
            NPC target = FindTarget(player);

            if (target != null)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                Projectile.velocity = (Projectile.velocity * 20f + direction * 6f) / 21f;
            }
            else
            {
                // Return to player
                Vector2 direction = idlePosition - Projectile.Center;
                Projectile.velocity = (Projectile.velocity * 20f + direction * 4f) / 21f;
            }
        }

        private NPC FindTarget(Player player)
        {
            NPC closest = null;
            float maxDistance = 700f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy())
                {
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < maxDistance)
                    {
                        maxDistance = distance;
                        closest = npc;
                    }
                }
            }
            return closest;
        }
    */

        /*
        public override void AI()
        {
            ChangedUtils.AI_067_FreakingPirates(Entity);
        }*/

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}
