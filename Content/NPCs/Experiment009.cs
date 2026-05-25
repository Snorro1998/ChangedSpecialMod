using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;


// bugs
// He has no sprite or animation for flying

namespace ChangedSpecialMod.Content.NPCs
{
    [AutoloadBossHead]
    public class Experiment009 : ModNPC
    {
        private enum ActionState
        {
            Idle,
            PickAttack,
            AttackSpread,
            AttackOrb,
            AttackDown,
            AttackFlyHorizontal,
            DashToPlayer
        }

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];
        public ref float AIAttack => ref NPC.ai[2];
        public ref float AIImpatienceTimer => ref NPC.ai[3];


        private bool SecondPhase { get { return NPC.life < NPC.lifeMax / 2; } }

        public SoundStyle SoundLightning = SoundID.Item113;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
            ChangedUtils.HideFromBestiary(this);
        }

        public override void SetDefaults() 
        {
            NPC.width = 18;
            NPC.height = 45;
            NPC.damage = 45;
            NPC.defense = 16;
            NPC.lifeMax = 10000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.0f;
            NPC.aiStyle = -1;
            NPC.npcSlots = 10f;
            AIType = NPCID.None;
            AnimationType = NPCID.None;
            NPC.boss = true;
            AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };

            NPC.waterMovementSpeed = 1f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.scale = 2.0f;

            var changedNPC = NPC.Changed();

            changedNPC.HatXOffset = 0;
            changedNPC.HatYOffset = -32;
            changedNPC.RemoveAllHats();

            changedNPC.GooType = GooType.White;
            changedNPC.ElementType = ElementType.Water;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Electrified, 60);
        }

        public override void OnKill()
        {
            var projs = Main.projectile.Where(x => x != null && x.active && x.type == ProjectileID.VortexVortexPortal);
            foreach (var proj in projs)
            {
                proj.active = false;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Main.NewText("This boss fight is an experiment, so don't expect much from it");
            NPC.TargetClosest(false);
        }

        private void SwitchState(ActionState newState)
        {
            AIState = (float)newState;
            AITimer = 0;
            NPC.netUpdate = true;
        }

        private void CheckIfShouldDash()
        {
            if (!NPC.HasValidTarget)
                NPC.TargetClosest(false);
            if (NPC.HasValidTarget)
            {
                var player = Main.player[NPC.target];
                var maxDist = 16 * 60;
                if (Vector2.DistanceSquared(player.Center, NPC.Center) > maxDist * maxDist)
                {
                    SwitchState(ActionState.DashToPlayer);
                }
            }
        }

        private void StateIdle()
        {
            AITimer++;

            if (NPC.position.Distance(NPC.oldPosition) < 0.05f)
                AIImpatienceTimer++;
            else if (AIImpatienceTimer > 0)
                AIImpatienceTimer--;

            if (AIImpatienceTimer > 120)
            {
                // Force fly up attack if stuck for a bit
                SwitchState(ActionState.PickAttack);
                AITimer = 50;
                AIAttack = 2;
            }

            CheckIfShouldDash();

            if (AITimer >= 2 * 60)
            {
                SwitchState(ActionState.PickAttack);
            }
        }


        private void Teleport(Vector2 newPosition)
        {
            NPC.Center = newPosition;

            for (int i = 0; i < 50; i++)
            {
                var dust = Dust.NewDust(NPC.TopLeft, NPC.width, NPC.height, DustID.Electric);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.alpha = 0;
            NPC.color = Color.Transparent;
            var sparkChance = 4;

            if (AIState != (float)ActionState.Idle)
            {
                sparkChance = 1;
                NPC.alpha = 200;
                NPC.color = Color.LightBlue;
            }
            else
            {
                NPC.alpha = 255;
                NPC.color = Color.White;
            }

            if (Main.rand.NextBool(sparkChance))
            {
                var dust = Dust.NewDust(NPC.TopLeft, NPC.width, NPC.height, DustID.Electric);
                Main.dust[dust].noGravity = true;
            }

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        #region Attacks

        // Pick the next attack
        private void StatePickAttack()
        {
            AITimer++;

            if (AITimer == 60)
            {
                NPC.TargetClosest(false);
                if (NPC.HasValidTarget)
                {
                    var entitySource = NPC.GetSource_FromAI();
                    var attack = AIAttack;
                    int xPos = (int)NPC.Center.X;
                    int yPos = (int)NPC.Center.Y;

                    if (AIAttack == 0)
                    {
                        SwitchState(ActionState.AttackSpread);
                        AIAttack++;
                    }
                    else if (AIAttack == 1)
                    {
                        SwitchState(ActionState.AttackOrb);
                        AIAttack++;
                    }
                    else if (AIAttack == 2)
                    {
                        SwitchState(ActionState.AttackFlyHorizontal);
                        AIAttack++;
                    }
                    else
                    {
                        SwitchState(ActionState.AttackDown);
                        AIAttack = 0;
                    }
                }
            }

            // Switch back to idle state in case the attack failed to start
            else if (AITimer >= 2 * 60)
            {
                SwitchState(ActionState.Idle);
            }
        }

        private void SpawnLightningBoltTowardsPlayer(float speedNew = -1, bool random = false)
        {
            if (!NPC.HasValidTarget)
                return;

            Player player = Main.player[NPC.target];
            var targetPos = player.Center;
            if (random)
            {
                targetPos.X += Main.rand.Next(-48, 48);
                targetPos.Y += Main.rand.Next(-32, 32);
            }    
            Vector2 direction = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            var projectileType = ProjectileID.VortexLightning;
            var pos = NPC.Center;
            var speed = 6.0f;
            if (speedNew != -1)
                speed = speedNew;

            Projectile.NewProjectile(
                NPC.GetSource_FromAI(),
                pos,
                direction * speed,
                projectileType,
                25,
                0f,
                Main.myPlayer,
                direction.ToRotation()
            );
        }

        private void StateAttackDown()
        {
            var nPerStrike = GetValueBasedOnHealth(5, 12);
            var speed = GetValueBasedOnHealth(7, 13);

            if (AITimer == 1)
            {
                Main.NewLightning();
                PunchCameraModifier modifier4 = new PunchCameraModifier(NPC.Center, new Vector2(0f, -1f), 20f, 6f, 60, 1000f, "Experiment009");
                Main.instance.CameraModifiers.Add(modifier4);
                SoundEngine.PlaySound(SoundID.Thunder);
                if (NPC.HasValidTarget)
                {
                    Player player = Main.player[NPC.target];

                    for (int i = 0; i < nPerStrike; i++)
                    {
                        var perStrike = 120 / nPerStrike;
                        var projectileType = ProjectileID.VortexLightning;
                        var xOff = (-60 + perStrike * (i + 0.5f)) * 16 + 32 * player.velocity.X;
                        var pos = player.Center + new Vector2(xOff, -16 * 35);
                        var direction = new Vector2(0, 1).ToRotation();
                        var velocity = direction.ToRotationVector2() * speed;
                        var randomShape = Main.rand.Next(0, 100);
                        var projId = Projectile.NewProjectile(
                            NPC.GetSource_FromAI(),
                            pos,
                            velocity,//velocity,
                            projectileType,
                            25,
                            0f,
                            Main.myPlayer,
                            direction,
                            randomShape
                        );
                    }
                }
            }

            else if (AITimer >= 60)
            {
                SwitchState(ActionState.Idle);
            }

            AITimer++;
        }

        private void StateAttackOrb()
        {
            AITimer++;
            var nProjs = 6;
            var anglePerProjectile = (float)(Math.PI * 2 / nProjs);
            var nWaves = 4;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                var proj = Main.projectile[i];

                if (proj != null && proj.active && proj.type == ProjectileID.VortexVortexPortal)
                {
                    var val = (int)proj.ai[0];

                    // Why
                    if (val == 100 || val == 101)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            var projectileType = ProjectileID.VortexLightning;
                            var pos = proj.position;
                            var direction = proj.velocity.SafeNormalize(Vector2.UnitY).ToRotation();
                            direction -= 0.5f * anglePerProjectile;
                            direction += j * anglePerProjectile;
                            var velocity = direction.ToRotationVector2() * proj.velocity.Length();
                            var projId = Projectile.NewProjectile(
                                NPC.GetSource_FromAI(),
                                pos,
                                velocity,//velocity,
                                projectileType,
                                25,
                                0f,
                                Main.myPlayer,
                                direction
                            );

                            if (projId != -1)
                            {
                                var proj2 = Main.projectile[projId];
                                proj2.friendly = false;
                                proj2.hostile = true;
                                proj2.timeLeft = 2 * 60;
                            }
                        }
                    }

                    // This speed up the animation. One is added every update normally
                    proj.ai[0] += 1;
                }
            }

            if (AITimer % 60 == 0 && AITimer < nWaves * 60)
            {
                var isSecond = (AITimer % 120) == 0;
                for (int i = 0; i < nProjs; i++)
                {
                    var projectileType = ProjectileID.VortexVortexPortal;
                    var pos = NPC.Center - Vector2.UnitY * 32;
                    var rot = (float)(i * anglePerProjectile);
                    if (isSecond)
                        rot += 0.5f * anglePerProjectile;
                    var direction = rot.ToRotationVector2();
                    var speed = 6;//8

                    var projId = Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        pos,
                        direction * speed,
                        projectileType,
                        25,
                        0f,
                        Main.myPlayer,
                        direction.ToRotation()
                    );

                    if (projId != -1)
                    {
                        var proj = Main.projectile[projId];
                        proj.friendly = false;
                        proj.hostile = true;
                        proj.timeLeft = 2 * 60;
                    }
                }
            }


            else if (AITimer >= 60 * 4)
            {
                SwitchState(ActionState.Idle);
            }
        }

        private void StateAttackFlyHorizontal()
        {
            AITimer++;
            var timeFlyUp = 60 * 1.5f;
            var timeAttackTotal = 60 * 3.5f;
            var interval = (int)GetValueBasedOnHealth(20, 12);
            var speed = (int)GetValueBasedOnHealth(6, 10);

            if (AITimer == 1)
            {
                if (NPC.HasValidTarget)
                {
                    NPC.velocity = new Vector2(0, -20);
                }
            }

            if (AITimer == timeFlyUp)
            {
                Player player = Main.player[NPC.target];
                var xPos = player.Center.X - 60 * 16;
                var yPos = player.Center.Y - 30 * 16;
                NPC.position = new Vector2(xPos, yPos);
                NPC.velocity = new Vector2(20, 0);
            }

            if (AITimer > timeFlyUp && (int)AITimer % interval == 0)
            {
                SpawnLightningBoltTowardsPlayer(speed, true);
            }

            if (AITimer > timeAttackTotal)
            {
                Player player = Main.player[NPC.target];
                var xPos = player.Center.X;
                var yPos = player.Center.Y - 35 * 16;
                //NPC.position = new Vector2(xPos, yPos);
                NPC.velocity = new Vector2(0, 0);
                Teleport(new Vector2(xPos, yPos));
                SwitchState(ActionState.Idle);
            }
        }

        private void StateAttackSpread()
        {
            AITimer++;

            if (AITimer == 1)
            {
                if (NPC.HasValidTarget)
                {
                    Player player = Main.player[NPC.target];
                    Vector2 direction = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
                    var projectileType = ProjectileID.VortexLightning;
                    var pos = NPC.Center - Vector2.UnitY * 32;

                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        pos,
                        direction * 3,
                        projectileType,
                        25,
                        0f,
                        Main.myPlayer,
                        direction.ToRotation()
                    );
                }
            }

            else if ((int)AITimer % 45 == 0)
            {
                var projs = Main.projectile.Where(x => x != null && x.active && x.type == ProjectileID.VortexLightning).ToList();
                foreach (var proj in projs)
                {
                    var pos = proj.Center;

                    Player player = Main.player[NPC.target];
                    Vector2 direction = (player.Center - pos).SafeNormalize(Vector2.UnitY);
                    var projectileType = ProjectileID.VortexLightning;

                    NPC.localAI[0] = Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        pos,
                        direction * 3,
                        projectileType,
                        25,
                        0f,
                        Main.myPlayer,
                        direction.ToRotation()
                    );
                }
            }

            else if (AITimer >= 60 * 3)
            {
                SwitchState(ActionState.Idle);
            }
        }


        #endregion

        // Dash towards player if trying to escape
        private void TeleportToPlayer()
        {
            AITimer++;
            
            if (AITimer == 1)
            {
                var player = Main.player[NPC.target];
                int targetTileX = (int)player.position.X / 16;
                int targetTileY = (int)player.position.Y / 16;
                Vector2 chosenTile = Vector2.Zero;
                if (NPC.AI_AttemptToFindTeleportSpot(ref chosenTile, targetTileX, targetTileY, 20, 10, 1, false, true))
                {
                    Teleport(chosenTile * 16);
                }
                else
                {
                    Teleport(player.position);
                }

                SwitchState(ActionState.Idle);
            }
        }

        private float GetValueBasedOnHealth(float valueMin, float valueMax)
        {
            float hpAmount = (float)NPC.life / (float)NPC.lifeMax;
            float hpFactor = 1f - hpAmount;
            return (float)Utils.Lerp(valueMin, valueMax, hpFactor);
        }

        private void Movement(bool haltMovement)
        {
            var walkSpeed = GetValueBasedOnHealth(2, 3);

            float num5 = -0.4f;
            float min = -8f;
            float num6 = 0.4f;
            Rectangle targetRectangle = NPC.GetTargetData().Hitbox;
            float xDistanceToTarget = (float)targetRectangle.Center.X - NPC.Center.X;
            float xDistanceToTargetAbs = Math.Abs(xDistanceToTarget);
            bool closeToTarget = xDistanceToTargetAbs < 16f;
            bool stopMoving = closeToTarget || haltMovement;

            if (stopMoving)
            {
                NPC.velocity.X *= 0.9f;
                if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                {
                    NPC.velocity.X = 0f;
                }
            }
            else
            {
                // Direction to face the target
                int targetDirection = Math.Sign(xDistanceToTarget);
                var acceleration = 0.1f;
                NPC.velocity.X += targetDirection * acceleration;
                NPC.velocity.X = Math.Clamp(NPC.velocity.X, -walkSpeed, walkSpeed);
            }

            if (NPC.velocity.X != 0)
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);

            int num10 = 40;
            int num11 = 20;
            int num12 = 0;
            Vector2 vector = new Vector2(NPC.Center.X - (float)(num10 / 2), NPC.position.Y + (float)NPC.height - (float)num11 + (float)num12);
            bool num13 = vector.X < (float)targetRectangle.X && vector.X + (float)NPC.width > (float)(targetRectangle.X + targetRectangle.Width);
            bool flag3 = vector.Y + (float)num11 < (float)(targetRectangle.Y + targetRectangle.Height - 16);
            bool acceptTopSurfaces = NPC.Bottom.Y >= (float)targetRectangle.Top;
            bool flag4 = ChangedUtils.SolidCollision(vector, num10, num11, acceptTopSurfaces);
            bool flag5 = ChangedUtils.SolidCollision(vector, num10, num11 - 4, acceptTopSurfaces);
            bool flag6 = !ChangedUtils.SolidCollision(vector + new Vector2(num10 * NPC.direction, 0f), 16, 80, acceptTopSurfaces);
            float jumpHeight = 8f;
            if (flag4 || flag5)
            {
                NPC.localAI[0] = 0f;
            }
            if ((num13 || closeToTarget) && flag3)
            {
                NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + num6 * 2f, 0.001f, 16f);
            }
            else if (flag4 && !flag5)
            {
                NPC.velocity.Y = 0f;
            }
            else if (flag4)
            {
                NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + num5, min, 0f);
            }
            else if (NPC.velocity.Y == 0f && flag6)
            {
                NPC.velocity.Y = 0f - jumpHeight;
                NPC.localAI[0] = 1f;
            }
            else
            {
                NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + num6, 0f - jumpHeight, 16f);
            }
        }

        private void Fly()
        {
            var player = Main.player[NPC.target];

            var currentVelocity = NPC.velocity;
            float speed = 5;
            var currentDirection = currentVelocity.SafeNormalize(Vector2.UnitY);

            var targetDistance = player.Center - NPC.Center;
            var targetDirection = targetDistance.SafeNormalize(Vector2.UnitY);
            float maxTurn = MathHelper.ToRadians(4f);

            float angleDifference = currentDirection.ToRotation().AngleTowards(targetDirection.ToRotation(), maxTurn);
            var newDirection = angleDifference.ToRotationVector2();

            newDirection = newDirection.SafeNormalize(Vector2.UnitY);
            NPC.velocity = newDirection * speed;
            if (NPC.velocity.X != 0)
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);
        }

        public override void AI()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                var npc = Main.npc[i];
                if (npc != null && npc.active && npc.type == NPCID.VortexHornet)
                    npc.active = false;
            }

            if (!NPC.HasValidTarget)
                NPC.TargetClosest();
            if (!NPC.HasValidTarget)
                NPC.active = false;

            // Force storm
            Main.cloudAlpha = Math.Min(Main.cloudAlpha + 0.02f, 0.6f);
            Main.windSpeedTarget = 1;

            bool haltMovement = false;
            switch (AIState)
            {
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.PickAttack:
                    StatePickAttack();
                    break;
                case (float)ActionState.AttackSpread:
                    StateAttackSpread();
                    break;
                case (float)ActionState.AttackOrb:
                    StateAttackOrb();
                    break;
                case (float)ActionState.AttackDown:
                    StateAttackDown();
                    break;
                case (float)ActionState.DashToPlayer:
                    TeleportToPlayer();
                    break;
                case (float)ActionState.AttackFlyHorizontal:
                    StateAttackFlyHorizontal();
                    break;
            }

            if (!SecondPhase)
            {
                if (AIState != (float)ActionState.DashToPlayer && AIState != (float)ActionState.AttackFlyHorizontal)
                    Movement(haltMovement);
            }
            else
            {
                if (AIState != (float)ActionState.DashToPlayer && AIState != (float)ActionState.AttackFlyHorizontal)
                    Fly();
            }
        }
    }
}
