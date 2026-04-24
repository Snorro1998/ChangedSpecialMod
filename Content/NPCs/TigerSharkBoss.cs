using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Projectiles.SharkBossFight;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace ChangedSpecialMod.Content.NPCs
{
    // Work in progress. Cannot be spawned normally yet
    [AutoloadBossHead]
    public class TigerSharkBoss : ModNPC
    {
        private enum ActionState
        {
            Idle,
            Attack,
            //AttackWhirlpoolHorizontal,
            AttackWhirlpoolVertical,
            AttackWhirlpoolVerticalDual,
            AttackSpawnSharks,
            AttackRain,
            AttackWall,
            AttackSharkWall,
            Fly,
            DashToPlayer
        }

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];
        public ref float AIAttack => ref NPC.ai[2];

        private int[] ProjectileOptionsFirstPhase = new int[] { };
        private int[] ProjectileOptionsSecondPhase = new int[] { };

        private bool SecondPhase { get { return NPC.life < NPC.lifeMax / 2; } }

        private int DualTornadoDistance = 640;

        private float ProjectileSpeed = 10f;
        // The damage calculation is strange, because it does way more damage (30 damage did around 50 with 15 defense!)
        private int ProjectileDamage = 15;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale * 0.7f,
                PortraitScale = 1 / NPC.scale * 0.7f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults() 
        {
            NPC.width = 18;
            NPC.height = 45;
            NPC.damage = 35;
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

            var changedNPC = NPC.Changed();

            changedNPC.HatXOffset = 0;
            changedNPC.HatYOffset = -32;
            changedNPC.RemoveAllHats();

            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.Water;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.TigerSharkBoss.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SharkFin, 5));
            npcLoot.Add(ItemDropRule.Common(ItemID.SharkBait, 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Main.NewText("This boss fight is still being developed, so don't expect much for now");
            NPC.TargetClosest(false);
            ProjectileOptionsFirstPhase = new int[] 
            { 
                ModContent.ProjectileType<GoldfishProjectile>(),
                ModContent.ProjectileType<JellyfishProjectile>(), 
                ModContent.ProjectileType<ArapaimaProjectile>()
            };

            ProjectileOptionsSecondPhase = new int[]
            {
                ModContent.ProjectileType<SharkProjectile>(),
                ModContent.ProjectileType<OrcaProjectile>()
            };
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
            CheckIfShouldDash();
            //SpawnFishes();

            if (AITimer == 1)
            {
                NPC.damage = NPC.defDamage;
                NPC.dontTakeDamage = false;
            }

            if (AITimer >= 4 * 60)
            {
                SwitchState(ActionState.Attack);
            }
        }

        private List<int> GetFishOptions(bool includeAll = false)
        {
            var options = new List<int>();
            var optionsFirst = ProjectileOptionsFirstPhase.ToList();
            var optionsSecond = ProjectileOptionsSecondPhase.ToList();
            options.AddRange(optionsFirst);

            if (SecondPhase || includeAll)
            {
                options.AddRange(optionsSecond);
            }

            return options;
        }

        private void DespawnFishes()
        {
            var options = GetFishOptions(true);

            for (int i = 0; i < Main.projectile.Length; i++)
            {
                var projectile = Main.projectile[i];
                if (projectile != null && projectile.active && options.Contains(projectile.type))
                {
                    projectile.active = false;
                }
            }
        }

        private void SpawnFishes()
        {
            var interval = 15;
            if (AITimer % interval == 0 && NPC.HasValidTarget)
            {
                var entitySource = NPC.GetSource_FromAI();
                var player = Main.player[NPC.target];

                int xPos = (int)NPC.Center.X;
                int yPos = (int)NPC.Center.Y;

                var projectileOptions = GetFishOptions().ToArray();
                var projectileType = Utils.SelectRandom(Main.rand, projectileOptions);
                var projectileId = Projectile.NewProjectile(entitySource, new Vector2(xPos, yPos), Vector2.Zero, projectileType, ProjectileDamage, 1);

                if (projectileId != -1)
                {
                    var projectile = Main.projectile[projectileId];
                    var multi = Main.rand.NextFloat(1.5f, 3.0f);
                    var spd = ProjectileSpeed * multi;
                    float gravity = 0.4f;

                    var dest = player.Bottom;
                    dest.X += Main.rand.Next(-128, 128);
                    dest.Y += Main.rand.Next(-32, 128);
                    Vector2 toTarget = dest - projectile.Center;

                    // Estimate time to reach target based on horizontal distance
                    float time = toTarget.Length() / spd;

                    // Compensate for gravity (raise the aim)
                    float gravityOffset = 0.5f * gravity * time * time;
                    toTarget.Y -= gravityOffset;

                    Vector2 direction = Vector2.Normalize(toTarget);
                    projectile.velocity = direction * spd;
                }
            }
        }

        private void StateSpawnSharks()
        {
            AITimer++;
            CheckIfShouldDash();
            var interval = (int)GetValueBasedOnHealth(60, 30);

            var tornados = Main.projectile.Where(x => x.active && x.type == ProjectileID.Sharknado).ToList();
            if (!tornados.Any())
            {
                SwitchState (ActionState.Idle);
                return;
            }

            if (NPC.HasValidTarget)
            {
                if (AITimer % interval == 0)
                {
                    var amount = 1;// SecondPhase ? 2 : 1;
                    for (int i = 0; i < amount; i++)
                    {
                        var entitySource = NPC.GetSource_FromAI();
                        var player = Main.player[NPC.target];
                        var tornadoIndex = Main.rand.Next(0, tornados.Count);
                        var tornado = tornados[tornadoIndex];

                        int xPos = (int)tornado.Center.X;
                        int yPos = (int)tornado.Center.Y;

                        var projectileOptions = GetFishOptions().ToArray();
                        var projectileType = Utils.SelectRandom(Main.rand, projectileOptions);
                        var projectileId = Projectile.NewProjectile(entitySource, new Vector2(xPos, yPos), Vector2.Zero, projectileType, ProjectileDamage, 1);

                        if (projectileId != -1)
                        {
                            var projectile = Main.projectile[projectileId];
                            var spd = ProjectileSpeed + 5 - 5 * (NPC.life / NPC.lifeMax);
                            float gravity = 0.4f;

                            Vector2 toTarget = player.Bottom - projectile.Center;

                            // Estimate time to reach target based on horizontal distance
                            float time = toTarget.Length() / spd;

                            // Compensate for gravity (raise the aim)
                            float gravityOffset = 0.5f * gravity * time * time;
                            toTarget.Y -= gravityOffset;

                            Vector2 direction = Vector2.Normalize(toTarget);
                            projectile.velocity = direction * spd;
                        }
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.alpha = 0;
            NPC.color = Color.Transparent;

            if (AIState == (float)ActionState.AttackWall)
            {
                NPC.alpha = 200;
                NPC.color = Color.LightBlue;
            }

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }



        // Spawns a whirlpool in segments
        private void SpawnWhirlpool(int xPos, float timeLeftMultiplier = 1.0f)
        {
            var interval = 5;
            var sectionHeight = 32;

            if ((int)AITimer % interval == 0)
            {
                var entitySource = NPC.GetSource_FromAI();
                int currentSection = (int)AITimer / interval;
                int yPos = (int)NPC.Center.Y - currentSection * sectionHeight;
                var projectileId = Projectile.NewProjectile(entitySource, new Vector2(xPos, yPos), Vector2.Zero, ProjectileID.Sharknado, 0, 0, -1, 0f, 0f);
                if (projectileId != -1)
                {
                    var projectile = Main.projectile[projectileId];
                    projectile.friendly = false;
                    projectile.damage = ProjectileDamage;
                    projectile.timeLeft = (int)(projectile.timeLeft * timeLeftMultiplier);
                }
            }
        }

        #region Attacks

        // Pick the next attack
        private void StatePickAttack()
        {
            AITimer++;

            if (AITimer == 1)
            {
                //SwitchState(ActionState.Fly);
                //return;

                // Despawn any flying fishes from previous attacks
                DespawnFishes();

                NPC.TargetClosest(false);
                if (NPC.HasValidTarget)
                {
                    var entitySource = NPC.GetSource_FromAI();
                    var attack = AIAttack;
                    int xPos = (int)NPC.Center.X;
                    int yPos = (int)NPC.Center.Y;

                    if (AIAttack == 0)
                    {
                        SwitchState(ActionState.AttackSharkWall);
                        AIAttack++;
                    }
                    else if (AIAttack == 1)
                    {
                        SwitchState(ActionState.AttackWhirlpoolVertical);
                        AIAttack++;
                    }
                    else
                    {
                        SwitchState(ActionState.AttackWall);
                        AIAttack = 0;
                    }
                    /*
                    else
                    {
                        SwitchState(ActionState.Fly);
                        AIAttack = 0;
                    }*/
                    /*

                    */
                }
            }

            // Switch back to idle state in case the attack failed to start
            else if (AITimer >= 2 * 60)
            {
                SwitchState(ActionState.Idle);
            }
        }

        // Summons flying sharks in a horizontal line
        private void StateAttackSharkWall()
        {
            AITimer++;

            if (AITimer == 1 && NPC.HasValidTarget)
            {
                var player = Main.player[NPC.target];

                // Shark height
                var projectileHeight = 64;
                var amount = 50;
                var height = projectileHeight * amount;
                var yPos = (float)(player.Center.Y - 0.5f * height);

                var width = 60 * 16;
                var tmpXPos = player.Center.X;
                var xPosLeft = (float)(tmpXPos - width);
                var xPosRight = (float)(tmpXPos + width);
                var nOpening = 2;

                var projectileType = ModContent.ProjectileType<SharkProjectile>();
                var entitySource = NPC.GetSource_FromAI();
                var speed = GetValueBasedOnHealth(10, 20);

                for (int i = 0; i < amount; i++)
                {
                    var xPos = xPosLeft;
                    var velocity = new Vector2(speed, 0);
                    if (i % (nOpening * 2) < nOpening)
                    {
                        xPos = xPosRight;
                        velocity.X *= -1;
                    }

                    var projectileId = Projectile.NewProjectile(entitySource, new Vector2(xPos, yPos + i * projectileHeight), velocity, projectileType, ProjectileDamage, 1, -1, 0, 0, 1);
                }
            }

            if (AITimer > 4 * 60)
            {
                SwitchState(ActionState.Idle);
                return;
            }
        }

        public Vector2 RotateRadians(Vector2 v, double radians)
        {
            var ca = Math.Cos(radians);
            var sa = Math.Sin(radians);
            return new Vector2((float)(ca * v.X - sa * v.Y), (float)(sa * v.X + ca * v.Y));
        }

        private void StateAttackWhirlpoolVertical()
        {
            AITimer++;
            var interval = 5;
            var nSections = 25;

            // If the full whirlpool has been created, move on to the state where sharks can be spawned from it
            if (AITimer >= nSections * interval)
            {
                SwitchState(ActionState.AttackSpawnSharks);
                return;
            }

            var xOffset = -NPC.spriteDirection * 96;
            if (!SecondPhase)
            {
                xOffset = -NPC.spriteDirection * 96;
                SpawnWhirlpool((int)NPC.Center.X + xOffset);
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    xOffset = i == 0 ? -DualTornadoDistance : DualTornadoDistance;
                    SpawnWhirlpool((int)NPC.Center.X + xOffset);
                }
            }
        }

        // Summons water streams with openings moving down
        private void StateAttackWall()
        {
            AITimer++;
            var nSections = 16;
            var sectionWidth = 96;
            var width = nSections * sectionWidth;

            var distFromGround = 480;
            var distBetweenWalls = 520;
            var nWalls = 6;
            var wallSpeed = GetValueBasedOnHealth(2.5f, 4.5f);
            var timeLeftMultiplier = GetValueBasedOnHealth(1.8f, 1.0f);

            NPC.damage = 0;
            NPC.dontTakeDamage = true;

            if (AITimer == 1)
            {
                var entitySource = NPC.GetSource_FromAI();
                for (int j = 0; j < nWalls; j++)
                {
                    var opening = Main.rand.Next(4, nSections - 4);
                    for (int i = 0; i < nSections; i++)
                    {
                        if (i == opening || i == opening + 1)
                            continue;
                        int currentSection = i;
                        int xPos = (int)(NPC.Center.X + currentSection * sectionWidth - 0.5f * width);
                        int yPos = (int)NPC.Center.Y - distFromGround - j * distBetweenWalls;
                        var projectileId = Projectile.NewProjectile(entitySource, new Vector2(xPos, yPos), Vector2.Zero, ProjectileID.Sharknado, 0, 0, -1, 0f, 0f);
                        if (projectileId != -1)
                        {
                            var projectile = Main.projectile[projectileId];
                            projectile.friendly = false;
                            projectile.damage = ProjectileDamage;
                            projectile.velocity = new Vector2(0, wallSpeed);
                            projectile.timeLeft = (int)(projectile.timeLeft * timeLeftMultiplier);
                        }
                    }
                }
            }

            var interval = 5;

            if (AITimer < interval * nSections && AITimer % interval == 0)
            {
                var leftPos = (int)(NPC.Center.X - 0.5f * width);
                var rightPos = (int)(NPC.Center.X + 0.5f * width);

                SpawnWhirlpool(leftPos, timeLeftMultiplier);
                SpawnWhirlpool(rightPos, timeLeftMultiplier);
            }

            var tornados = Main.projectile.Where(x => x.active && x.type == ProjectileID.Sharknado).ToList();
            if (!tornados.Any())
            {
                SwitchState(ActionState.Idle);
                return;
            }
        }

        private void StateAttackSharkSpiral()
        {
            AITimer++;

            var projectileType = ModContent.ProjectileType<JellyfishProjectile>();
            var entitySource = NPC.GetSource_FromAI();

            int spawnInterval = 10;//6
            float rotationSpeed = 0.15f;
            float speed = GetValueBasedOnHealth(6f, 10f);

            if (AITimer % spawnInterval == 0)
            {
                float angle = AITimer * rotationSpeed;

                int nArms = 3; //4
                for (int i = 0; i < nArms; i++)
                {
                    float offset = MathHelper.TwoPi / nArms * i;
                    Vector2 velocity = new Vector2((float)Math.Cos(angle + offset), (float)Math.Sin(angle + offset)) * speed;

                    Projectile.NewProjectile(entitySource, NPC.Center, velocity, projectileType, ProjectileDamage, 1, -1, 0, 0, 1);
                }
            }

            if (AITimer > 5 * 60)
            {
                SwitchState(ActionState.Idle);
            }
        }

        private void StateAttackWhirlpoolBurst()
        {
            AITimer++;

            var player = Main.player[NPC.target];

            // Pull player toward NPC
            float pullStrength = 0.3f;
            Vector2 direction = NPC.Center - player.Center;
            //player.velocity += Vector2.Normalize(direction) * pullStrength;

            if (AITimer == 1)
            {
                var entitySource = NPC.GetSource_FromAI();
                var velocity = Vector2.Zero;
                var projectileType = ModContent.ProjectileType<WaterProjectile>();
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, projectileType, ProjectileDamage, 1, -1, 0, 0, 0);
            }

            // Burst
            if (AITimer == 90)
            {
                var entitySource = NPC.GetSource_FromAI();
                var projectileType = ModContent.ProjectileType<SharkProjectile>();

                int count = 16;
                float speed = GetValueBasedOnHealth(8f, 14f);

                for (int i = 0; i < count; i++)
                {
                    float angle = MathHelper.TwoPi * i / count;
                    Vector2 velocity = angle.ToRotationVector2() * speed;
                    velocity *= 2;

                    Projectile.NewProjectile(entitySource, NPC.Center, velocity, projectileType, ProjectileDamage, 1, -1, 0, 0, 0);
                }
            }

            if (AITimer > 120)
            {
                SwitchState(ActionState.Idle);
            }
        }

        private void StateAttackDashTrail()
        {
            AITimer++;

            var player = Main.player[NPC.target];

            if (AITimer == 1)
            {
                Vector2 direction = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                NPC.velocity = direction * 20f;
            }

            // Spawn trail
            if (AITimer % 5 == 0)
            {
                var entitySource = NPC.GetSource_FromAI();
                var projectileType = ModContent.ProjectileType<SharkProjectile>();

                Projectile.NewProjectile(entitySource, NPC.Center, Vector2.Zero, projectileType, ProjectileDamage, 1, -1, 0, 0, 0);
            }

            // Slow down
            NPC.velocity *= 0.98f;

            if (AITimer > 60)
            {
                NPC.velocity = Vector2.Zero;
                SwitchState(ActionState.Idle);
            }
        }

        #endregion

        // Dash towards player if trying to escape
        private void StateDashToPlayer()
        {
            AITimer++;
            if (!NPC.HasValidTarget)
            {
                SwitchState(ActionState.Idle);
                return;
            }

            var player = Main.player[NPC.target];
            var direction = Vector2.Normalize(player.Center - NPC.Center);

            if (AITimer == 1)
            {
                SoundEngine.PlaySound(Sounds.SoundMonster, player.Center);
            }

            NPC.velocity = 20 * direction;

            var minDist = 16 * 10;
            if (Vector2.DistanceSquared(player.Center, NPC.Center) < minDist * minDist)
            {
                SwitchState(ActionState.Fly);
                return;
            }
        }

        private void StateFly()
        {
            AITimer++;
            CheckIfShouldDash();

            if (NPC.HasValidTarget)
            {
                var player = Main.player[NPC.target];
                float speed = 5;

                var currentDirection = Vector2.Normalize(NPC.velocity);
                var targetDirection = Vector2.Normalize(player.Center - NPC.Center);

                // Max radians to rotate per update
                float maxTurn = MathHelper.ToRadians(4f);

                float angleDifference = currentDirection.ToRotation().AngleTowards(targetDirection.ToRotation(), maxTurn);
                var newDirection = angleDifference.ToRotationVector2();

                // Re-normalize to preserve direction accuracy
                newDirection.SafeNormalize(Vector2.Zero);
                NPC.velocity = newDirection * speed;
            }

            if (AITimer > 5 * 60)
            {
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

        public override void AI()
        {
            bool haltMovement = false;
            switch (AIState)
            {
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.Attack:
                    haltMovement = true;
                    StatePickAttack();
                    break;
                case (float)ActionState.AttackWhirlpoolVertical:
                    //StateAttackWhirlpoolBurst();
                    haltMovement = true;
                    StateAttackWhirlpoolVertical();
                    break;
                case (float)ActionState.AttackSpawnSharks:
                    StateSpawnSharks();
                    break;
                case (float)ActionState.AttackWall:
                    haltMovement = true;
                    StateAttackWall();
                    break;
                case (float)ActionState.AttackSharkWall:
                    //StateAttackSharkSpiral();
                    //StateAttackDashTrail();
                    StateAttackSharkWall();
                    break;
                case (float)ActionState.Fly:
                    StateFly();
                    break;
                case (float)ActionState.DashToPlayer:
                    StateDashToPlayer();
                    break;
            }

            if (AIState != (float)ActionState.DashToPlayer
                && AIState != (float)ActionState.Fly)
                Movement(haltMovement);
        }
    }
}
