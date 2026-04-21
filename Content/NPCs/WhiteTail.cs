using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.IO;
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
    [AutoloadBossHead]
    public class WhiteTail : ModNPC
	{
        public enum ActionState
        {
            Walk,       // Chasing player or wandering
            Door,       // Banging the door
            Shrink,     // Begin teleport to player
            Grow        // Teleport to player
        }

        float maxRunVelocity = 8f; //3f
        float maxWanderVelocity = 2f;

        public float knockDelay = 60f;
        //backuptimer = 60 = 1s
        public float wanderTime = 5f;

        public int maxFollowDistance = 300 * 16;


        // AI Values
        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];
        public ref float AIDoorKnockValue => ref NPC.ai[2];
        public ref float AIImpatienceTimer => ref NPC.ai[3];

        public int doorCheckX = 0; 
        public int doorCheckY = 0;
        public int backUpTimer = 60;
        float accel = 0.08f;

        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale,
                PortraitScale = 1 / NPC.scale
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
            NPC.width = 36;
            NPC.height = 32;
            NPC.damage = 20; //25
            NPC.defense = 10;
            NPC.lifeMax = 2000;
            NPC.HitSound = SoundID.NPCHit1; //SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = ChangedUtils.GetNPCValue(gold: 3);
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = -1;
            NPC.npcSlots = 3;
            AIType = -1;
            AnimationType = NPCID.Zombie;
            NPC.boss = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type };

            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.GooType = GooType.None;
            ChangedGlobalNPC.ElementType = ElementType.None;
            ChangedGlobalNPC.DefaultOnHitPlayer = true;
            ChangedGlobalNPC.DefaultHitEffect = true;
        }

        
        public override void OnSpawn(IEntitySource source)
        {
            if (ChangedUtils.AnnounceBoss)
            {
                var npcName = Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.WhiteTail.DisplayName");
                Main.NewText(Language.GetTextValue("Announcement.HasAwoken", npcName), new Color(175, 75, 255));
                //Main.NewText("White Tail has awoken!", 255, 0, 255);
            }

            ChangedUtils.AnnounceBoss = true;
            NPC.TargetClosest();
        }

        public override void OnKill()
        {
            if (!DownedBossSystem.DownedWhiteTail)
            {
                var msg = Language.GetTextValue("Mods.ChangedSpecialMod.BossMessages.WolfKingCanSpawn");
                Main.NewText(msg, byte.MaxValue, 240, 20);
            }

            base.OnKill();
            DownedBossSystem.DownedWhiteTail = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Orange>(), 20));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.WhiteTail.Description")),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (DownedBossSystem.DownedWhiteTail || NPC.AnyNPCs(ModContent.NPCType<WhiteTail>()))
                return 0;

            var nMinKills = 20;     // Minimum monster kills needed to spawn
            var nMaxKills = 40;     // Guaranteed to spawn

            // Black
            var nGoop = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<BlackGoop>());
            var nCub = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<DarkLatexCub>());
            var nAdult = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<MaleDarkLatex>());
            nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FemaleDarkLatex>());
            var nFlying = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FlyingDarkLatex>());

            // White
            nGoop += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WhiteGoop>());
            nCub += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WhiteLatexCub>());
            nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WhiteKnight>());

            // Drunk
            if (ChangedUtils.IsDrunk(spawnInfo.Player))
            {
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<BackLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<DarkLatexCubOfDoom>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<PuroWormHead>());
                nCub += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<QuackLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<SnackLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<StackLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WackLatex>());

                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<BrightLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FightLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FlightLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<HideLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<MightLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<SideLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WideLatex>());
            }

            // Increase the change even further if the player has more then 200 hp
            var playerHP = spawnInfo.Player.statLifeMax2;// Math.Max(0, spawnInfo.Player.statLifeMax2 - 100);
            var playerHPMultiplier = Math.Max(0, playerHP - 200) / 200 * nMinKills;

            var nKills = 0.3f * nGoop + 0.5f * nCub + nAdult + 1.5f * nFlying + playerHPMultiplier;
            
            if (nKills < nMinKills)
            {
                return 0;
            }

            var chance = 0.5f + (nKills - nMinKills) * (2.5f / (nMaxKills - nMinKills));
            var ChangedGlobalNPC = NPC.Changed();
            return chance * ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }

        public void SwitchState(ActionState newState)
        {
            AIState = (float)newState;
            AITimer = 0;
            AIDoorKnockValue = 0;
            AIImpatienceTimer = 0;
        }

        public void StateDoor()
        {
            if (!NPC.HasValidTarget)
            {
                SwitchState(ActionState.Shrink);
                return;
            }

            AITimer++;

            NPC.velocity.X += NPC.direction * accel;
            if (AITimer >= knockDelay)
            {
                // Check if still at the door
                if (!(ChangedUtils.IsDoorAtTile(doorCheckX, doorCheckY - 1) || ChangedUtils.IsTallGateAtTile(doorCheckX, doorCheckY - 1)))
                {
                    SwitchState(ActionState.Walk);
                    return;
                }

                // Move a bit backwards, giving the impression of banging on the door
                NPC.velocity.X = -(float)NPC.direction;
                int doorOpenInc = 5;
                
                if (Main.tile[doorCheckX, doorCheckY - 1].TileType == TileID.TallGateClosed)
                {
                    doorOpenInc = 2;
                }

                AIDoorKnockValue += (float)doorOpenInc;
                AITimer = 0f;
                bool letMeIn = false;

                // LetMeIn after knocking
                if (AIDoorKnockValue >= 10f)
                {
                    letMeIn = true;
                }

                // Set fail to true, so it won't destroy the tile but only create particles
                WorldGen.KillTile(doorCheckX, doorCheckY - 1, true, false, false);
                if (letMeIn && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (ChangedUtils.IsDoorAtTile(doorCheckX, doorCheckY - 1))
                    {
                        bool canOpenDoor = WorldGen.OpenDoor(doorCheckX, doorCheckY - 1, NPC.direction);
                        if (!canOpenDoor)
                        {
                            // Break the door if he can't open it
                            WorldGen.KillTile(doorCheckX, doorCheckY - 1, false, false, false);

                            // Send server message for tile manipulation
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, doorCheckX, doorCheckY - 1, 0);
                            }
                        }
                        if (Main.netMode == NetmodeID.Server & canOpenDoor)
                        {
                            NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, (float)doorCheckX, (float)(doorCheckY - 1), (float)NPC.direction, 0, 0, 0);
                        }
                        SwitchState(ActionState.Walk);
                        return;
                    }

                    if (ChangedUtils.IsTallGateAtTile(doorCheckX, doorCheckY - 1))
                    {
                        bool canOpenTallGate = WorldGen.ShiftTallGate(doorCheckX, doorCheckY - 1, false);
                        if (!canOpenTallGate)
                        {
                            // Break the gate if it can't open
                            WorldGen.KillTile(doorCheckX, doorCheckY - 1, false, false, false);

                            // Send server message for tile manipulation
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, doorCheckX, doorCheckY - 1, 0);
                            }
                        }
                        if (Main.netMode == NetmodeID.Server & canOpenTallGate)
                        {
                            NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, (float)doorCheckX, (float)(doorCheckY - 1), 0f, 0, 0, 0);
                        }
                        SwitchState(ActionState.Walk);
                        return;
                    }
                }
            }
        }

        private void SpawnParticles()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            int dustType = Terraria.ID.DustID.TintableDust;
            Color color = Color.White;

            var nParticles = 2;
            for (int i = 0; i < nParticles; i++)
            {
                var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, dustType, 0, 0, 1, color);
                dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.scale *= NPC.scale + Main.rand.NextFloat(-0.03f, 0.03f);
            }
        }

        private void StateGrow()
        {
            AITimer++;
            SpawnParticles();

            if (NPC.velocity.Y == 0)
                NPC.velocity.X = 0;

            if (AITimer == 1)
            {
                SoundEngine.PlaySound(Sounds.SoundTransfur, NPC.Center);
                NPC.TargetClosest(false);
                var player = Main.player[NPC.target];
                int targetTileX = (int)player.Center.X / 16;
                int targetTileY = (int)player.Center.Y / 16;
                Vector2 chosenTile = Vector2.Zero;

                // If the player is grappling, be extra annoying and spawn on top of him
                if (ChangedUtils.IsPlayerGrappling(player))
                {
                    player.RemoveAllGrapplingHooks();
                    chosenTile = new Vector2(targetTileX, targetTileY);
                }
                else
                {
                    // Try to find a position on the ground
                    if (!NPC.AI_AttemptToFindTeleportSpot(ref chosenTile, targetTileX, targetTileY, 8, 5, 1, false, false))
                        // Drop him onto the player if no spot was found
                        chosenTile = new Vector2(targetTileX, targetTileY);
                }

                NPC.position.X = chosenTile.X * 16f - (float)(NPC.width / 2);
                NPC.position.Y = chosenTile.Y * 16f - (float)NPC.height;
            }

            if (NPC.scale >= 1)
            {
                SwitchState(ActionState.Walk);
            }

            NPC.scale += 0.02f;
        }

        private void StateShrink()
        {
            AITimer++;
            SpawnParticles();

            if (AITimer == 1)
            {
                SoundEngine.PlaySound(Sounds.SoundTransfur, NPC.Center);
            }

            if (NPC.scale < 0.02f)
            {
                NPC.TargetClosest(false);
                // Despawn if all players are dead
                if (!NPC.HasValidTarget)
                {
                    NPC.active = false;
                    return;
                }

                var dist = Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center);
                // Despawn if target is too far away
                if (dist > maxFollowDistance * maxFollowDistance)
                {
                    NPC.active = false;
                    return;
                }

                SwitchState(ActionState.Grow);
            }

            NPC.scale -= 0.02f;
        }

        private void StateWalk()
        {
            if (!NPC.HasValidTarget)
            {
                SwitchState(ActionState.Shrink);
                return;
            }

            bool noXMovement = false;
            if (NPC.velocity.X == 0f)
            {
                noXMovement = true;
            }

            bool isStuck = false;
            bool unusedFlag = false;

            // Not falling and sliding while facing the other direction
            if (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0)))
            {
                isStuck = true;
            }

            var argh = maxRunVelocity;
            argh += (1f - (float)NPC.life / (float)NPC.lifeMax) * 2f;

            // If stuck, increase the impatience timer and leave if it takes too long
            if ((NPC.position.X == NPC.oldPosition.X || AIImpatienceTimer >= (float)backUpTimer) | isStuck)
            {
                AIImpatienceTimer += 1f;
            }

            // Decrease impatience timer otherwise
            else if ((double)Math.Abs(NPC.velocity.X) > 0.9f * argh && AIImpatienceTimer > 0f)
            {
                AIImpatienceTimer -= 1f;
            }

            // Target the player again after a while of walking away
            if (AIImpatienceTimer > (float)(backUpTimer * wanderTime))
            {
                AIImpatienceTimer = 0f;
                SwitchState(ActionState.Shrink);
                return;
            }

            // Reset the impatience timer if he gets hurt, making him target the player
            if (NPC.justHit)
            {
                AIImpatienceTimer = 0f;
                noXMovement = false;
            }

            if (AIImpatienceTimer == (float)backUpTimer)
            {
                NPC.netUpdate = true;
            }

            float maxVelocity = maxWanderVelocity;

            // Normal logic, targeting player
            if (AIImpatienceTimer < (float)backUpTimer)
            {
                maxVelocity = maxRunVelocity;
                NPC.TargetClosest(true);
            }

            // Walking away
            else
            {
                if (NPC.velocity.X == 0f)
                {
                    if (NPC.velocity.Y == 0f)
                    {
                        AITimer += 1f;
                        // Turn around if stuck for too long
                        if (AITimer >= 2f)
                        {
                            NPC.direction *= -1;
                            NPC.spriteDirection = NPC.direction;
                            AITimer = 0f;
                        }
                    }
                }
                // Reset stuck timer if moving
                else
                {
                    AITimer = 0f;
                }

                if (NPC.direction == 0)
                {
                    // Face to the right
                    NPC.direction = 1;
                }
            }

            // Slow down if near player
            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) < 18f)
            {
                AIImpatienceTimer = 0f;
                NPC.velocity.X = NPC.velocity.X * 0.9f;
                if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                    NPC.velocity.X = 0f;
                return;
            }

            UpdateXSpeed(maxVelocity);
            SlopeLogic();

            if (IsOnSolidTile())
            {
                doorCheckX = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f) + NPC.direction;
                doorCheckY = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);

                if (ChangedUtils.IsDoorAtTile(doorCheckX, doorCheckY - 1) || ChangedUtils.IsTallGateAtTile(doorCheckX, doorCheckY - 1))
                {
                    SwitchState(ActionState.Door);
                    return;
                }
                else
                {
                    WalkLogic(doorCheckX, doorCheckY, unusedFlag, noXMovement);
                }
            }
        }

        private void Jump(float jumpHeight, float xSpeedMultiplier = 1f)
        {
            NPC.velocity.Y = -jumpHeight;
            NPC.velocity.X *= xSpeedMultiplier;
            // Do we really need this?
            NPC.netUpdate = true;
        }

        public void WalkLogic(int doorCheckX, int doorCheckY, bool unusedFlag, bool noXMovement)
        {
            int faceDirection = NPC.spriteDirection;

            // Moving into the direction we are facing at
            if (Math.Sign(NPC.velocity.X) == faceDirection)
            {
                if (NPC.height >= 32 && Main.tile[doorCheckX, doorCheckY - 2].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[doorCheckX, doorCheckY - 2].TileType])
                {
                    if (Main.tile[doorCheckX, doorCheckY - 3].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[doorCheckX, doorCheckY - 3].TileType])
                    {
                        Jump(8);
                    }
                    else
                    {
                        Jump(7);
                    }
                }
                else if (Main.tile[doorCheckX, doorCheckY - 1].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[doorCheckX, doorCheckY - 1].TileType])
                {
                    Jump(6);
                }
                else if (NPC.position.Y + (float)NPC.height - (float)(doorCheckY * 16) > 20f && 
                    Main.tile[doorCheckX, doorCheckY].HasUnactuatedTile && 
                    !Main.tile[doorCheckX, doorCheckY].TopSlope && 
                    Main.tileSolid[(int)Main.tile[doorCheckX, doorCheckY].TileType])
                {
                    Jump(5);
                }
                // Big jump, with some extra horizontal speed
                else if (NPC.directionY < 0 && 
                    (!Main.tile[doorCheckX, doorCheckY + 1].HasUnactuatedTile || 
                    !Main.tileSolid[(int)Main.tile[doorCheckX, doorCheckY + 1].TileType]) && 
                    (!Main.tile[doorCheckX + NPC.direction, doorCheckY + 1].HasUnactuatedTile || 
                    !Main.tileSolid[(int)Main.tile[doorCheckX + NPC.direction, doorCheckY + 1].TileType]))
                {
                    Jump(8, 1.5f);
                }
                // Chance to jump if near the player
                else if (NPC.HasValidTarget && Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) < 256 && Main.rand.NextBool(1 * 60))
                {
                    Jump(8, 1.5f);
                }
                if ((NPC.velocity.Y == 0f & noXMovement) && AIImpatienceTimer == 1f)
                {
                    Jump(5);
                }
            }
        }

        // Debug method to visualize a position by spamming confetti particles at it
        public void VisualizePosition(int xPos, int yPos)
        {
            var dustPos = new Vector2(xPos, yPos);

            for (int i = 0; i < 10; i++)
            {
                int dustType = Main.rand.Next(139, 143);
                var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, dustType);
                dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.scale *= 1f + Main.rand.NextFloat(-0.03f, 0.03f);
            }
        }

        private void SlopeLogic()
        {
            if (NPC.velocity.Y >= 0f)
            {
                int direction = Math.Sign(NPC.velocity.X);

                Vector2 pos = NPC.position;
                pos.X += NPC.velocity.X;

                int centerX = NPC.width / 2;
                int tileX = (int)((pos.X + centerX + (centerX + 1) * direction) / 16f);
                int tileY = (int)((pos.Y + NPC.height - 1f) / 16f);

                Tile tile1 = Main.tile[tileX, tileY];
                Tile tile2 = Main.tile[tileX, tileY - 1];
                Tile tile3 = Main.tile[tileX, tileY - 2];
                Tile tile4 = Main.tile[tileX, tileY - 3];
                Tile tile5 = Main.tile[tileX, tileY - 4];
                Tile tile6 = Main.tile[tileX - direction, tileY - 3];

                float tileWorldX = tileX * 16;
                float npcRight = pos.X + NPC.width;
                bool overlapsTile =
                    tileWorldX < npcRight &&
                    tileWorldX + 16 > pos.X;

                bool solidTile(Tile t) =>
                    t.HasUnactuatedTile &&
                    Main.tileSolid[t.TileType] &&
                    !Main.tileSolidTop[t.TileType];

                bool nonBlocking(Tile t) =>
                    !t.HasUnactuatedTile ||
                    !Main.tileSolid[t.TileType] ||
                    Main.tileSolidTop[t.TileType];

                bool stepableTile1 =
                    (solidTile(tile1) && !tile1.TopSlope && !tile2.TopSlope)
                    || (tile2.IsHalfBlock && tile2.HasUnactuatedTile);

                bool tile2Passable =
                    nonBlocking(tile2) ||
                    (tile2.IsHalfBlock && nonBlocking(tile5));

                bool spaceAboveClear =
                    nonBlocking(tile3) &&
                    nonBlocking(tile4) &&
                    nonBlocking(tile6);

                if (overlapsTile && stepableTile1 && tile2Passable && spaceAboveClear)
                {
                    float yPixel = tileY * 16;

                    if (tile1.IsHalfBlock)
                        yPixel += 8f;

                    if (tile2.IsHalfBlock)
                        yPixel -= 8f;

                    float npcBottom = pos.Y + NPC.height;

                    if (yPixel < npcBottom)
                    {
                        float rise = npcBottom - yPixel;

                        if (rise <= 16.1f)
                        {
                            NPC.gfxOffY += rise;
                            NPC.position.Y = yPixel - NPC.height;
                            NPC.stepSpeed = rise < 9f ? 1f : 2f;
                        }
                    }
                }
            }
        }

        // Fall through platforms if he has a target and the target is lower then him
        public override bool? CanFallThroughPlatforms()
        {
            if (!NPC.HasValidTarget) return false;
            else return Main.player[NPC.target].Center.Y> NPC.Center.Y + 32;
        }

        public bool IsOnSolidTile()
        {
            if (NPC.velocity.Y == 0f)
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

        private void UpdateXSpeed(float maxVelocity)
        {
            float acceleration = 0.08f;
            maxVelocity += (1f - (float)NPC.life / (float)NPC.lifeMax) * 2f;
            acceleration += (1f - (float)NPC.life / (float)NPC.lifeMax) * 0.2f;

            // Walking with speed exceeding maximum
            if (NPC.velocity.X < -maxVelocity || NPC.velocity.X > maxVelocity)
            {
                if (NPC.velocity.Y == 0f)
                    NPC.velocity *= 0.7f;
            }
            // Walking right - Facing right and speed less then max right speed
            else if (NPC.velocity.X < maxVelocity && NPC.direction == 1)
            {
                NPC.velocity.X = NPC.velocity.X + acceleration;
                if (NPC.velocity.X > maxVelocity)
                    NPC.velocity.X = maxVelocity;
            }
            // Walking left - Facing left and speed less then max left speed
            else if (NPC.velocity.X > -maxVelocity && NPC.direction == -1)
            {
                NPC.velocity.X = NPC.velocity.X - acceleration;
                if (NPC.velocity.X < -maxVelocity)
                    NPC.velocity.X = -maxVelocity;
            }
        }

        public override void AI()
        {
            switch(AIState)
            {
                case (float)ActionState.Walk:
                    StateWalk();
                    break;
                case (float)ActionState.Door:
                    StateDoor();
                    break;
                case (float)ActionState.Shrink:
                    StateShrink();
                    break;
                case (float)ActionState.Grow:
                    StateGrow();
                    break;
            }
        }
    }
}
