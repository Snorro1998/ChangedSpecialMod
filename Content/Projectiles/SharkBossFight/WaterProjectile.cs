using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace ChangedSpecialMod.Content.Projectiles.SharkBossFight
{
	public class WaterProjectile : ModProjectile
	{
        public override string Texture => "ChangedSpecialMod/Content/Projectiles/SharkBossFight/WaterProjectile";
        public ref float AITimer => ref Projectile.ai[1];
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.scale = 0;
        }

        public override void AI()
        {
            AITimer++;
            Projectile.scale = Math.Min(1, Projectile.scale + 0.02f);

            var sharkBoss = Main.npc.FirstOrDefault(x => x.active && x.type == ModContent.NPCType<TigerSharkBoss>());
            if (sharkBoss != null)
            {
                Projectile.position = sharkBoss.position - new Vector2(0, 4 * 16);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            //DrawProjectileCentered(Projectile, lightColor);
            return false;
        }
    }
}