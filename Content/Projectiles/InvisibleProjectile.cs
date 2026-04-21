using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    // DirectStrike from Calamity
    public class InvisibleProjectile : ModProjectile
    {
        public override string Texture => "ChangedSpecialMod/Content/NPCs/Drunk/PuroWormTail";

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            //Projectile.timeLeft = 2;

            // Hacky thing to prevent it from being spammed and killing the npc instantly
            Projectile.penetrate = 30;
            Projectile.timeLeft = 30;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] < 0f || Projectile.ai[0] > 199f || Projectile.ai[0] == target.whoAmI)
                return null;
            return (bool?)false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = 0;
        }
    }
}
