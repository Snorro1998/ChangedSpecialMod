using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public class FakeCloudProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            var npcIndex = NPC.FindFirstNPC(ModContent.NPCType<Experiment009>());
            if (npcIndex == -1)
            {
                Projectile.active = false;
                return;
            }

            Projectile.timeLeft = 3600;
            var npc = Main.npc[npcIndex];
            Projectile.Center = npc.Center + Vector2.One * 16;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }

    public class ElectricOrbProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            var pos = Projectile.Center;
            var velocity = Vector2.Zero;
            var projType = ProjectileID.Electrosphere;

            var i = Projectile.NewProjectile(
                Projectile.GetSource_FromAI(),
                pos,
                velocity,
                projType,
                25,
                0f,
                Main.myPlayer
            );
        }
    }

    public class LightningBoltProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vortex Lightning Clone");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.VortexLightning);

            // Important: define your AI style to match
            AIType = ProjectileID.VortexLightning;
        }
    }
    /*
    public class LightningBoltProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.VortexLightning);
            Projectile.aiStyle = ProjAIStyleID.Vortex;
        }
    }
    */

    /*
    public class LightningBoltProjectile : ModProjectile
    {
        public ref float AIStartDirection => ref Projectile.ai[0];
        public ref float AITimer => ref Projectile.ai[1];
        public ref float AISplit => ref Projectile.ai[2];

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lightning Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // Add light
            Lighting.AddLight(Projectile.Center, 0.3f, 0.45f, 0.9f);

            // Lightning jitter effect
            //Projectile.velocity.X += Main.rand.NextFloat(-0.5f, 0.5f);
            //Projectile.velocity.Y += Main.rand.NextFloat(-0.5f, 0.5f);

            // Slight acceleration
            //Projectile.velocity *= 1.02f;

            var dif = 1f;
            var direction = AIStartDirection + Main.rand.NextFloat(-dif, dif);
            Projectile.velocity = direction.ToRotationVector2() * 10;

            // Rotation matches velocity
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Dust effect
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (AISplit != 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 newVelocity = Main.rand.NextVector2Circular(4f, 4f);
                    Projectile.NewProjectile(
                        Projectile.GetSource_Death(),
                        Projectile.Center,
                        newVelocity,
                        ModContent.ProjectileType<LightningBoltProjectile>(),
                        Projectile.damage / 2,
                        0f,
                        Main.myPlayer,
                        newVelocity.SafeNormalize(Vector2.UnitY).ToRotation(),
                        0,
                        1
                    );
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Electrified, 120);
        }
    }
    */
}