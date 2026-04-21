using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    [AutoloadBossHead]
    public class BehemothHand : ModNPC
	{
        private enum ActionState
        {
            Idle,
            Grow,
            Shrink,

            // Attacks
            RubbleAttack,
            Dash = 4,
            Slam
        }

        // We are gonna do it just like GameMaker, but we have to do it all ourselves
        public double imageSpeed = 5D;
        public int imageIndex = 0;

        public int[] animation = new int[] { 0, 1, 2, 1 };

        public int[] AnimIdle = new int[] { 0, 1, 2, 1 };
        public int[] AnimDash = new int[] { 0 };
        public int[] AnimSlam = new int[] { 3 };
        public int[] AnimRubble = new int[] { 4, 5, 6, 7 };
        public int[] AnimGrow = new int[] { 11, 10, 9, 8};
        public int[] AnimShrink = new int[] { 8, 9, 10, 11};

        public int ImageLength {get { return animation.Length; }}
        public bool Loop = true;

        public double imageCounter = 0D;

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];

        public int npcDirection = 1;
        public float shrinkTime = 2f;

        private int projectileDamage = 15;

        // Dash State
        // Idle time before performing the attack
        private int fiddleHandsTime = 120;
        // Duration of moving back before dashing towards the player
        private int backupTime = 15;

        public int maxFollowDistance = 300 * 16;

        public double CalcAnimationDuration()
        {
            return animation.Length / imageSpeed * 60;
        }

        public override void SetStaticDefaults() 
        {
            Main.npcFrameCount[Type] = 12;
            NPCID.Sets.DoesntDespawnToInactivityAndCountsNPCSlots[NPC.type] = true;
            ChangedUtils.HideFromBestiary(this);
        }

		public override void SetDefaults() 
        {
            NPC.width = 72;
            NPC.height = 72;
            NPC.aiStyle = -1;
            NPC.damage = 25;
            NPC.defense = 10;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(0, 5);
            NPC.npcSlots = 10f;
            NPC.dontTakeDamage = true;
            AnimationType = NPCID.None;
            NPC.scale = 0.75f;

            NPC.boss = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.GooType = GooType.White;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.HitEffectScale = 3;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SwitchState(ActionState.Grow, AnimGrow, false);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Orange>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void FindFrame(int frameHeight)
        {
            imageCounter += imageSpeed;
            if (imageCounter >= ImageLength * 60)
            {
                if (Loop)
                {
                    imageCounter %= ImageLength * 60;
                }
                else
                {
                    imageCounter = ImageLength * 60 - 1;
                }
            }

            var arrayIndex = (int)(imageCounter / 60D);
            imageIndex = animation[arrayIndex];
            NPC.frame.Y = imageIndex * frameHeight;
        }

        private void SwitchAnimation(int[] newAnimation)
        {
            imageCounter = 0;
            animation = newAnimation;
        }

        private void SwitchState(ActionState newState, int[] nextAnimation = null, bool? nextLoop = null)
        {
            AIState = (float)newState;
            AITimer = 0;
            NPC.netUpdate = true;

            if (nextLoop != null)
                Loop = (bool)nextLoop;
            if (nextAnimation != null)
                SwitchAnimation(nextAnimation);
        }

        private void StateGrow()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(AnimGrow);
                Loop = false;
                SoundEngine.PlaySound(Sounds.SoundTransfur, NPC.Center);
            }

            if (AITimer >= 60)
            {
                SwitchState(ActionState.Idle, AnimIdle, true);
            }
        }

        private void StateShrink()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(AnimShrink);
                Loop = false;
                SoundEngine.PlaySound(Sounds.SoundTransfur, NPC.Center);
            }

            if (AITimer >= 60)
            {
                MoveToRandomPosition();
                SwitchState(ActionState.Grow, AnimGrow, false);
            }
        }

        private void MoveToRandomPosition()
        {
            NPC.TargetClosest(false);

            if (NPC.HasValidTarget)
            {
                var player = Main.player[NPC.target];

                int xPos = (int)(player.Center.X / 16f) + npcDirection * Main.rand.Next(10, 20);
                int yPos = (int)(player.Center.Y / 16f) - 10;

                for (int y= yPos; y < yPos + 30; y++)
                {
                    if (WorldGen.SolidTile(xPos, y))
                    {
                        NPC.position = new Vector2(xPos * 16 - 0.5f * NPC.width, y * 16 -  NPC.height);
                        NPC.direction = player.Center.X < NPC.Center.X ? -1 : 1;
                        NPC.spriteDirection = NPC.direction;
                        break;
                    }
                }
            }
        }

        // This started out as a copy of the deerclops AI, so there is still some junk left
        public override void AI()
        {
            bool isBackingUpForDashAttack = AIState == (float)ActionState.Dash && AITimer < fiddleHandsTime + backupTime;

            // Update sprite direction if moving and not preparing to dash
            if (!isBackingUpForDashAttack && NPC.velocity.X != 0)
            {
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);
            }

            NPCAimedTarget targetData = NPC.GetTargetData();
            bool haltMovement = false;
            bool goHome = false;
            // Invulnarable if distance is greater then this
            bool flag = NPC.Distance(targetData.Center) >= 450f;
            NPC.localAI[3] = MathHelper.Clamp(NPC.localAI[3] + (float)flag.ToDirectionInt(), 0f, 30f);
            float lifePercent = (float)NPC.life / (float)NPC.lifeMax;

            if (NPC.homeTileX == -1 && NPC.homeTileY == -1)
            {
                Point point = NPC.Bottom.ToTileCoordinates();
                NPC.homeTileX = point.X;
                NPC.homeTileY = point.Y;
                NPC.ai[2] = NPC.homeTileX;
                NPC.ai[3] = NPC.homeTileY;
                NPC.netUpdate = true;
                NPC.timeLeft = 86400;
            }
            NPC.timeLeft -= Main.worldEventUpdates;
            if (NPC.timeLeft < 0)
            {
                NPC.timeLeft = 0;
            }

            switch ((int)AIState)
            {
                default:
                    StateIdle();
                    break;
                case (int)ActionState.RubbleAttack:
                    StateRubbleAttack(ref haltMovement, ref targetData);
                    break;
                case (int)ActionState.Dash:
                    StateDash(false);
                    break;
                case (int)ActionState.Slam:
                    StateSlam();
                    break;
                case (int)ActionState.Grow:
                    StateGrow();
                    break;
                case (int)ActionState.Shrink:
                    StateShrink();
                    break;
            }

            if ((int)AIState != (int)ActionState.Dash && (int)AIState != (int)ActionState.Slam)
                Movement(haltMovement, goHome);
        }

        private void Attack_ShootRubbleUp(ref NPCAimedTarget targetData, ref Point sourceTileCoords, int howMany, int distancedByThisManyTiles, float upBiasPerSpike, int whichOne)
        {
            int num2 = whichOne * distancedByThisManyTiles;
            int nProjectiles = 20;//35
            for (int i = 0; i < nProjectiles; i++)
            {
                int num3 = sourceTileCoords.X + num2 * NPC.direction;
                int num4 = sourceTileCoords.Y + i;

                if (WorldGen.SolidTileAllowTopSlope(num3, num4))
                {
                    Vector2 vector = targetData.Center + new Vector2(num2 * NPC.direction * 20, (0f - upBiasPerSpike) * (float)howMany + (float)num2 * upBiasPerSpike / (float)distancedByThisManyTiles);
                    Vector2 vector2 = new Vector2(num3 * 16 + 8, num4 * 16 + 8);
                    Vector2 vector3 = (vector - vector2).SafeNormalize(-Vector2.UnitY);
                    vector3 = new Vector2(0f, -1f).RotatedBy((float)(whichOne * NPC.direction) * 0.7f * ((float)Math.PI / 4f / (float)howMany));
                    int num5 = Main.rand.Next(Main.projFrames[962] * 4);
                    num5 = 6 + Main.rand.Next(6);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(num3 * 16 + 8, num4 * 16 - 8), vector3 * (8f + Main.rand.NextFloat() * 8f), ModContent.ProjectileType<WhiteLatexProjectile>(), projectileDamage, 0f, Main.myPlayer, 0f, num5);
                    break;
                }
            }
        }

        public bool IsOnSolidTile()
        {
            if (NPC.velocity.Y > 0f)
            {
                int yTile = (int)(NPC.position.Y + (float)NPC.height + 7f) / 16;
                int initialXTile = (int)NPC.position.X / 16;
                int maxXTile = (int)(NPC.position.X + (float)NPC.width) / 16;
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
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            NPC.TargetClosest(false);
            var entitySource = NPC.GetSource_FromAI();

            float hpAmount = 1;
            var behemoth = Main.npc.FirstOrDefault(x => x.active && x.type == ModContent.NPCType<Behemoth>());
            if (behemoth != null)
                hpAmount = (float)behemoth.life / (float)behemoth.lifeMax;

            int count = 20;
            int projWidth = 32;
            int spacing = (int)(96 + 96 * hpAmount);

            var totalWidth = count * (projWidth + spacing); 
            var num5 = 6 + Main.rand.Next(6);
            Rectangle targetRectangle = NPC.GetTargetData().Hitbox;
            var yPos = NPC.GetTargetData().Center.Y - 16 * 40;
            float targetX = (float)targetRectangle.Center.X;
            
            for (int i = 0; i < count; i++)
            {
                var xPos = (int)(targetX - 0.5 * totalWidth + i * (projWidth + spacing));
                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(xPos, yPos), new Vector2(0, 1) * (8f + Main.rand.NextFloat() * 8f), ModContent.ProjectileType<WhiteLatexProjectile>(), projectileDamage, 0f, Main.myPlayer, 0f, num5);
            }
        }

        private void StateIdle()
        {
            NPC.TargetClosest();
            var targetData = NPC.GetTargetData();

            AITimer += 1f;

            if (AITimer == 1)
            {
                SwitchAnimation(AnimIdle);
                Loop = true;
            }

            Vector2 vector = NPC.Bottom + new Vector2(0f, -32f);
            Vector2 vector2 = targetData.Hitbox.ClosestPointInRect(vector);
            Vector2 distanceVectorToPlayer = vector2 - vector;
            (vector2 - NPC.Center).Length();
            float yDistanceMultiplier = 0.6f;
            bool flag4 = Math.Abs(distanceVectorToPlayer.X) >= Math.Abs(distanceVectorToPlayer.Y) * yDistanceMultiplier || distanceVectorToPlayer.Length() < 48f;
            bool flag5 = distanceVectorToPlayer.Y <= (float)(100 + targetData.Height) && distanceVectorToPlayer.Y >= -200f;
            bool grounded = NPC.velocity.Y == 0f;

            bool farAway = Math.Abs(distanceVectorToPlayer.X) > 35 * 16;
            bool veryFarAway = Math.Abs(distanceVectorToPlayer.X) > maxFollowDistance;

            float hpAmount = 1;
            var behemoth = Main.npc.FirstOrDefault(x => x.active && x.type == ModContent.NPCType<Behemoth>());
            if (behemoth != null)
                hpAmount = (float)behemoth.life / (float)behemoth.lifeMax;

            // If moving, grounded and 4 seconds have passed
            bool timePassed = AITimer >= 120 + hpAmount * 120;

            if (!NPC.HasValidTarget)
                NPC.TargetClosest();
            if (!NPC.HasValidTarget)
                return;

            var behemothDistance = float.MaxValue;
            if (behemoth != null)
            {
                var player = Main.player[NPC.target];
                behemothDistance = Vector2.DistanceSquared(behemoth.Center, player.Center);
            }

            // This is stupid but it works most of the times
            // Measure how far the behemoth is from the player and only let him (the hand) teleport if close
            if ((veryFarAway || (farAway && timePassed)) && behemothDistance < maxFollowDistance * maxFollowDistance)
            {
                SwitchState(ActionState.Shrink, AnimShrink, false);
                NPC.localAI[1] = 0f;
            }
            
            else if (grounded /*&& NPC.velocity.X != 0f*/ && timePassed)
            {
                NPC.TargetClosest();
                if (!NPC.HasValidTarget)
                    return;
                var previousAttacks = (int)NPC.ai[2];
                var attackSlamDone = (previousAttacks & 1) == 1;
                var attackRubbleDone = (previousAttacks & 2) == 2;
                var attackDashDone = (previousAttacks & 4) == 4;

                List<ActionState> attackOptions = new List<ActionState>();
                if (!attackSlamDone) attackOptions.Add(ActionState.Slam);
                if (!attackRubbleDone) attackOptions.Add(ActionState.RubbleAttack);
                if (!attackDashDone) attackOptions.Add(ActionState.Dash);

                if (!attackOptions.Any())
                {
                    NPC.ai[2] = 0;
                    attackOptions.Add(ActionState.Slam);
                    attackOptions.Add(ActionState.RubbleAttack);
                    attackOptions.Add(ActionState.Dash);
                }

                var numb = ChangedUtils.MainRandNext(0, attackOptions.Count);
                var attackType = attackOptions[numb];

                if (attackType == ActionState.Slam) NPC.ai[2] += 1;
                if (attackType == ActionState.RubbleAttack) NPC.ai[2] += 2;
                if (attackType == ActionState.Dash) NPC.ai[2] += 4;

                var otherHand = Main.npc.Where(x => x.active && x.type == Type && x.whoAmI != NPC.whoAmI).FirstOrDefault();
                // Delay attack of other hand if idle
                if (otherHand != null && otherHand.ai[0] == 0)
                {
                    otherHand.ai[1] = 60;
                }

                NPC.velocity.X = 0f;
                SwitchState(attackType);
                NPC.localAI[1] = 0f;
            }
        }

        private void StateSlam()
        {
            AITimer += 1f;

            // Start
            if (AITimer == 1)
            {
                SwitchAnimation(AnimSlam);
                NPC.velocity.X = 0f;
                NPC.velocity.Y = -5f;
            }

            // Hit the floor or after 5 seconds just in case
            if (IsOnSolidTile() || AITimer > 5 * 60)
            {
                PunchCameraModifier modifier4 = new PunchCameraModifier(NPC.Center, new Vector2(0f, -1f), 20f, 6f, 30, 1000f, "Deerclops");
                Main.instance.CameraModifiers.Add(modifier4);
                //ScreenShakeSystem.Shake(0.5f, 6f);
                SoundEngine.PlaySound(Sounds.SoundSlam, NPC.Center);
                SpawnBlobs();

                SwitchState(ActionState.Idle, AnimIdle, true);
            }

            NPC.velocity.Y += 0.3f;
        }

        private void StateDash(bool haltMovement)
        {
            AITimer += 1f;
            int dashSpeed = 20;
            int backupTime = 15;

            if (AITimer == 1f)
            {
                imageSpeed = 10;
                SwitchAnimation(AnimIdle);
            }

            if (AITimer < fiddleHandsTime)
            {
                return;
            }

            float num = (float)NPC.life / (float)NPC.lifeMax;
            float num2 = 1f - num;
            float num3 = 3.5f + 1f * num2;

            Rectangle targetRectangle = NPC.GetTargetData().Hitbox;

            float num7 = (float)targetRectangle.Center.X - NPC.Center.X;
            float xDistanceToTarget = Math.Abs(num7);

            bool flag = xDistanceToTarget < 80f;
            bool stopMoving = flag || haltMovement;
            
            int num9 = Math.Sign(num7);
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

            // Prepare lunge
            if (AITimer == fiddleHandsTime)
            {
                NPC.TargetClosest();
                SwitchAnimation(AnimDash);
                var pos = vector;
                var targetPos = new Vector2(targetRectangle.Center.X, targetRectangle.Center.Y);
                var moveVector = targetPos - pos;
                moveVector.Normalize();
                moveVector *= dashSpeed;
                moveVector *= -0.5f;
                NPC.velocity = moveVector;
            }

            // Lunge towards player position
            else if (AITimer == fiddleHandsTime + backupTime)
            {
                NPC.TargetClosest();
                SwitchAnimation(AnimDash);
                NPC.damage = (int)(NPC.defDamage * 1.5f);
                var pos = vector;
                var targetPos = new Vector2(targetRectangle.Center.X, targetRectangle.Center.Y);
                var moveVector = targetPos - pos;
                moveVector.Normalize();
                moveVector *= dashSpeed;
                NPC.velocity = moveVector;
            }

            if (flag4 || flag5)
            {
                NPC.localAI[0] = 0f;
            }

            if (AITimer >= fiddleHandsTime + backupTime + 60f)
            {
                NPC.damage = NPC.defDamage;
                imageSpeed = 5;
                SwitchState(ActionState.Idle, AnimIdle, true);
            }
        }

        private void Movement(bool haltMovement, bool goHome)
        {
            goHome = false;

            float hpAmount = 1;
            var behemoth = Main.npc.FirstOrDefault(x => x.active && x.type == ModContent.NPCType<Behemoth>());
            if (behemoth != null)
                hpAmount = (float)behemoth.life / (float)behemoth.lifeMax;

            float num2 = 1f - hpAmount;
            float num3 = 2f + 1f * num2;
            float num4 = 2.5f;
            float num5 = -0.4f;
            float min = -8f;
            float num6 = 0.4f;
            Rectangle targetRectangle = NPC.GetTargetData().Hitbox;
            if (goHome)
            {
                targetRectangle = new Rectangle(NPC.homeTileX * 16, NPC.homeTileY * 16, 16, 16);
                if (NPC.Distance(targetRectangle.Center.ToVector2()) < 240f)
                {
                    targetRectangle.X = (int)(NPC.Center.X + (float)(160 * NPC.direction));
                }
            }
            float num7 = (float)targetRectangle.Center.X - NPC.Center.X;
            float xDistanceToTarget = Math.Abs(num7);
            if (goHome && num7 != 0f)
            {
                NPC.direction = (NPC.spriteDirection = Math.Sign(num7));
            }
            bool flag = xDistanceToTarget < 16f;
            bool stopMoving = flag || haltMovement;

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
                int num9 = Math.Sign(num7);
                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, (float)num9 * num3, 1f / num4);
            }
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
            if ((num13 || flag) && flag3)
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

        private void StateRubbleAttack(ref bool haltMovement, ref NPCAimedTarget targetData)
        {
            int attackDelay = 60;
            int num9 = 8 * 4 + attackDelay;
            AITimer += 1f;

            if (AITimer == attackDelay)
            {
                SwitchAnimation(AnimRubble);
                Loop = false;
            }

            if (AITimer == (float)num9)
            {
                SoundEngine.PlaySound(in SoundID.DeerclopsRubbleAttack, NPC.Center);
            }
            haltMovement = true;
            if (Main.netMode != NetmodeID.MultiplayerClient && AITimer >= (float)num9)
            {
                Point sourceTileCoords = NPC.Top.ToTileCoordinates();

                float hpAmount = 1;
                var behemoth = Main.npc.FirstOrDefault(x => x.active && x.type == ModContent.NPCType<Behemoth>());
                if (behemoth != null)
                    hpAmount = (float)behemoth.life / (float)behemoth.lifeMax;

                float num = (float)NPC.life / (float)NPC.lifeMax;
                float num2 = 1f - hpAmount;
                int nProjectiles = (int)(5 + 10f * num2);//3.5f + 1f * num2;

                //int nProjectiles = 10;
                int distancedByThisManyTiles = 1;
                float upBiasPerSpike = 200f;
                sourceTileCoords.X += NPC.direction * 3;
                sourceTileCoords.Y -= 10;
                int num11 = (int)AITimer - num9;
                if (num11 == 0)
                {
                    PunchCameraModifier modifier4 = new PunchCameraModifier(NPC.Center, new Vector2(0f, -1f), 20f, 6f, 30, 1000f, "Deerclops");
                    Main.instance.CameraModifiers.Add(modifier4);
                }
                int num12 = 1;
                int num13 = num11 / num12 * num12;
                int num14 = num13 + num12;
                if (num11 % num12 != 0)
                {
                    num14 = num13;
                }
                for (int j = num13; j < num14 && j < nProjectiles; j++)
                {
                    Attack_ShootRubbleUp(ref targetData, ref sourceTileCoords, nProjectiles, distancedByThisManyTiles, upBiasPerSpike, j);
                }
            }
            if (AITimer >= 60f + attackDelay)
            {
                SwitchState(ActionState.Idle, AnimIdle, true);
            }
        }
    }
}
