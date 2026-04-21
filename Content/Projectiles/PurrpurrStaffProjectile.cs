using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public class PurrpurrStaffProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
            Main.projPet[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Summon;
            // Enemies have grown bigger and stronger by the time you can create this weapon
            // So make the cat spawned by this weapon bigger as well
            Projectile.scale = 1.3f;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }

        private void SayRandomText()
        {
            if (Main.rand.Next(1000) == 0)
            {
                var txt = ChangedUtils.Choose("Purrr", "Meow", "Mrrrr");
                Color color = new Color(Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255));
                color = new Color(255, 0, 255);
                CombatText.NewText(new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height), color, txt, true);
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.PurrpurrStaffBuff>());
                return;
            }

            if (player.HasBuff(ModContent.BuffType<Buffs.PurrpurrStaffBuff>()))
                Projectile.timeLeft = 2;

            ChangedUtils.AI_067_FreakingPirates(Entity);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}
