using ChangedSpecialMod.Assets;
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

        /// <summary>
        /// Which type of segment this NPC is considered to be
        /// </summary>
        public abstract WormSegmentType SegmentType { get; }

        /// <summary>
        /// The NPCID or ModContent.NPCType for the body segment NPCs.<br/>
        /// This property is only used if <see cref="HasCustomBodySegments"/> returns <see langword="false"/>.
        /// </summary>
        public abstract int BodyType { get; }

        /// <summary>
        /// The NPCID or ModContent.NPCType for the tail segment NPC.<br/>
        /// This property is only used if <see cref="HasCustomBodySegments"/> returns <see langword="false"/>.
        /// </summary>
        public abstract int TailType { get; }

        /// <summary>
        /// The minimum amount of segments expected, including the head and tail segments
        /// </summary>
        public int MinSegmentLength { get; set; }

        /// <summary>
        /// The maximum amount of segments expected, including the head and tail segments
        /// </summary>
        public int MaxSegmentLength { get; set; }

        /// <summary>
        /// Whether the NPC ignores tile collision when attempting to "dig" through tiles, like how Wyverns work.
        /// </summary>
        public bool CanFly { get; set; }

        /// <summary>
        /// The maximum velocity for the NPC
        /// </summary>
        public float MoveSpeed { get; set; }

        /// <summary>
        /// The rate at which the NPC gains velocity
        /// </summary>
        public float Acceleration { get; set; }

        public int ReverseAfterTime { get; set; }

        public float MaxEntendDistance { get; set; }

        /// <summary>
        /// If not <see langword="null"/>, this NPC will target the given world position instead of its player target
        /// </summary>
        public Vector2? ForcedTargetPosition { get; set; }

        /// <summary>
        /// The NPC instance of the head segment for this worm.
        /// </summary>
        //public NPC HeadSegment => Main.npc[NPC.realLife];

        /// <summary>
        /// The maximum distance in <b>pixels</b> within which the NPC will use tile collision, if <see cref="CanFly"/> returns <see langword="false"/>.<br/>
        /// Defaults to 1000 pixels, which is equivalent to 62.5 tiles.
        /// </summary>
        public virtual int MaxDistanceForUsingTileCollision => 1000;

        /// <summary>
        /// The NPC instance of the segment that this segment is following (ai[1]).  For head segments, this property always returns <see langword="null"/>.
        /// </summary>
        public ModProjectile FollowingNPC => SegmentType == WormSegmentType.Head ? null : Main.projectile[(int)Projectile.ai[1]].ModProjectile;

        /// <summary>
        /// The NPC instance of the segment that is following this segment (ai[0]).  For tail segment, this property always returns <see langword="null"/>.
        /// </summary>
        public ModProjectile FollowerNPC => SegmentType == WormSegmentType.Tail ? null : Main.projectile[(int)Projectile.ai[0]].ModProjectile;


        private bool startDespawning;

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
            // Set the correct rotation for this NPC.
            // Assumes the sprite for the NPC points upward.  You might have to modify this line to properly account for your NPC's orientation
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // Some netupdate stuff (multiplayer compatibility).
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

            // Force a netupdate if the NPC's velocity changed sign and it was not "just hit" by a player
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

                /*
                if (!Projectile.HasValidTarget)
                {
                    NPC.TargetClosest(true);

                    // If the NPC is a boss and it has no target, force it to fall to the underworld quickly
                    if (!NPC.HasValidTarget && NPC.boss)
                    {
                        NPC.velocity.Y += 8f;

                        MoveSpeed = 1000f;

                        if (!startDespawning)
                        {
                            startDespawning = true;

                            // Despawn after 90 ticks (1.5 seconds) if the NPC gets far enough away
                            NPC.timeLeft = 90;
                        }
                    }
                }
                */
            }
            /*
            else
            {
                BodyTailAI();
            }
            */

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
            // If there is no collision with tiles, we check if the distance between this NPC and its target is too large, so that we can still trigger "collision".
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
                        continue;  // Not a valid player

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
            {
                HeadAI_SpawnSegments();
            }

            bool collision = HeadAI_CheckCollisionForDustSpawns();
            HeadAI_CheckTargetDistance(ref collision);
            HeadAI_Movement(collision);
        }

        /// <summary>
        /// Whether the NPC uses
        /// </summary>
        public virtual bool HasCustomBodySegments => false;

        /// <summary>
        /// Override this method to use custom body-spawning code.<br/>
        /// This method only runs if <see cref="HasCustomBodySegments"/> returns <see langword="true"/>.
        /// </summary>
        /// <param name="segmentCount">How many body segments are expected to be spawned</param>
        /// <returns>The whoAmI of the most-recently spawned NPC, which is the result of calling <see cref="NPC.NewNPC(Terraria.DataStructures.IEntitySource, int, int, int, int, float, float, float, float, int)"/></returns>
        public virtual int SpawnBodySegments(int segmentCount)
        {
            // Defaults to just returning this NPC's whoAmI, since the tail segment uses the return value as its "following" NPC index
            return Projectile.whoAmI;
        }

        public void HeadAI_SpawnSegments()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // So, we start the AI off by checking if NPC.ai[0] (the following NPC's whoAmI) is 0.
                // This is practically ALWAYS the case with a freshly spawned NPC, so this means this is the first update.
                // Since this is the first update, we can safely assume we need to spawn the rest of the worm (bodies + tail).
                bool hasFollower = Projectile.ai[0] > 0;
                if (!hasFollower)
                {
                    allSegments.Add(this);

                    // So, here we assign the NPC.realLife value.
                    // The NPC.realLife value is mainly used to determine which NPC loses life when we hit this NPC.
                    // We don't want every single piece of the worm to have its own HP pool, so this is a neat way to fix that.
                    //NPC.realLife = NPC.whoAmI;
                    // latestNPC is going to be used in SpawnSegment() and I'll explain it there.
                    int latestNPC = Projectile.whoAmI;

                    // Here we determine the length of the worm.
                    int randomWormLength = Main.rand.Next(MinSegmentLength, MaxSegmentLength + 1);

                    int distance = randomWormLength - 2;

                    IEntitySource source = Projectile.GetSource_FromAI();

                    // Spawn the body segments like usual
                    while (distance > 0)
                    {
                        latestNPC = SpawnSegment(source, BodyType, latestNPC);
                        distance--;
                    }

                    // Spawn the tail segment
                    var tailIndex = SpawnSegment(source, TailType, latestNPC);
                    var tailProjectile = Main.projectile[tailIndex].ModProjectile as WormProjectile;

                    for (int i = allSegments.Count - 1; i >= 0; i--)
                    {
                        tailProjectile.allSegments.Add(allSegments[i]);
                    }

                    Projectile.netUpdate = true;

                    


                    // Set the player target for good measure
                    //NPC.TargetClosest(true);
                }
            }
        }

        /// <summary>
        /// Spawns a body or tail segment of the worm.
        /// </summary>
        /// <param name="source">The spawn source</param>
        /// <param name="type">The ID of the segment NPC to spawn</param>
        /// <param name="latestNPC">The whoAmI of the most-recently spawned segment NPC in the worm, including the head</param>
        /// <returns></returns>
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
            // MoveSpeed determines the max speed at which this NPC can move.
            // Higher value = faster speed.
            float speed = MoveSpeed;
            // acceleration is exactly what it sounds like. The speed at which this NPC accelerates.
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
            // Using a ValueTuple like this allows for easy assignment of multiple values
            (targetXPos, targetYPos) = (forcedTarget.X, forcedTarget.Y);

            // Copy the value, since it will be clobbered later
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

            // Ensure that the tile range is within the world bounds
            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;

            bool collision = false;

            // This is the initial check for collision with tiles.
            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    Tile tile = Main.tile[i, j];
                    Color tmpColor = Lighting.GetColor(i, j);

                    // If the tile is solid or is considered a platform, then there's valid collision
                    if (tmpColor == new Color(0, 0, 0) && tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0))
                    {
                        collision = true;
                        /*
                        Vector2 tileWorld = new Point16(i, j).ToWorldCoordinates(0, 0);

                        if (Projectile.Right.X > tileWorld.X && Projectile.Left.X < tileWorld.X + 16 && Projectile.Bottom.Y > tileWorld.Y && Projectile.Top.Y < tileWorld.Y + 16)
                        {
                            // Collision found
                            collision = true;

                            if (Main.rand.NextBool(100))
                                WorldGen.KillTile(i, j, fail: true, effectOnly: true, noItem: false);
                        }
                        */
                    }
                }
            }

            return collision;
        }

        private bool HeadAI_CheckCollisionForDustSpawns()
        {
            int minTilePosX = (int)(Projectile.Left.X / 16) - 1;
            int maxTilePosX = (int)(Projectile.Right.X / 16) + 2;
            int minTilePosY = (int)(Projectile.Top.Y / 16) - 1;
            int maxTilePosY = (int)(Projectile.Bottom.Y / 16) + 2;

            // Ensure that the tile range is within the world bounds
            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;

            bool collision = false;

            // This is the initial check for collision with tiles.
            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    Tile tile = Main.tile[i, j];

                    // If the tile is solid or is considered a platform, then there's valid collision
                    if (tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0) || tile.LiquidAmount > 64)
                    {
                        Vector2 tileWorld = new Point16(i, j).ToWorldCoordinates(0, 0);

                        if (Projectile.Right.X > tileWorld.X && Projectile.Left.X < tileWorld.X + 16 && Projectile.Bottom.Y > tileWorld.Y && Projectile.Top.Y < tileWorld.Y + 16)
                        {
                            // Collision found
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