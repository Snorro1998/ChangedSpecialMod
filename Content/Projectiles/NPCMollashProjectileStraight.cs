using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public class NPCMollashProjectileStraight : ModProjectile
    {
        private const int SegmentCount = 15;
        private const float MaxLength = 225;
        private const int AnimationTime = 30;
        // For keeping track which NPCs it has already hit. Otherwise it will spam hit and instakill them
        private List<int> HitNPCs = new List<int>();

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = AnimationTime;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item152, Projectile.Center);
        }

        public override void AI()
        {
            NPC owner = Main.npc[(int)Projectile.ai[0]];

            if (!owner.active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = owner.Center;

            float totalTime = (float)AnimationTime;
            float progress = 1f - (Projectile.timeLeft / totalTime);

            float baseRotation = Projectile.ai[1];
            float currentRotation = baseRotation;

            float rotationPerSegment = 0f;

            List<Vector2> segmentPositions;
            List<float> segmentRotations;
            float length = MathHelper.Lerp(0f, MaxLength, (float)Math.Sin(progress * Math.PI));

            GetPositionAndRotationLists(
                owner,
                currentRotation,
                rotationPerSegment,
                baseRotation,
                length,
                out segmentPositions,
                out segmentRotations
            );

            for (int i = 0; i < segmentPositions.Count; i++)
            {
                Vector2 pos = segmentPositions[i];

                Rectangle hitbox = new Rectangle(
                    (int)pos.X - 6,
                    (int)pos.Y - 6,
                    12,
                    12
                );

                CheckPlayerHit(hitbox, Projectile.damage);
                CheckNPCHitIncludingPassive(hitbox, Projectile.damage);
            }
        }

        private void CheckPlayerHit(Rectangle hitbox, int damage)
        {
            int ownerIndex = (int)Projectile.ai[0];
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && hitbox.Intersects(player.Hitbox))
                {
                    player.Hurt(
                        PlayerDeathReason.ByNPC(ownerIndex),
                        damage,
                        0
                    );
                }
            }
        }

        private void CheckNPCHitIncludingPassive(Rectangle hitbox, int damage)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || !npc.friendly || npc.immortal || npc.dontTakeDamage || HitNPCs.Contains(npc.whoAmI))
                    continue;

                if (hitbox.Intersects(npc.Hitbox))
                {
                    npc.SimpleStrikeNPC(damage, 0, false);
                    HitNPCs.Add(npc.whoAmI);
                }
            }
        }

        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Projectiles/MollashSpineThin").Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2 - 3, 0);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 2; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());
                Vector2 scale = new Vector2(1, (diff.Length()) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);
                pos += diff;
            }
        }

        private void GetPositionAndRotationLists(NPC owner, float currentRotation, float rotationPerSegment, float baseRotation, float length, out List<Vector2> segmentPositions, out List<float> segmentRotations)
        {
            segmentPositions = new List<Vector2>();
            segmentRotations = new List<float>();

            Vector2 direction = baseRotation.ToRotationVector2();

            for (int i = 0; i < SegmentCount; i++)
            {
                float t = i / (float)SegmentCount;
                Vector2 pos = owner.Center + direction * t * length;

                segmentPositions.Add(pos);
                segmentRotations.Add(baseRotation);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            NPC owner = Main.npc[(int)Projectile.ai[0]];

            float totalTime = (float)AnimationTime;
            float progress = 1f - (Projectile.timeLeft / totalTime);

            float baseRotation = Projectile.ai[1];
            float currentRotation = baseRotation;

            Vector2 direction = currentRotation.ToRotationVector2();

            float length = MathHelper.Lerp(0f, MaxLength, (float)Math.Sin(progress * Math.PI));
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            List<Vector2> segmentPositions = new List<Vector2>();
            List<float> segmentRotations = new List<float>();

            float rotationPerSegment = 0f;

            GetPositionAndRotationLists(
                owner,
                currentRotation,
                rotationPerSegment,
                baseRotation,
                length,
                out segmentPositions,
                out segmentRotations
            );

            //DrawLine(segmentPositions);

            for (int i = 0; i < segmentPositions.Count; i++)
            {
                var color = Lighting.GetColor(segmentPositions[i].ToTileCoordinates());
                var pos = segmentPositions[i];
                var rotation = currentRotation - MathHelper.PiOver2;

                Rectangle frame = new Rectangle(0, 18, 18, 18);
                Vector2 origin = new Vector2(5, 8);

                if (i == SegmentCount - 1)
                {
                    frame.Y = 68;
                    frame.Height = 18;
                }

                Main.spriteBatch.Draw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }

            return false;
        }
    }
}
