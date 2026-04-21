using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public class TentatrioProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // This makes the projectile use whip collision detection and allows flasks to be applied to it.
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true; // This prevents the projectile from hitting through solid tiles.
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = DamageClass.SummonMeleeSpeed;
            Projectile.WhipSettings.Segments = 20;
            Projectile.WhipSettings.RangeMultiplier = 2.0f;
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; // Without PiOver2, the rotation would be off by 90 degrees counterclockwise.
            Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * Timer;
            Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;
            Timer++;

            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            if (Timer >= swingTime || owner.itemAnimation <= 0)
            {
                Projectile.Kill();
                return;
            }

            owner.heldProj = Projectile.whoAmI;
            if (Timer == swingTime / 2)
            {
                // Plays a whipcrack sound at the tip of the whip.
                List<Vector2> points = Projectile.WhipPointsForCollision;
                Projectile.FillWhipControlPoints(Projectile, points);
                SoundEngine.PlaySound(SoundID.Item153, points[points.Count - 1]);
            }

            // Spawn Dust along the whip path
            float swingProgress = Timer / swingTime;
            // This code limits dust to only spawn during the the actual swing.
            if (Utils.GetLerpValue(0.1f, 0.7f, swingProgress, clamped: true) * Utils.GetLerpValue(0.9f, 0.7f, swingProgress, clamped: true) > 0.5f && !Main.rand.NextBool(3))
            {
                List<Vector2> points = Projectile.WhipPointsForCollision;
                points.Clear();
                Projectile.FillWhipControlPoints(Projectile, points);
                int pointIndex = Main.rand.Next(points.Count - 10, points.Count);
                Rectangle spawnArea = Utils.CenteredRectangle(points[pointIndex], new Vector2(30f, 30f));
                int dustType = DustID.SnowSpray;
                if (Main.rand.NextBool(3))
                    dustType = DustID.TintableDustLighted;

                // After choosing a randomized dust and a whip segment to spawn from, dust is spawned.
                Dust dust = Dust.NewDustDirect(spawnArea.TopLeft(), spawnArea.Width, spawnArea.Height, dustType, 0f, 0f, 100, Color.White);
                dust.position = points[pointIndex];
                dust.fadeIn = 0.3f;
                Vector2 spinningPoint = points[pointIndex] - points[pointIndex - 1];
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                // This math causes these dust to spawn with a velocity perpendicular to the direction of the whip segments, giving the impression of the dust flying off like sparks.
                dust.velocity += spinningPoint.RotatedBy(owner.direction * ((float)Math.PI / 2f));
                dust.velocity *= 0.5f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.7f); // Multihit penalty. Decrease the damage the more enemies the whip hits.
        }

        // This method draws a line between all points of the whip, in case there's empty space between the sprites.
        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Projectiles/MollashSpine").Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2 - 3, 0);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 2; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates()/*, Color.White*/);
                Vector2 scale = new Vector2(1, (diff.Length()) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);

            DrawLine(list);
            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 pos = list[0];

            for (int i = 0; i < list.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 18, 18);
                Vector2 origin = new Vector2(5, 8);
                float scale = 1;

                if (i == list.Count - 2)
                {
                    frame.Y = 68;
                    frame.Height = 18;

                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Timer / timeToFlyOut;
                    scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else
                {
                    frame.Y = 18;
                    frame.Height = 18;
                }

                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

                pos += diff;
            }
            return false;
        }
    }
}
