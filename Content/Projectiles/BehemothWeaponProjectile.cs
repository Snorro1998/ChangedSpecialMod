using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public class BehemothWeaponProjectile : ModProjectile
    {
        public double imageSpeed = 5D;
        public int imageIndex = 0;

        public int[] animation = new int[] { 0, 1, 2, 1 };

        public static readonly int[] animIdle = new int[] { 0, 1, 2, 1 };
        public static readonly int[] animSlam = new int[] { 3 };

        public int ImageLength { get { return animation.Length; } }

        private int nSwipes = 10;

        public ref float AIState => ref Projectile.ai[0];
        public ref float AITimer => ref Projectile.ai[1];

        float yOffset = -8;

        private void SwitchAnimation(int[] newAnimation)
        {
            Projectile.frameCounter = 10;
            animation = newAnimation;
        }

        public void FindFrame()
        {
            Projectile.frameCounter++;

            if (Projectile.frameCounter > 10)
            {
                Projectile.frameCounter = 0;
                imageIndex++;
                if (imageIndex >= ImageLength)
                    imageIndex = 0;
                Projectile.frame = animation[imageIndex];
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity = Vector2.Zero;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            Main.projPet[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.DamageType = DamageClass.Summon;
            /*
            Projectile.width = 72;
            Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1800;
            Projectile.tileCollide = false;
            */
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public bool IsOnSolidTile()
        {
            if (Projectile.velocity.Y > 0f)
            {
                int yTile = (int)(Projectile.position.Y + (float)Projectile.height + 7f) / 16;
                int initialXTile = (int)Projectile.position.X / 16;
                int maxXTile = (int)(Projectile.position.X + (float)Projectile.width) / 16;
                for (int xTile = initialXTile; xTile <= maxXTile; xTile++)
                {
                    if (Main.tile[xTile, yTile] == null)
                    {
                        return false;
                    }
                    if (Main.tile[xTile, yTile].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[xTile, yTile].TileType])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void SpawnBlobs()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            var entitySource = Projectile.GetSource_FromAI();
            float hpAmount = 0.25f;
            int count = 20;
            int projWidth = 32;
            int spacing = (int)(96 + 96 * hpAmount);
            var owner = Main.player[Projectile.owner];
            var totalWidth = count * (projWidth + spacing);
            var num5 = 6 + Main.rand.Next(6);
            var yPos = owner.Center.Y - 16 * 40;
            float targetX = owner.Center.X;

            var projType = ModContent.ProjectileType<WhiteLatexProjectileFriendly>();
            var damage = 40;

            for (int i = 0; i < count; i++)
            {
                var xPos = (int)(targetX - 0.5 * totalWidth + i * (projWidth + spacing));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(xPos, yPos), new Vector2(0, 1) * (8f + Main.rand.NextFloat() * 8f), projType, damage, 0f, Main.myPlayer, 0f, num5);
            }
        }

        private void StateDash(bool haltMovement)
        {
            AITimer += 1f;
            int dashSpeed = 10;
            Projectile.tileCollide = false;
            var attackTarget = (int)Projectile.localAI[0];

            if (AITimer == 1f)
            {
                // Follow up swipe. Break out if the target is no longer valid
                if (Projectile.ai[2] > 0)
                {
                    var targetIndexValid = attackTarget >= 0 && attackTarget < Main.maxNPCs;
                    if (!targetIndexValid || !Main.npc[attackTarget].active)
                    {
                        Projectile.velocity = Vector2.Zero;
                        Projectile.ai[0] = 0;
                        Projectile.ai[1] = 0;
                        Projectile.ai[2] = 0;
                        Projectile.localAI[0] = -1;
                        return;
                    }
                }

                Projectile.velocity = new Vector2(0, 0);
                if (attackTarget == -1)
                {
                    Projectile.Minion_FindTargetInRange(600, ref attackTarget, skipIfCannotHitWithOwnBody: false, delegate { return true; });
                    Projectile.localAI[0] = attackTarget;
                }
            }

            var targetIndexValid2 = attackTarget >= 0 && attackTarget < Main.maxNPCs;
            if (!targetIndexValid2 || !Main.npc[attackTarget].active)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[0] = 0;
                Projectile.ai[1] = 0;
                Projectile.ai[2] = 0;
                Projectile.localAI[0] = -1;
                return;
            }

            var tmpTarget = Main.npc[attackTarget];
            var tmpTargetPos = tmpTarget.Center;
            var pos = Projectile.Center;
            var offset = 75;
            var xOffset = ((int)Projectile.ai[2]) % 2 == 0 ? -offset : offset;
            var targetX = tmpTargetPos.X + xOffset;
            var targetY = tmpTargetPos.Y;
            var targetPos = new Vector2(targetX, targetY);

            var moveVector = targetPos - pos;
            var dist = moveVector.Length();
            moveVector.Normalize();
            moveVector *= dashSpeed;
            Projectile.velocity = moveVector;

            if (dist < 10)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[1] = 0;
                Projectile.ai[2]++;
                Projectile.localAI[1]++;
            }

            if (Projectile.ai[2] >= nSwipes)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[0] = 0;
                Projectile.ai[1] = 0;
                Projectile.localAI[0] = -1;
            }
        }

        private void StateSlam()
        {
            AITimer += 1f;

            // Start
            if (AITimer == 1)
            {
                SwitchAnimation(animSlam);
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = -5f;
            }

            // Hit the floor or after 5 seconds just in case
            if (IsOnSolidTile() || AITimer > 5 * 60)
            {
                PunchCameraModifier modifier4 = new PunchCameraModifier(Projectile.Center, new Vector2(0f, -1f), 20f, 6f, 30, 1000f, "Deerclops");
                Main.instance.CameraModifiers.Add(modifier4);
                SoundEngine.PlaySound(Sounds.SoundSlam, Projectile.Center);
                SpawnBlobs();

                Projectile.ai[0] = 0;
                Projectile.ai[1] = 0;
                SwitchAnimation(animIdle);
            }

            Projectile.velocity.Y += 0.3f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.BehemothWeaponBuff>());
                return;
            }

            if (player.HasBuff(ModContent.BuffType<Buffs.BehemothWeaponBuff>()))
                Projectile.timeLeft = 2;

            FindFrame();

            switch (AIState)
            {
                case 3:
                    Projectile.rotation = 0f;
                    StateDash(false);
                    break;
                case 4:
                    Projectile.rotation = 0f;
                    StateSlam();
                    break;
                default:
                    Movement(Projectile);
                    break;
            }

            // Set sprite direction
            if (Projectile.velocity.X != 0f)
            {
                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            }
        }

        public void Movement(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            int targetingRange = 450;

            var playerPosition = player.position;
            var playerCenter = player.Center;

            float flyDistance = 500f;
            float walkDistance = 300f;

            // vector is the target position
            Vector2 vector = playerCenter;
            vector.X -= (45 + player.width / 2) * player.direction;
            vector.X -= projectile.minionPos * 30 * player.direction;

            if (player.direction == -1)
                vector.X += 64;

            //vector.X += 24;
            vector.X -= 16;

            projectile.shouldFallThrough = playerPosition.Y + (float)player.height - 12f > projectile.position.Y + (float)projectile.height;
            projectile.friendly = false;
            int num8 = 0;
            int num9 = 15;
            int attackTarget = -1;
            projectile.friendly = true;

            bool walkState = projectile.ai[0] == 0f;
            bool flyState = projectile.ai[0] == 1f;

            if (walkState)
            {
                projectile.Minion_FindTargetInRange(targetingRange, ref attackTarget, skipIfCannotHitWithOwnBody: true, delegate { return true; });
            }

            if (attackTarget != -1)
            {
                // Slam
                if (projectile.ai[2] >= nSwipes || projectile.localAI[1] >= nSwipes)
                {
                    projectile.ai[0] = 4;
                    projectile.ai[1] = 0;
                    projectile.ai[2] = 0;
                    projectile.localAI[1] = 0;
                }
                // Swipe
                else
                {
                    projectile.ai[0] = 3;
                    projectile.ai[1] = 0;
                    projectile.localAI[0] = -1;
                }

                return;
            }

            float playerDistance;
            float myDistance;
            bool closerIsMe;

            // Flying towards player
            if (flyState)
            {
                projectile.tileCollide = false;
                float acceleration = 0.2f;
                float num18 = 10f;
                int num19 = 200;
                if (num18 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                {
                    num18 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                }
                Vector2 distanceToPlayerVector = playerCenter - projectile.Center;
                float distanceToPlayer = distanceToPlayerVector.Length();
                // Teleport to player if very far away
                if (distanceToPlayer > 2000f)
                {
                    projectile.position = playerCenter - new Vector2(projectile.width, projectile.height) / 2f;
                }

                if (distanceToPlayer < (float)num19 && player.velocity.Y == 0f &&
                    projectile.position.Y + (float)projectile.height <= player.position.Y + (float)player.height &&
                    !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    // Switch to walk state
                    projectile.ai[0] = 0f;
                    projectile.netUpdate = true;
                    if (projectile.velocity.Y < -6f)
                    {
                        projectile.velocity.Y = -6f;
                    }
                }
                if (distanceToPlayer >= 60f)
                {
                    distanceToPlayerVector.Normalize();
                    distanceToPlayerVector *= num18;
                    if (projectile.velocity.X < distanceToPlayerVector.X)
                    {
                        projectile.velocity.X += acceleration;
                        if (projectile.velocity.X < 0f)
                        {
                            projectile.velocity.X += acceleration * 1.5f;
                        }
                    }
                    if (projectile.velocity.X > distanceToPlayerVector.X)
                    {
                        projectile.velocity.X -= acceleration;
                        if (projectile.velocity.X > 0f)
                        {
                            projectile.velocity.X -= acceleration * 1.5f;
                        }
                    }
                    if (projectile.velocity.Y < distanceToPlayerVector.Y)
                    {
                        projectile.velocity.Y += acceleration;
                        if (projectile.velocity.Y < 0f)
                        {
                            projectile.velocity.Y += acceleration * 1.5f;
                        }
                    }
                    if (projectile.velocity.Y > distanceToPlayerVector.Y)
                    {
                        projectile.velocity.Y -= acceleration;
                        if (projectile.velocity.Y > 0f)
                        {
                            projectile.velocity.Y -= acceleration * 1.5f;
                        }
                    }
                }
                // Set sprite direction
                if (projectile.velocity.X != 0f)
                {
                    projectile.spriteDirection = Math.Sign(projectile.velocity.X);
                }
            }
            if (projectile.ai[0] == 2f && projectile.ai[1] < 0f)
            {
                projectile.friendly = false;
                projectile.ai[1] += 1f;
                if (num9 >= 0)
                {
                    projectile.ai[1] = 0f;
                    // Switch to walk state
                    projectile.ai[0] = 0f;
                    projectile.netUpdate = true;
                    return;
                }
            }
            else if (projectile.ai[0] == 2f)
            {
                projectile.spriteDirection = projectile.direction;
                projectile.rotation = 0f;

                projectile.velocity.Y += 0.4f;
                if (projectile.velocity.Y > 10f)
                {
                    projectile.velocity.Y = 10f;
                }
                projectile.ai[1] -= 1f;
                if (projectile.ai[1] <= 0f)
                {
                    if (num8 <= 0)
                    {
                        projectile.ai[1] = 0f;
                        // Switch to walk state
                        projectile.ai[0] = 0f;
                        projectile.netUpdate = true;
                        return;
                    }
                    projectile.ai[1] = -num8;
                }
            }

            // Has a target
            if (attackTarget >= 0)
            {
                float maxDistance2 = targetingRange;

                NPC nPC2 = Main.npc[attackTarget];
                Vector2 targetCenter = nPC2.Center;
                vector = targetCenter;
                if (projectile.IsInRangeOfMeOrMyOwner(nPC2, maxDistance2, out myDistance, out playerDistance, out closerIsMe))
                {
                    projectile.shouldFallThrough = nPC2.Center.Y > projectile.Bottom.Y;
                    bool grounded = projectile.velocity.Y == 0f;
                    if (projectile.wet && projectile.velocity.Y > 0f && !projectile.shouldFallThrough)
                    {
                        grounded = true;
                    }
                    // Jump if grounded
                    if (targetCenter.Y < projectile.Center.Y - 30f && grounded)
                    {
                        float num26 = (targetCenter.Y - projectile.Center.Y) * -1f;
                        float num27 = 0.4f;
                        float num28 = (float)Math.Sqrt(num26 * 2f * num27);
                        // Clamp jump height
                        if (num28 > 26f)
                        {
                            num28 = 26f;
                        }
                        projectile.velocity.Y = 0f - num28;
                    }
                }
            }
            // Walk state, no target
            if (projectile.ai[0] == 0f && attackTarget < 0)
            {
                // Fly if the player is using rocket boots
                if (Main.player[projectile.owner].rocketDelay2 > 0)
                {
                    // Switch to fly state
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                }
                Vector2 vectorToPlayer = playerCenter - projectile.Center;
                // Teleport to player if very far away
                if (vectorToPlayer.Length() > 2000f)
                {
                    projectile.position = playerCenter - new Vector2(projectile.width, projectile.height) / 2f;
                }
                else if (vectorToPlayer.Length() > flyDistance || Math.Abs(vectorToPlayer.Y) > walkDistance)
                {
                    // Switch to fly state
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                    if (projectile.velocity.Y > 0f && vectorToPlayer.Y < 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    if (projectile.velocity.Y < 0f && vectorToPlayer.Y > 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                }
            }
            // Walk state
            if (projectile.ai[0] == 0f)
            {
                // No target
                if (attackTarget < 0)
                {
                    if (projectile.Distance(playerCenter) > 60f && projectile.Distance(vector) > 60f &&
                        Math.Sign(vector.X - playerCenter.X) != Math.Sign(projectile.Center.X - playerCenter.X))
                    {
                        vector = playerCenter;
                    }
                    Rectangle r = Utils.CenteredRectangle(vector, projectile.Size);
                    for (int i = 0; i < 20; i++)
                    {
                        if (Collision.SolidCollision(r.TopLeft(), r.Width, r.Height))
                        {
                            break;
                        }
                        r.Y += 16;
                        vector.Y += 16f;
                    }
                    Vector2 vector8 = Collision.TileCollision(playerCenter - projectile.Size / 2f, vector - playerCenter, projectile.width, projectile.height);
                    vector = playerCenter - projectile.Size / 2f + vector8;
                    if (projectile.Distance(vector) < 32f)
                    {
                        float distFromPlayerToTarget = playerCenter.Distance(vector);
                        if (playerCenter.Distance(projectile.Center) < distFromPlayerToTarget)
                        {
                            vector = projectile.Center;
                        }
                    }
                    Vector2 vectorTargetToPlayer = playerCenter - vector;
                    if (vectorTargetToPlayer.Length() > flyDistance || Math.Abs(vectorTargetToPlayer.Y) > walkDistance)
                    {
                        Rectangle r2 = Utils.CenteredRectangle(playerCenter, projectile.Size);
                        Vector2 vector10 = vector - playerCenter;
                        Vector2 vector11 = r2.TopLeft();
                        for (float num33 = 0f; num33 < 1f; num33 += 0.05f)
                        {
                            Vector2 vector12 = r2.TopLeft() + vector10 * num33;
                            if (Collision.SolidCollision(r2.TopLeft() + vector10 * num33, r.Width, r.Height))
                            {
                                break;
                            }
                            vector11 = vector12;
                        }
                        vector = vector11 + projectile.Size / 2f;
                    }
                }
                projectile.tileCollide = true;
                float acceleration2 = 0.5f;
                float xSpeedMax2 = 4f;
                float xSpeedMax = 4f;
                float slowAcceleration = 0.1f;
                if (attackTarget != -1)
                {
                    acceleration2 = 0.65f;
                    xSpeedMax2 = 5.5f;
                    xSpeedMax = 5.5f;
                }

                if (xSpeedMax < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                {
                    xSpeedMax = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                    acceleration2 = 0.7f;
                }

                int num39 = 0;
                bool shouldJump = false;
                float xDistToTarget = vector.X - projectile.Center.X;
                Vector2 vectorToTarget = vector - projectile.Center;
                if (Math.Abs(xDistToTarget) > 5f)
                {
                    // Target to the left
                    if (xDistToTarget < 0f)
                    {
                        num39 = -1;
                        if (projectile.velocity.X > 0f - xSpeedMax2)
                        {
                            projectile.velocity.X -= acceleration2;
                        }
                        else
                        {
                            projectile.velocity.X -= slowAcceleration;
                        }
                    }
                    // Target to the right
                    else
                    {
                        num39 = 1;
                        if (projectile.velocity.X < xSpeedMax2)
                        {
                            projectile.velocity.X += acceleration2;
                        }
                        else
                        {
                            projectile.velocity.X += slowAcceleration;
                        }
                    }

                    bool flag14 = true;
                    flag14 = attackTarget > -1 && Main.npc[attackTarget].Hitbox.Intersects(projectile.Hitbox);

                    if (flag14)
                    {
                        shouldJump = true;
                    }
                }
                else
                {
                    projectile.velocity.X *= 0.9f;
                    if (Math.Abs(projectile.velocity.X) < acceleration2 * 2f)
                    {
                        projectile.velocity.X = 0f;
                    }
                }
                bool flag15 = Math.Abs(vectorToTarget.X) >= 64f || (vectorToTarget.Y <= -48f && Math.Abs(vectorToTarget.X) >= 8f);
                if (num39 != 0 && flag15)
                {
                    int num41 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                    int num42 = (int)projectile.position.Y / 16;
                    num41 += num39;
                    num41 += (int)projectile.velocity.X;
                    for (int j = num42; j < num42 + projectile.height / 16 + 1; j++)
                    {
                        if (WorldGen.SolidTile(num41, j))
                        {
                            shouldJump = true;
                        }
                    }
                }

                Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
                float num43 = Utils.GetLerpValue(0f, 100f, vectorToTarget.Y, clamped: true) * Utils.GetLerpValue(-2f, -6f, projectile.velocity.Y, clamped: true);
                if (projectile.velocity.Y == 0f)
                {
                    if (shouldJump)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            int projectileXPosBottom = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                            if (k == 0)
                            {
                                projectileXPosBottom = (int)projectile.position.X / 16;
                            }
                            if (k == 2)
                            {
                                projectileXPosBottom = (int)(projectile.position.X + (float)projectile.width) / 16;
                            }
                            int projectileYPosBottom = (int)(projectile.position.Y + (float)projectile.height) / 16;
                            if (!WorldGen.SolidTile(projectileXPosBottom, projectileYPosBottom) && !Main.tile[projectileXPosBottom, projectileYPosBottom].IsHalfBlock && 
                                Main.tile[projectileXPosBottom, projectileYPosBottom].Slope <= 0 &&
                                (!TileID.Sets.Platforms[Main.tile[projectileXPosBottom, projectileYPosBottom].TileType] || 
                                !Main.tile[projectileXPosBottom, projectileYPosBottom].HasTile || Main.tile[projectileXPosBottom, projectileYPosBottom].IsActuated))
                            {
                                continue;
                            }
                            try
                            {
                                projectileXPosBottom = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                                projectileYPosBottom = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
                                projectileXPosBottom += num39;
                                projectileXPosBottom += (int)projectile.velocity.X;
                                if (!WorldGen.SolidTile(projectileXPosBottom, projectileYPosBottom - 1) && 
                                    !WorldGen.SolidTile(projectileXPosBottom, projectileYPosBottom - 2))
                                {
                                    projectile.velocity.Y = -5.1f;
                                }
                                else if (!WorldGen.SolidTile(projectileXPosBottom, projectileYPosBottom - 2))
                                {
                                    projectile.velocity.Y = -7.1f;
                                }
                                else if (WorldGen.SolidTile(projectileXPosBottom, projectileYPosBottom - 5))
                                {
                                    projectile.velocity.Y = -11.1f;
                                }
                                else if (WorldGen.SolidTile(projectileXPosBottom, projectileYPosBottom - 4))
                                {
                                    projectile.velocity.Y = -10.1f;
                                }
                                else
                                {
                                    projectile.velocity.Y = -9.1f;
                                }
                            }
                            catch
                            {
                                projectile.velocity.Y = -9.1f;
                            }
                        }
                        if (vector.Y - projectile.Center.Y < -48f)
                        {
                            float num46 = vector.Y - projectile.Center.Y;
                            num46 *= -1f;
                            if (num46 < 60f)
                            {
                                projectile.velocity.Y = -6f;
                            }
                            else if (num46 < 80f)
                            {
                                projectile.velocity.Y = -7f;
                            }
                            else if (num46 < 100f)
                            {
                                projectile.velocity.Y = -8f;
                            }
                            else if (num46 < 120f)
                            {
                                projectile.velocity.Y = -9f;
                            }
                            else if (num46 < 140f)
                            {
                                projectile.velocity.Y = -10f;
                            }
                            else if (num46 < 160f)
                            {
                                projectile.velocity.Y = -11f;
                            }
                            else if (num46 < 190f)
                            {
                                projectile.velocity.Y = -12f;
                            }
                            else if (num46 < 210f)
                            {
                                projectile.velocity.Y = -13f;
                            }
                            else if (num46 < 270f)
                            {
                                projectile.velocity.Y = -14f;
                            }
                            else if (num46 < 310f)
                            {
                                projectile.velocity.Y = -15f;
                            }
                            else
                            {
                                projectile.velocity.Y = -16f;
                            }
                        }
                        if (projectile.wet && num43 == 0f)
                        {
                            projectile.velocity.Y *= 2f;
                        }
                    }
                }
                if (projectile.velocity.X > xSpeedMax)
                {
                    projectile.velocity.X = xSpeedMax;
                }
                if (projectile.velocity.X < -xSpeedMax)
                {
                    projectile.velocity.X = -xSpeedMax;
                }
                if (projectile.velocity.X < 0f)
                {
                    projectile.direction = -1;
                }
                if (projectile.velocity.X > 0f)
                {
                    projectile.direction = 1;
                }
                if (projectile.velocity.X == 0f)
                {
                    projectile.direction = ((playerCenter.X > projectile.Center.X) ? 1 : (-1));
                }
                if (projectile.velocity.X > acceleration2 && num39 == 1)
                {
                    projectile.direction = 1;
                }
                if (projectile.velocity.X < 0f - acceleration2 && num39 == -1)
                {
                    projectile.direction = -1;
                }
                projectile.spriteDirection = projectile.direction;

                if (projectile.velocity.Y == 0f)
                {
                    projectile.rotation = projectile.rotation.AngleTowards(0f, 0.3f);
                }
                // Jumping or falling
                else if (projectile.velocity.Y != 0f)
                {
                    projectile.rotation = Math.Min(4f, projectile.velocity.Y) * -0.1f;
                    // Fix rotation when falling facing left
                    if (projectile.spriteDirection == -1)
                    {
                        projectile.rotation -= (float)Math.PI * 2f;
                    }
                }

                projectile.velocity.Y += 0.4f + num43 * 1f;

                // Clamp falling velocity
                if (projectile.velocity.Y > 10f)
                {
                    projectile.velocity.Y = 10f;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        // I am sick of this thing, so just draw it higher. No idea if that will cause issues but I dont care anymore
        public override bool PreDraw(ref Color lightColor)
        {
            // It doesnt actually draw it centered, but with yoff
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor, null, true, yOffset);
            return false;
        }
    }
}