using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Projectiles.Patterns;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public abstract class WormProjectile : ModProjectile
    {
        /*  ai[] usage:
            *
            *  ai[0] = "follower" segment, the segment that's following this segment
            *  ai[1] = "following" segment, the segment that this segment is following
            *
            *  localAI[0] = used when syncing changes to collision detection
            *  localAI[1] = checking if Init() was called
            */

        public abstract WormSegmentType SegmentType { get; }

        public abstract int BodyType { get; }

        public abstract int TailType { get; }

        public int MinSegmentLength { get; set; }

        public int MaxSegmentLength { get; set; }

        public bool CanFly { get; set; }

        public float MoveSpeed { get; set; }

        public float Acceleration { get; set; }

        public int ReverseAfterTime { get; set; }

        public float MaxEntendDistance { get; set; }

        public Vector2? ForcedTargetPosition { get; set; }
        
        public virtual int MaxDistanceForUsingTileCollision => 1000;

        public bool dontDraw = false;

        public List<WormProjectile> allSegments = new List<WormProjectile>();

        public Vector2 startPosition = new Vector2(0, 0);

        public void PlayDigSounds(float length)
        {
            if (Projectile.soundDelay == 0)
            {
                // Play sounds quicker the closer the NPC is to the target location
                float num1 = length / 40f;

                if (num1 < 10)
                    num1 = 10f;

                if (num1 > 20)
                    num1 = 20f;

                Projectile.soundDelay = (int)num1;
                SoundEngine.PlaySound(SoundID.WormDig, Projectile.Center);
            }
        }

        public void Leader_Movement_SetRotation(bool collision)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (collision)
            {
                if (Projectile.localAI[0] != 1)
                    Projectile.netUpdate = true;

                Projectile.localAI[0] = 1f;
            }
            else
            {
                if (Projectile.localAI[0] != 0)
                    Projectile.netUpdate = true;

                Projectile.localAI[0] = 0f;
            }

            if (((Projectile.velocity.X > 0 && Projectile.oldVelocity.X < 0) || (Projectile.velocity.X < 0 && Projectile.oldVelocity.X > 0) || (Projectile.velocity.Y > 0 && Projectile.oldVelocity.Y < 0) || (Projectile.velocity.Y < 0 && Projectile.oldVelocity.Y > 0)))
                Projectile.netUpdate = true;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            var color = Lighting.GetColor((int)(Projectile.position.X / 16), (int)(Projectile.position.Y / 16));

            if (!dontDraw)
                ChangedUtils.DrawProjectileCentered(Projectile, color);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public sealed override bool PreAI()
        {
            // hacky thing to set the mode
            if (Projectile.ai[2] < 0)
            {
                Projectile.localAI[2] = Projectile.ai[2];
                Projectile.ai[2] = 0;
            }

            CanFly = true;
            //MoveSpeed = 5f;
            //Acceleration = 1f;

            bool collision = OverlappingAnyBlackTile();
            dontDraw = collision;

            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[1] = 1f;
                Init();
            }

            var reverseMovement = Projectile.ai[2] > ReverseAfterTime;
            var leadingType = reverseMovement ? WormSegmentType.Tail : WormSegmentType.Head;

            if (SegmentType == leadingType)
            {
                HeadAI();

                for (int i = 1; i < allSegments.Count; i++)
                {
                    allSegments[i].BodyTailAI();
                }
            }

            if (Projectile.ai[2] == 60)
            {
                startPosition = Projectile.Center;
            }
            else if (Projectile.ai[2] > 120 && Projectile.ai[2] < ReverseAfterTime)
            {
                if (SegmentType == WormSegmentType.Tail && Vector2.Distance(Projectile.Center, startPosition) > MaxEntendDistance)
                {
                    foreach (var segment in allSegments)
                    {
                        segment.Projectile.ai[2] = ReverseAfterTime;
                    }
                }
            }



            if (Projectile.ai[2] == ReverseAfterTime)
            {
                Projectile.velocity = Vector2.Zero;
                ForcedTargetPosition = new Vector2(Projectile.position.X, Main.maxTilesY * 16);
            }

            Projectile.ai[2]++;

            return true;
        }

        public void HeadAI_CheckTargetDistance(ref bool collision)
        {
            if (!collision)
            {
                Rectangle hitbox = Projectile.Hitbox;

                int maxDistance = MaxDistanceForUsingTileCollision;

                bool tooFar = true;

                foreach (var player in Main.ActivePlayers)
                {
                    Rectangle areaCheck;

                    if (ForcedTargetPosition is Vector2 target)
                        areaCheck = new Rectangle((int)target.X - maxDistance, (int)target.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                    else if (!player.dead && !player.ghost)
                        areaCheck = new Rectangle((int)player.position.X - maxDistance, (int)player.position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                    else
                        continue;

                    if (hitbox.Intersects(areaCheck))
                    {
                        tooFar = false;
                        break;
                    }
                }

                if (tooFar)
                    collision = true;
            }
        }

        public void HeadAI()
        {
            if (SegmentType == WormSegmentType.Head)
                HeadAI_SpawnSegments();

            bool collision = HeadAI_CheckCollisionForDustSpawns();
            HeadAI_CheckTargetDistance(ref collision);
            HeadAI_Movement(collision);
        }

        public void HeadAI_SpawnSegments()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                bool hasFollower = Projectile.ai[0] > 0;
                if (!hasFollower)
                {
                    allSegments.Add(this);

                    int latestNPC = Projectile.whoAmI;
                    int randomWormLength = Main.rand.Next(MinSegmentLength, MaxSegmentLength + 1);

                    int distance = randomWormLength - 2;
                    IEntitySource source = Projectile.GetSource_FromAI();

                    while (distance > 0)
                    {
                        latestNPC = SpawnSegment(source, BodyType, latestNPC);
                        distance--;
                    }

                    var tailIndex = SpawnSegment(source, TailType, latestNPC);
                    var tailProjectile = Main.projectile[tailIndex].ModProjectile as WormProjectile;

                    for (int i = allSegments.Count - 1; i >= 0; i--)
                    {
                        tailProjectile.allSegments.Add(allSegments[i]);
                    }

                    Projectile.netUpdate = true;
                }
            }
        }

        protected int SpawnSegment(IEntitySource source, int type, int latestNPC)
        {
            //(int)NPC.Center.Y
            // Decoil the worm
            var yPos = (int)(Main.projectile[latestNPC].Center.Y + 16 * 10);

            int oldLatest = latestNPC;
            var SquogNPC = Main.npc.FirstOrDefault(x => x.active && x.type == ModContent.NPCType<SquidDogBoss>());

            if (SquogNPC != null)
            {
                latestNPC = Projectile.NewProjectile(SquogNPC.GetSource_FromThis(), new Vector2((int)Projectile.Center.X, yPos), new Vector2(0, 0), type, 10, 0f, Main.myPlayer);
                // ADD CHECK
                allSegments.Add(Main.projectile[latestNPC].ModProjectile as WormProjectile);
            }
            
            // Spawn segment
            Main.projectile[latestNPC].timeLeft = Projectile.timeLeft;
            Main.projectile[latestNPC].ai[1] = oldLatest;

            // Previous segment
            Main.projectile[oldLatest].ai[0] = latestNPC;

            return latestNPC;
        }

        public void HeadAI_Movement(bool collision)
        {
            float speed = MoveSpeed;
            float acceleration = Acceleration;

            float targetXPos, targetYPos;

            Player playerTarget = ChangedUtils.GetClosestPlayer((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), true);//Main.player[NPC.target];

            // Following waving pattern projectile instead of player
            if (Projectile.localAI[2] == -1)
            {
                MoveSpeed = 2.5f;
                Acceleration = 0.2f;

                var projPos = Projectile.Center;
                var closestProjectile = ChangedUtils.GetClosestProjectile(projPos.X, projPos.Y, ModContent.ProjectileType<WavePatternProjectile>());
                if (closestProjectile != null)
                {
                    //Projectile.Center = closestProjectile.Center;
                    ForcedTargetPosition = closestProjectile.Center;
                }
            }

            Vector2 forcedTarget = ForcedTargetPosition ?? playerTarget.Center;
            (targetXPos, targetYPos) = (forcedTarget.X, forcedTarget.Y);
            Vector2 projectileCenter = Projectile.Center;

            float targetRoundedPosX = (float)((int)(targetXPos / 16f) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16f) * 16);
            projectileCenter.X = (float)((int)(projectileCenter.X / 16f) * 16);
            projectileCenter.Y = (float)((int)(projectileCenter.Y / 16f) * 16);
            float dirX = targetRoundedPosX - projectileCenter.X;
            float dirY = targetRoundedPosY - projectileCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            // If we do not have any type of collision, we want the NPC to fall down and de-accelerate along the X axis.
            if (!collision && !CanFly)
                HeadAI_Movement_HandleFallingFromNoCollision(dirX, speed, acceleration);
            else
            {
                HeadAI_Movement_HandleMovement(dirX, dirY, length, speed, acceleration);
            }

            // Else we want to play some audio (soundDelay) and move towards our target.
            if (collision)
                PlayDigSounds(length);

            Leader_Movement_SetRotation(collision);
        }

        public bool OverlappingAnyBlackTile()
        {
            int minTilePosX = (int)(Projectile.Left.X / 16) - 1;
            int maxTilePosX = (int)(Projectile.Right.X / 16) + 2;
            int minTilePosY = (int)(Projectile.Top.Y / 16) - 1;
            int maxTilePosY = (int)(Projectile.Bottom.Y / 16) + 2;

            minTilePosX = Math.Max(0, minTilePosX);
            maxTilePosX = Math.Min(Main.maxTilesX, maxTilePosX);
            minTilePosY = Math.Max(0, minTilePosY);
            maxTilePosY = Math.Min(Main.maxTilesY, maxTilePosY);

            var colorBlack = new Color(0, 0, 0);

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    Tile tile = Main.tile[i, j];
                    Color tmpColor = Lighting.GetColor(i, j);

                    if (tmpColor == colorBlack && tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0))
                        return true;
                }
            }

            return false;
        }

        private bool HeadAI_CheckCollisionForDustSpawns()
        {
            int minTilePosX = (int)(Projectile.Left.X / 16) - 1;
            int maxTilePosX = (int)(Projectile.Right.X / 16) + 2;
            int minTilePosY = (int)(Projectile.Top.Y / 16) - 1;
            int maxTilePosY = (int)(Projectile.Bottom.Y / 16) + 2;

            minTilePosX = Math.Max(0, minTilePosX);
            maxTilePosX = Math.Min(Main.maxTilesX, maxTilePosX);
            minTilePosY = Math.Max(0, minTilePosY);
            maxTilePosY = Math.Min(Main.maxTilesY, maxTilePosY);

            bool collision = false;

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0) || tile.LiquidAmount > 64)
                    {
                        Vector2 tileWorld = new Point16(i, j).ToWorldCoordinates(0, 0);

                        if (Projectile.Right.X > tileWorld.X && Projectile.Left.X < tileWorld.X + 16 && Projectile.Bottom.Y > tileWorld.Y && Projectile.Top.Y < tileWorld.Y + 16)
                        {
                            collision = true;

                            if (Main.rand.NextBool(100))
                                WorldGen.KillTile(i, j, fail: true, effectOnly: true, noItem: false);
                        }
                    }
                }
            }

            return collision;
        }

        private void HeadAI_Movement_HandleMovement(float dirX, float dirY, float length, float speed, float acceleration)
        {
            float absDirX = Math.Abs(dirX);
            float absDirY = Math.Abs(dirY);
            float newSpeed = speed / length;
            dirX *= newSpeed;
            dirY *= newSpeed;

            if ((Projectile.velocity.X > 0 && dirX > 0) || (Projectile.velocity.X < 0 && dirX < 0) || (Projectile.velocity.Y > 0 && dirY > 0) || (Projectile.velocity.Y < 0 && dirY < 0))
            {
                // The NPC is moving towards the target location
                if (Projectile.velocity.X < dirX)
                    Projectile.velocity.X += acceleration;
                else if (Projectile.velocity.X > dirX)
                    Projectile.velocity.X -= acceleration;

                if (Projectile.velocity.Y < dirY)
                    Projectile.velocity.Y += acceleration;
                else if (Projectile.velocity.Y > dirY)
                    Projectile.velocity.Y -= acceleration;

                // The intended Y-velocity is small AND the NPC is moving to the left and the target is to the right of the NPC or vice versa
                if (Math.Abs(dirY) < speed * 0.2 && ((Projectile.velocity.X > 0 && dirX < 0) || (Projectile.velocity.X < 0 && dirX > 0)))
                {
                    if (Projectile.velocity.Y > 0)
                        Projectile.velocity.Y += acceleration * 2f;
                    else
                        Projectile.velocity.Y -= acceleration * 2f;
                }

                // The intended X-velocity is small AND the NPC is moving up/down and the target is below/above the NPC
                if (Math.Abs(dirX) < speed * 0.2 && ((Projectile.velocity.Y > 0 && dirY < 0) || (Projectile.velocity.Y < 0 && dirY > 0)))
                {
                    if (Projectile.velocity.X > 0)
                        Projectile.velocity.X = Projectile.velocity.X + acceleration * 2f;
                    else
                        Projectile.velocity.X = Projectile.velocity.X - acceleration * 2f;
                }
            }
            else if (absDirX > absDirY)
            {
                // The X distance is larger than the Y distance.  Force movement along the X-axis to be stronger
                if (Projectile.velocity.X < dirX)
                    Projectile.velocity.X += acceleration * 1.1f;
                else if (Projectile.velocity.X > dirX)
                    Projectile.velocity.X -= acceleration * 1.1f;

                if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < speed * 0.5)
                {
                    if (Projectile.velocity.Y > 0)
                        Projectile.velocity.Y += acceleration;
                    else
                        Projectile.velocity.Y -= acceleration;
                }
            }
            else
            {
                // The X distance is larger than the Y distance.  Force movement along the X-axis to be stronger
                if (Projectile.velocity.Y < dirY)
                    Projectile.velocity.Y += acceleration * 1.1f;
                else if (Projectile.velocity.Y > dirY)
                    Projectile.velocity.Y -= acceleration * 1.1f;

                if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < speed * 0.5)
                {
                    if (Projectile.velocity.X > 0)
                        Projectile.velocity.X += acceleration;
                    else
                        Projectile.velocity.X -= acceleration;
                }
            }
        }

        private void HeadAI_Movement_HandleFallingFromNoCollision(float dirX, float speed, float acceleration)
        {
            // Keep searching for a new target
            //NPC.TargetClosest(true);

            // Constant gravity of 0.11 pixels/tick
            Projectile.velocity.Y += 0.11f;

            // Ensure that the NPC does not fall too quickly
            if (Projectile.velocity.Y > speed)
                Projectile.velocity.Y = speed;

            // The following behavior mimics vanilla worm movement
            if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < speed * 0.4f)
            {
                // Velocity is sufficiently fast, but not too fast
                if (Projectile.velocity.X < 0.0f)
                    Projectile.velocity.X -= acceleration * 1.1f;
                else
                    Projectile.velocity.X += acceleration * 1.1f;
            }
            else if (Projectile.velocity.Y == speed)
            {
                // NPC has reached terminal velocity
                if (Projectile.velocity.X < dirX)
                    Projectile.velocity.X += acceleration;
                else if (Projectile.velocity.X > dirX)
                    Projectile.velocity.X -= acceleration;
            }
            else if (Projectile.velocity.Y > 4)
            {
                if (Projectile.velocity.X < 0)
                    Projectile.velocity.X += acceleration * 0.9f;
                else
                    Projectile.velocity.X -= acceleration * 0.9f;
            }
        }

        internal virtual void BodyTailAI() { }

        public abstract void Init();
    }

    public abstract class WormHeadProjectile : WormProjectile
    {
        public sealed override WormSegmentType SegmentType => WormSegmentType.Head;


        internal override void BodyTailAI()
        {
            WormBodyProjectile.CommonAI_BodyTail(this);
        }
    }

    public abstract class WormBodyProjectile : WormProjectile
    {
        public sealed override WormSegmentType SegmentType => WormSegmentType.Body;

        internal override void BodyTailAI()
        {
            CommonAI_BodyTail(this);
        }

        internal static void CommonAI_BodyTail(WormProjectile worm)
        {
            //  ai[0] = "follower" segment, the segment that's following this segment
            //  ai[1] = "following" segment, the segment that this segment is following

            var reverseMovement = worm.Projectile.ai[2] > worm.ReverseAfterTime;

            Projectile following = null;

            if (!reverseMovement)
            {
                var projectileIndex = (int)worm.Projectile.ai[1];
                if (projectileIndex >= 0 && projectileIndex < Main.maxProjectiles)
                {
                    following = worm.SegmentType == WormSegmentType.Head ? null : Main.projectile[projectileIndex];
                }
            }
            else
            {
                var projectileIndex = (int)worm.Projectile.ai[0];
                if (projectileIndex >= 0 && projectileIndex < Main.maxProjectiles)
                {
                    following = worm.SegmentType == WormSegmentType.Tail ? null : Main.projectile[projectileIndex];
                }
            }

            if (following is not null)
            {
                // Follow behind the segment "in front" of this NPC
                // Use the current NPC.Center to calculate the direction towards the "parent NPC" of this NPC.
                float dirX = following.Center.X - worm.Projectile.Center.X;
                float dirY = following.Center.Y - worm.Projectile.Center.Y;

                // We then use Atan2 to get a correct rotation towards that parent NPC.
                // Assumes the sprite for the NPC points upward.  You might have to modify this line to properly account for your NPC's orientation
                worm.Projectile.rotation = (float)Math.Atan2(dirY, dirX) + MathHelper.PiOver2;
                // We also get the length of the direction vector.
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

                // We calculate a new, correct distance.
                float dist = (length - worm.Projectile.height) / length;//width
                float posX = dirX * dist;
                float posY = dirY * dist;

                // Reset the velocity of this NPC, because we don't want it to move on its own
                worm.Projectile.velocity = Vector2.Zero;
                // And set this NPCs position accordingly to that of this NPCs parent NPC.
                worm.Projectile.position.X += posX;
                worm.Projectile.position.Y += posY;

                if (reverseMovement)
                {
                    worm.Projectile.rotation += (float)Math.PI;
                }
            }
        }
    }

    // Since the body and tail segments share the same AI
    public abstract class WormTailProjectile : WormProjectile
    {
        public sealed override WormSegmentType SegmentType => WormSegmentType.Tail;

        internal override void BodyTailAI()
        {
            WormBodyProjectile.CommonAI_BodyTail(this);
        }
    }
}