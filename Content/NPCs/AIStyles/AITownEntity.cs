using ChangedSpecialMod.Content.NPCs.AIMethods;
using ChangedSpecialMod.Content.NPCs.AIStates.Passive;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs.AIStyles
{
    // Modified passive aistyle from vanilla. Even thought this is custom ai logic, the NPC using this
    // must still have aistyle 7, or other NPCs won't be able to talk to him or will sit on top of him
    public static class AIPassive
    {
        public enum States
        {
            // Also used when the player is talking to him
            Idle = 0,
            Walking = 1,
            Blink = 2,
            TalkedToByOtherNPC = 3,
            TalkingToOtherNPC = 4,
            Sitting = 5,
            RunFromEnemy = 8,
            AttackThrowProjectile = 10,
            Snooze = 11,
            AttackShoot = 12,
            HealingSyringe = 13,
            AttackMagicProjectile = 14,
            AttackMeleeSwing = 15,
            HoldBeer = 18,
            Shimmering = 24,

            // Custom states
            WateringPlants = 30,

            // Unknown
            State6 = 6,
            State7 = 7,
            State9 = 9,
            State16 = 16,
            State17 = 17,
            State19 = 19,
            State20 = 20,
            State21 = 21,
            State22 = 22,
            State23 = 23,
        }

        public static void Update(NPC npc)
        {
            var AIState = (States)npc.ai[0];

            // Chance the NPC will...
            int waterPlantsChance = 75;     // Water plants if walking near them 
            int sitDownChance = 300;        // Sit down if walking near a chair

            // Check which npc it is
            var isPrototype = npc.type == ModContent.NPCType<Prototype>();
            var isDrK = npc.type == ModContent.NPCType<Scientist>();
            // Puro uses standard passive AI and only has custom animation logic

            // Abilities
            var canBreatheUnderwater = false;
            var canWaterPlants = isPrototype;
            var canThrowHealingSyringes = isDrK; // Nurse can throw healing syringes
            var canSnooze = false; // The pirate can snooze and shit himself. Why is this a thing in vanilla?! 
            var canHoldBeer = false; // Tavernkeep can hold a beer if the player is close

            bool shouldStayInside = Main.raining || !Main.dayTime || Main.eclipse || Main.slimeRain;
            float num2 = 1f;

            AIMethodsPassive.CalculateDefenseAndAttackMultiplier(npc, ref num2);

            npc.dontTakeDamage = false;
            if (npc.ai[0] == 25f)
            {
                npc.dontTakeDamage = true;
                if (npc.ai[1] == 0f)
                {
                    npc.velocity.X = 0f;
                }
                npc.shimmerWet = false;
                npc.wet = false;
                npc.lavaWet = false;
                npc.honeyWet = false;
                if (npc.ai[1] == 0f && Main.netMode == 1)
                {
                    return;
                }
                if (npc.ai[1] == 0f && npc.ai[2] < 1f)
                {
                    AIMethodsPassive.ShimmerTeleportToLandingSpot(npc);
                }
                if (npc.ai[2] > 0f)
                {
                    npc.ai[2] -= 1f;
                    if (npc.ai[2] <= 0f)
                    {
                        npc.ai[1] = 1f;
                    }
                    return;
                }
                npc.ai[1] += 1f;
                if (npc.ai[1] >= 30f)
                {
                    if (!Collision.WetCollision(npc.position, npc.width, npc.height))
                    {
                        npc.shimmerTransparency = MathHelper.Clamp(npc.shimmerTransparency - 1f / 60f, 0f, 1f);
                    }
                    else
                    {
                        npc.ai[1] = 30f;
                    }
                    npc.velocity = new Vector2(0f, -4f * npc.shimmerTransparency);
                }
                Rectangle hitbox = npc.Hitbox;
                hitbox.Y += 20;
                hitbox.Height -= 20;
                float num5 = Main.rand.NextFloatDirection();
                Lighting.AddLight(npc.Center, Main.hslToRgb((float)Main.timeForVisualEffects / 360f % 1f, 0.6f, 0.65f).ToVector3() * Utils.Remap(npc.ai[1], 30f, 90f, 0f, 0.7f));
                if (Main.rand.NextFloat() > Utils.Remap(npc.ai[1], 30f, 60f, 1f, 0.5f))
                {
                    Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox) + Main.rand.NextVector2Circular(8f, 0f) + new Vector2(0f, 4f), 309, new Vector2(0f, -2f).RotatedBy(num5 * ((float)Math.PI * 2f) * 0.11f), 0, default(Color), 1.7f - Math.Abs(num5) * 1.3f);
                }
                if (npc.ai[1] > 60f && Main.rand.Next(15) == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vector = Main.rand.NextVector2FromRectangle(npc.Hitbox);
                        ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerBlock, new ParticleOrchestraSettings
                        {
                            PositionInWorld = vector,
                            MovementVector = npc.DirectionTo(vector).RotatedBy((float)Math.PI * 9f / 20f * (float)(Main.rand.Next(2) * 2 - 1)) * Main.rand.NextFloat()
                        });
                    }
                }
                npc.TargetClosest();
                NPCAimedTarget targetData = npc.GetTargetData();
                if (npc.ai[1] >= 75f && npc.shimmerTransparency <= 0f && Main.netMode != 1)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    Math.Sign(targetData.Center.X - npc.Center.X);
                    npc.velocity = new Vector2(0f, -4f);
                    npc.localAI[0] = 0f;
                    npc.localAI[1] = 0f;
                    npc.localAI[2] = 0f;
                    npc.localAI[3] = 0f;
                    npc.netUpdate = true;
                    npc.townNpcVariationIndex = ((npc.townNpcVariationIndex != 1) ? 1 : 0);
                    NetMessage.SendData(MessageID.UniqueTownNPCInfoSyncRequest, -1, -1, null, npc.whoAmI);
                    npc.Teleport(npc.position, 12);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
                    {
                        PositionInWorld = npc.Center
                    });
                }
                return;
            }
            if (npc.type >= NPCID.None && NPCID.Sets.TownCritter[npc.type] && npc.target == 255)
            {
                npc.TargetClosest();
                if (npc.position.X < Main.player[npc.target].position.X)
                {
                    npc.direction = 1;
                    npc.spriteDirection = npc.direction;
                }
                if (npc.position.X > Main.player[npc.target].position.X)
                {
                    npc.direction = -1;
                    npc.spriteDirection = npc.direction;
                }
                if (npc.homeTileX == -1)
                {
                    npc.UpdateHomeTileState(npc.homeless, (int)((npc.position.X + (float)(npc.width / 2)) / 16f), npc.homeTileY);
                }
            }
            else if (npc.homeTileX == -1 && npc.homeTileY == -1 && npc.velocity.Y == 0f && !npc.shimmering)
            {
                npc.UpdateHomeTileState(npc.homeless, (int)npc.Center.X / 16, (int)(npc.position.Y + (float)npc.height + 4f) / 16);
            }
            bool flag3 = false;
            int myTileX = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
            int myTileY = (int)(npc.position.Y + (float)npc.height + 1f) / 16;
            AIMethodsPassive.FindGoodRestingSpot(npc, myTileX, myTileY, out var floorX, out var floorY);

            npc.directionY = -1;
            if (npc.direction == 0)
            {
                npc.direction = 1;
            }
            // Not shimmering
            if (npc.ai[0] != 24f)
            {
                for (int j = 0; j < Main.maxPlayers; j++)
                {
                    if (Main.player[j].active && Main.player[j].talkNPC == npc.whoAmI)
                    {
                        flag3 = true;
                        if (npc.ai[0] != 0f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.ai[0] = 0f;
                        npc.ai[1] = 300f;
                        npc.localAI[3] = 100f;
                        if (Main.player[j].position.X + (float)(Main.player[j].width / 2) < npc.position.X + (float)(npc.width / 2))
                        {
                            npc.direction = -1;
                        }
                        else
                        {
                            npc.direction = 1;
                        }
                    }
                }
            }
            if (npc.ai[3] == 1f)
            {
                npc.life = -1;
                npc.HitEffect();
                npc.active = false;
                npc.netUpdate = true;
                return;
            }

            // NPC is outside the world, or the tile is not loaded in multiplayer
            if (!WorldGen.InWorld(myTileX, myTileY) || (Main.netMode == NetmodeID.MultiplayerClient && !Main.sectionManager.TileLoaded(myTileX, myTileY)))
            {
                return;
            }

            if (!npc.homeless && Main.netMode != NetmodeID.MultiplayerClient && npc.townNPC && (shouldStayInside || (npc.type == NPCID.OldMan && Main.tileDungeon[Main.tile[myTileX, myTileY].TileType])) && !AIMethodsPassive.IsInAGoodRestingSpot(npc, myTileX, myTileY, floorX, floorY))
            {
                bool notOnAnyPlayersScreen = true;
                for (int k = 0; k < 2; k++)
                {
                    if (!notOnAnyPlayersScreen)
                    {
                        break;
                    }
                    Rectangle rectangle = new Rectangle((int)(npc.position.X + (float)(npc.width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(npc.position.Y + (float)(npc.height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    if (k == 1)
                    {
                        rectangle = new Rectangle(floorX * 16 + 8 - NPC.sWidth / 2 - NPC.safeRangeX, floorY * 16 + 8 - NPC.sHeight / 2 - NPC.safeRangeY, NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    }
                    for (int playerIndex = 0; playerIndex < Main.maxPlayers; playerIndex++)
                    {
                        var tmpPlayer = Main.player[playerIndex];
                        if (tmpPlayer.active && new Rectangle((int)tmpPlayer.position.X, (int)tmpPlayer.position.Y, tmpPlayer.width, tmpPlayer.height).Intersects(rectangle))
                        {
                            notOnAnyPlayersScreen = false;
                            break;
                        }
                    }
                }
                if (notOnAnyPlayersScreen)
                    AIMethodsPassive.TeleportToHome(npc, floorX, floorY);
            }

            bool isTurtle = npc.type == NPCID.Turtle || npc.type == NPCID.TurtleJungle || npc.type == NPCID.SeaTurtle;
            bool isFrogOrYellowTownSlime = npc.type == NPCID.Frog || npc.type == NPCID.GoldFrog || npc.type == NPCID.BoundTownSlimeYellow;
            bool flag8 = NPCID.Sets.IsTownSlime[npc.type];
            bool flag10 = isTurtle || isFrogOrYellowTownSlime;
            bool flag11 = flag8;
            bool flag12 = flag8;
            float dangerDetectRange = 200f;

            if (NPCID.Sets.DangerDetectRange[npc.type] != -1)
            {
                dangerDetectRange = NPCID.Sets.DangerDetectRange[npc.type];
            }

            bool enemyNearby = false;
            bool flag14 = false;
            float num9 = -1f;
            float num10 = -1f;
            int num11 = 0;
            int num12 = -1;
            int num13 = -1;
            bool keepwalking;

            if (!isTurtle && Main.netMode != 1 && !flag3)
            {
                for (int m = 0; m < Main.maxNPCs; m++)
                {
                    if (!Main.npc[m].active || Main.npc[m].friendly || Main.npc[m].damage <= 0 || !(Main.npc[m].Distance(npc.Center) < dangerDetectRange) || (npc.type == 453 && NPCID.Sets.Skeletons[Main.npc[m].type]) || (!Main.npc[m].noTileCollide && !Collision.CanHit(npc.Center, 0, 0, Main.npc[m].Center, 0, 0)) || !NPCLoader.CanHitNPC(Main.npc[m], npc))
                    {
                        continue;
                    }
                    bool flag15 = Main.npc[m].CanBeChasedBy(npc);
                    enemyNearby = true;
                    float num14 = Main.npc[m].Center.X - npc.Center.X;
                    
                    if (num14 < 0f && (num9 == -1f || num14 > num9))
                    {
                        num9 = num14;
                        if (flag15)
                        {
                            num12 = m;
                        }
                    }
                    if (num14 > 0f && (num10 == -1f || num14 < num10))
                    {
                        num10 = num14;
                        if (flag15)
                        {
                            num13 = m;
                        }
                    }
                }
                if (enemyNearby)
                {
                    num11 = ((num9 == -1f) ? 1 : ((num10 != -1f) ? (num10 < 0f - num9).ToDirectionInt() : (-1)));
                    float num15 = 0f;
                    if (num9 != -1f)
                    {
                        num15 = 0f - num9;
                    }
                    if (num15 == 0f || (num10 < num15 && num10 > 0f))
                    {
                        num15 = num10;
                    }
                    if (npc.ai[0] == 8f)
                    {
                        if (npc.direction == -num11)
                        {
                            npc.ai[0] = 1f;
                            npc.ai[1] = 300 + Main.rand.Next(300);
                            npc.ai[2] = 0f;
                            npc.localAI[3] = 0f;
                            npc.netUpdate = true;
                        }
                    }
                    else if (npc.ai[0] != 10f && npc.ai[0] != 12f && npc.ai[0] != 13f && npc.ai[0] != 14f && npc.ai[0] != 15f)
                    {
                        if (NPCID.Sets.PrettySafe[npc.type] != -1 && (float)NPCID.Sets.PrettySafe[npc.type] < num15)
                        {
                            enemyNearby = false;
                            flag14 = NPCID.Sets.AttackType[npc.type] > -1;
                        }
                        else if (npc.ai[0] != 1f)
                        {
                            int tileX = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                            int tileY = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                            bool currentlyDrowning = npc.wet && !canBreatheUnderwater;
                            AIMethodsPassive.GetWalkPrediction(npc, myTileX, floorX, canBreatheUnderwater, currentlyDrowning, tileX, tileY, out keepwalking, out var avoidFalling);
                            if (!avoidFalling)
                            {
                                if (npc.ai[0] == 3f || npc.ai[0] == 4f || npc.ai[0] == 16f || npc.ai[0] == 17f)
                                {
                                    NPC nPC = Main.npc[(int)npc.ai[2]];
                                    if (nPC.active)
                                    {
                                        nPC.ai[0] = 1f;
                                        nPC.ai[1] = 120 + Main.rand.Next(120);
                                        nPC.ai[2] = 0f;
                                        nPC.localAI[3] = 0f;
                                        nPC.direction = -num11;
                                        nPC.netUpdate = true;
                                    }
                                }
                                npc.ai[0] = 1f;
                                npc.ai[1] = 120 + Main.rand.Next(120);
                                npc.ai[2] = 0f;
                                npc.localAI[3] = 0f;
                                npc.direction = -num11;
                                npc.netUpdate = true;
                            }
                        }
                        else if (npc.ai[0] == 1f && npc.direction != -num11)
                        {
                            npc.direction = -num11;
                            npc.netUpdate = true;
                        }
                    }
                }
            }
            if (npc.ai[0] == 0f)
            {
                if (npc.localAI[3] > 0f)
                {
                    npc.localAI[3] -= 1f;
                }
                int num16 = 120;

                if ((isFrogOrYellowTownSlime || flag8) && npc.wet)
                {
                    npc.ai[0] = 1f;
                    npc.ai[1] = 200 + Main.rand.Next(500, 700);
                    npc.ai[2] = 0f;
                    npc.localAI[3] = 0f;
                    npc.netUpdate = true;
                }
                else if (shouldStayInside && !flag3 && !NPCID.Sets.TownCritter[npc.type])
                {
                    if (Main.netMode != 1)
                    {
                        if (myTileX == floorX && myTileY == floorY)
                        {
                            if (npc.velocity.X != 0f)
                            {
                                npc.netUpdate = true;
                            }
                            if (npc.velocity.X > 0.1f)
                            {
                                npc.velocity.X -= 0.1f;
                            }
                            else if (npc.velocity.X < -0.1f)
                            {
                                npc.velocity.X += 0.1f;
                            }
                            else
                            {
                                npc.velocity.X = 0f;
                                AIMethodsPassive.TryForcingSitting(npc, floorX, floorY);
                            }
                            if (NPCID.Sets.IsTownPet[npc.type])
                            {
                                AIMethodsPassive.AttemptToPlayIdleAnimationsForPets(npc, num16 * 4);
                            }
                        }
                        else
                        {
                            if (myTileX > floorX)
                            {
                                npc.direction = -1;
                            }
                            else
                            {
                                npc.direction = 1;
                            }
                            npc.ai[0] = 1f;
                            npc.ai[1] = 200 + Main.rand.Next(200);
                            npc.ai[2] = 0f;
                            npc.localAI[3] = 0f;
                            npc.netUpdate = true;
                        }
                    }
                }
                else
                {
                    if (npc.velocity.X > 0.1f)
                    {
                        npc.velocity.X -= 0.1f;
                    }
                    else if (npc.velocity.X < -0.1f)
                    {
                        npc.velocity.X += 0.1f;
                    }
                    else
                    {
                        npc.velocity.X = 0f;
                    }
                    if (Main.netMode != 1)
                    {
                        if (!flag3 && NPCID.Sets.IsTownPet[npc.type] && npc.ai[1] >= 100f && npc.ai[1] <= 150f)
                        {
                            AIMethodsPassive.AttemptToPlayIdleAnimationsForPets(npc, num16);
                        }
                        if (npc.ai[1] > 0f)
                        {
                            npc.ai[1] -= 1f;
                        }
                        bool flag16 = true;
                        int tileX2 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                        int tileY2 = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                        bool currentlyDrowning2 = npc.wet && !canBreatheUnderwater;
                        AIMethodsPassive.GetWalkPrediction(npc, myTileX, floorX, canBreatheUnderwater, currentlyDrowning2, tileX2, tileY2, out keepwalking, out var avoidFalling2);
                        if (npc.wet && !canBreatheUnderwater)
                        {
                            bool currentlyDrowning3 = Collision.DrownCollision(npc.position, npc.width, npc.height, 1f, includeSlopes: true);
                            if (AIMethodsPassive.CheckIfWillDrown(currentlyDrowning3))
                            {
                                npc.ai[0] = 1f;
                                npc.ai[1] = 200 + Main.rand.Next(300);
                                npc.ai[2] = 0f;
                                if (NPCID.Sets.TownCritter[npc.type])
                                {
                                    npc.ai[1] += Main.rand.Next(200, 400);
                                }
                                npc.localAI[3] = 0f;
                                npc.netUpdate = true;
                            }
                        }
                        if (avoidFalling2)
                        {
                            flag16 = false;
                        }
                        if (npc.ai[1] <= 0f)
                        {
                            if (flag16 && !avoidFalling2)
                            {
                                npc.ai[0] = 1f;
                                npc.ai[1] = 200 + Main.rand.Next(300);
                                npc.ai[2] = 0f;
                                if (NPCID.Sets.TownCritter[npc.type])
                                {
                                    npc.ai[1] += Main.rand.Next(200, 400);
                                }
                                npc.localAI[3] = 0f;
                                npc.netUpdate = true;
                            }
                            else
                            {
                                npc.direction *= -1;
                                npc.ai[1] = 60 + Main.rand.Next(120);
                                npc.netUpdate = true;
                            }
                        }
                    }
                }
                if (Main.netMode != 1 && (!shouldStayInside || AIMethodsPassive.IsInAGoodRestingSpot(npc, myTileX, myTileY, floorX, floorY)))
                {
                    if (myTileX < floorX - 25 || myTileX > floorX + 25)
                    {
                        if (npc.localAI[3] == 0f)
                        {
                            if (myTileX < floorX - 50 && npc.direction == -1)
                            {
                                npc.direction = 1;
                                npc.netUpdate = true;
                            }
                            else if (myTileX > floorX + 50 && npc.direction == 1)
                            {
                                npc.direction = -1;
                                npc.netUpdate = true;
                            }
                        }
                    }
                    else if (Main.rand.Next(80) == 0 && npc.localAI[3] == 0f)
                    {
                        npc.localAI[3] = 200f;
                        npc.direction *= -1;
                        npc.netUpdate = true;
                    }
                }
            }
            else if (npc.ai[0] == 1f)
            {
                StateWalking.Update(npc, shouldStayInside, floorX, floorY, enemyNearby, isFrogOrYellowTownSlime, flag8, flag11, flag12, isFrogOrYellowTownSlime, myTileX, myTileY, canBreatheUnderwater, isTurtle);
            }
            else if (AIState == States.Blink || AIState == States.Snooze)
            {
                StateBlinkSnooze.Update(npc);
            }
            else if (AIState == States.TalkedToByOtherNPC || AIState == States.TalkingToOtherNPC ||
                AIState == States.Sitting || AIState == States.RunFromEnemy || AIState == States.State9 || 
                npc.ai[0] == 16f || npc.ai[0] == 17f || npc.ai[0] == 20f || npc.ai[0] == 21f || 
                npc.ai[0] == 22f || npc.ai[0] == 23f || AIState == States.WateringPlants)
            {
                npc.velocity.X *= 0.8f;
                npc.ai[1] -= 1f;

                // Keep running if enemy is still nearby
                if (npc.ai[0] == 8f && npc.ai[1] < 60f && enemyNearby)
                {
                    npc.ai[1] = 180f;
                    npc.netUpdate = true;
                }
                if (AIState == States.Sitting)
                {
                    Point coords = (npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                    Tile tile = Main.tile[coords.X, coords.Y];
                    if (!TileID.Sets.CanBeSatOnForNPCs[tile.TileType])
                    {
                        npc.ai[1] = 0f;
                    }
                    else
                    {
                        Main.sittingManager.AddNPC(npc.whoAmI, coords);
                    }
                }
                // Switch back to idle
                if (npc.ai[1] <= 0f)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 60 + Main.rand.Next(60);
                    npc.ai[2] = 0f;
                    npc.localAI[3] = 30 + Main.rand.Next(60);
                    npc.netUpdate = true;
                }
            }
            else if (AIState == States.State6 || AIState == States.State7 || 
                AIState == States.HoldBeer || AIState == States.State19)
            {
                if (npc.ai[0] == 18f && (npc.localAI[3] < 1f || npc.localAI[3] > 2f))
                {
                    npc.localAI[3] = 2f;
                }
                npc.velocity.X *= 0.8f;
                npc.ai[1] -= 1f;
                int num34 = (int)npc.ai[2];
                if (num34 < 0 || num34 > 255 || !Main.player[num34].CanBeTalkedTo || Main.player[num34].Distance(npc.Center) > 200f || !Collision.CanHitLine(npc.Top, 0, 0, Main.player[num34].Top, 0, 0))
                {
                    npc.ai[1] = 0f;
                }
                if (npc.ai[1] > 0f)
                {
                    int num35 = ((npc.Center.X < Main.player[num34].Center.X) ? 1 : (-1));
                    if (num35 != npc.direction)
                    {
                        npc.netUpdate = true;
                    }
                    npc.direction = num35;
                }
                else
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 60 + Main.rand.Next(60);
                    npc.ai[2] = 0f;
                    npc.localAI[3] = 30 + Main.rand.Next(60);
                    npc.netUpdate = true;
                }
            }
            else if (AIState == States.AttackThrowProjectile)
                StateAttackThrowProjectile.Update(npc, enemyNearby, num2, num11, num12, num13);
            else if (AIState == States.AttackShoot)
                StateAttackShoot.Update(npc, enemyNearby, num2, num11, num12, num13);
            else if (AIState == States.HealingSyringe)
                StateHealingSyringe.Update(npc);
            else if (AIState == States.AttackMagicProjectile)
                StateAttackMagicProjectile.Update(npc, enemyNearby, num2, num11, num12, num13);
            else if (AIState == States.AttackMeleeSwing)
                State15.Update(npc, enemyNearby, num2, num11, num12, num13);
            else if (AIState == States.Shimmering)
                StateShimmering.Update(npc);

            if (flag11 && npc.wet)
            {
                int num84 = (int)(npc.Center.X / 16f);
                int num85 = 5;
                if (npc.collideX || (num84 < num85 && npc.direction == -1) || (num84 > Main.maxTilesX - num85 && npc.direction == 1))
                {
                    npc.direction *= -1;
                    npc.velocity.X *= -0.25f;
                    npc.netUpdate = true;
                }
                npc.velocity.Y *= 0.9f;
                npc.velocity.Y -= 0.5f;
                if (npc.velocity.Y < -15f)
                {
                    npc.velocity.Y = -15f;
                }
            }
            if (flag10 && npc.wet)
            {
                if (isFrogOrYellowTownSlime)
                {
                    npc.ai[1] = 50f;
                }
                int num86 = (int)(npc.Center.X / 16f);
                int num87 = 5;
                if (npc.collideX || (num86 < num87 && npc.direction == -1) || (num86 > Main.maxTilesX - num87 && npc.direction == 1))
                {
                    npc.direction *= -1;
                    npc.velocity.X *= -0.25f;
                    npc.netUpdate = true;
                }
                if (Collision.GetWaterLine(npc.Center.ToTileCoordinates(), out var waterLineHeight))
                {
                    float num88 = npc.Center.Y + 1f;
                    if (npc.Center.Y > waterLineHeight)
                    {
                        npc.velocity.Y -= 0.8f;
                        if (npc.velocity.Y < -4f)
                        {
                            npc.velocity.Y = -4f;
                        }
                        if (num88 + npc.velocity.Y < waterLineHeight)
                        {
                            npc.velocity.Y = waterLineHeight - num88;
                        }
                    }
                    else
                    {
                        npc.velocity.Y = MathHelper.Min(npc.velocity.Y, waterLineHeight - num88);
                    }
                }
                else
                {
                    npc.velocity.Y -= 0.2f;
                }
            }
            if (Main.netMode != 1 && npc.isLikeATownNPC && !flag3)
            {
                bool flag26 = npc.ai[0] < 2f && !enemyNearby && !npc.wet;
                bool flag27 = (npc.ai[0] < 2f || npc.ai[0] == 8f) && (enemyNearby || flag14);
                if (npc.localAI[1] > 0f)
                {
                    npc.localAI[1] -= 1f;
                }
                if (npc.localAI[1] > 0f)
                {
                    flag27 = false;
                }
                if (npc.CanTalk && flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(300) == 0)
                {
                    int num90 = 420;
                    num90 = ((Main.rand.Next(2) != 0) ? (num90 * Main.rand.Next(1, 3)) : (num90 * Main.rand.Next(1, 4)));
                    int num91 = 100;
                    int num92 = 20;
                    for (int num93 = 0; num93 < 200; num93++)
                    {
                        NPC nPC4 = Main.npc[num93];
                        bool flag28 = (nPC4.ai[0] == 1f && nPC4.closeDoor) || (nPC4.ai[0] == 1f && nPC4.ai[1] > 200f) || nPC4.ai[0] > 1f || nPC4.wet;
                        if (nPC4 != npc && nPC4.active && nPC4.CanBeTalkedTo && !flag28 && nPC4.Distance(npc.Center) < (float)num91 && nPC4.Distance(npc.Center) > (float)num92 && Collision.CanHit(npc.Center, 0, 0, nPC4.Center, 0, 0))
                        {
                            int num94 = (npc.position.X < nPC4.position.X).ToDirectionInt();
                            npc.ai[0] = 3f;
                            npc.ai[1] = num90;
                            npc.ai[2] = num93;
                            npc.direction = num94;
                            npc.netUpdate = true;
                            nPC4.ai[0] = 4f;
                            nPC4.ai[1] = num90;
                            nPC4.ai[2] = npc.whoAmI;
                            nPC4.direction = -num94;
                            nPC4.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (npc.CanTalk && flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(1800) == 0)
                {
                    int num95 = 420;
                    num95 = ((Main.rand.Next(2) != 0) ? (num95 * Main.rand.Next(1, 3)) : (num95 * Main.rand.Next(1, 4)));
                    int num96 = 100;
                    int num97 = 20;
                    for (int num98 = 0; num98 < 200; num98++)
                    {
                        NPC nPC5 = Main.npc[num98];
                        bool flag29 = (nPC5.ai[0] == 1f && nPC5.closeDoor) || (nPC5.ai[0] == 1f && nPC5.ai[1] > 200f) || nPC5.ai[0] > 1f || nPC5.wet;
                        if (nPC5 != npc && nPC5.active && nPC5.CanBeTalkedTo && !NPCID.Sets.IsTownPet[nPC5.type] && !flag29 && nPC5.Distance(npc.Center) < (float)num96 && nPC5.Distance(npc.Center) > (float)num97 && Collision.CanHit(npc.Center, 0, 0, nPC5.Center, 0, 0))
                        {
                            int num99 = (npc.position.X < nPC5.position.X).ToDirectionInt();
                            npc.ai[0] = 16f;
                            npc.ai[1] = num95;
                            npc.ai[2] = num98;
                            npc.localAI[2] = Main.rand.Next(4);
                            npc.localAI[3] = Main.rand.Next(3 - (int)npc.localAI[2]);
                            npc.direction = num99;
                            npc.netUpdate = true;
                            nPC5.ai[0] = 17f;
                            nPC5.ai[1] = num95;
                            nPC5.ai[2] = npc.whoAmI;
                            nPC5.localAI[2] = 0f;
                            nPC5.localAI[3] = 0f;
                            nPC5.direction = -num99;
                            nPC5.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (!NPCID.Sets.IsTownPet[npc.type] && flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(1200) == 0 && (npc.type == NPCID.PartyGirl || (BirthdayParty.PartyIsUp && NPCID.Sets.AttackType[npc.type] == NPCID.Sets.AttackType[208])))
                {
                    int num100 = 300;
                    int num101 = 150;
                    for (int num102 = 0; num102 < 255; num102++)
                    {
                        Player player = Main.player[num102];
                        if (player.active && !player.dead && player.Distance(npc.Center) < (float)num101 && Collision.CanHitLine(npc.Top, 0, 0, player.Top, 0, 0))
                        {
                            int num103 = (npc.position.X < player.position.X).ToDirectionInt();
                            npc.ai[0] = 6f;
                            npc.ai[1] = num100;
                            npc.ai[2] = num102;
                            npc.direction = num103;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
                // Holding a beer
                else if (flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && canHoldBeer)
                {
                    int num104 = 300;
                    int num105 = 150;
                    for (int num106 = 0; num106 < 255; num106++)
                    {
                        Player player2 = Main.player[num106];
                        if (player2.active && !player2.dead && player2.Distance(npc.Center) < (float)num105 && Collision.CanHitLine(npc.Top, 0, 0, player2.Top, 0, 0))
                        {
                            int num107 = (npc.position.X < player2.position.X).ToDirectionInt();
                            npc.ai[0] = 18f;
                            npc.ai[1] = num104;
                            npc.ai[2] = num106;
                            npc.direction = num107;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
                // Blink
                else if (!NPCID.Sets.IsTownPet[npc.type] && flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(1800) == 0)
                {
                    npc.ai[0] = (float)States.Blink;
                    npc.ai[1] = 45 * Main.rand.Next(1, 2);
                    npc.netUpdate = true;
                }
                // Snooze
                else if (flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && canSnooze && !flag14)
                {
                    npc.ai[0] = (float)States.Snooze;
                    npc.ai[1] = 30 * Main.rand.Next(1, 4);
                    npc.netUpdate = true;
                }
                else if (flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(1200) == 0)
                {
                    int num108 = 220;
                    int num109 = 150;
                    for (int num110 = 0; num110 < 255; num110++)
                    {
                        Player player3 = Main.player[num110];
                        if (player3.CanBeTalkedTo && player3.Distance(npc.Center) < (float)num109 && Collision.CanHitLine(npc.Top, 0, 0, player3.Top, 0, 0))
                        {
                            int num111 = (npc.position.X < player3.position.X).ToDirectionInt();
                            npc.ai[0] = 7f;
                            npc.ai[1] = num108;
                            npc.ai[2] = num110;
                            npc.direction = num111;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (flag26 && npc.ai[0] == 1f && npc.velocity.Y == 0f && sitDownChance > 0 && Main.rand.Next(sitDownChance) == 0)
                {
                    Point point = (npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                    bool flag30 = WorldGen.InWorld(point.X, point.Y, 1);
                    if (flag30)
                    {
                        for (int num112 = 0; num112 < 200; num112++)
                        {
                            if (Main.npc[num112].active && Main.npc[num112].aiStyle == NPCAIStyleID.Passive && Main.npc[num112].townNPC && Main.npc[num112].ai[0] == 5f && (Main.npc[num112].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point)
                            {
                                flag30 = false;
                                break;
                            }
                        }
                        for (int num113 = 0; num113 < 255; num113++)
                        {
                            if (Main.player[num113].active && Main.player[num113].sitting.isSitting && Main.player[num113].Center.ToTileCoordinates() == point)
                            {
                                flag30 = false;
                                break;
                            }
                        }
                    }
                    if (flag30)
                    {
                        Tile tile2 = Main.tile[point.X, point.Y];
                        flag30 = TileID.Sets.CanBeSatOnForNPCs[tile2.TileType];
                        if (flag30 && tile2.TileType == TileID.Chairs && tile2.TileFrameY >= 1080 && tile2.TileFrameY <= 1098)
                        {
                            flag30 = false;
                        }
                        if (flag30)
                        {
                            npc.ai[0] = 5f;
                            npc.ai[1] = 900 + Main.rand.Next(10800);
                            npc.SitDown(point, out var targetDirection, out var bottom);
                            npc.direction = targetDirection;
                            npc.Bottom = bottom;
                            npc.velocity = Vector2.Zero;
                            npc.localAI[3] = 0f;
                            npc.netUpdate = true;
                        }
                    }
                }
                else if (flag26 && npc.ai[0] == 1f && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && Utils.PlotTileLine(npc.Top, npc.Bottom, npc.width, DelegateMethods.SearchAvoidedByNPCs))
                {
                    Point point2 = (npc.Center + new Vector2(npc.direction * 10, 0f)).ToTileCoordinates();
                    bool flag31 = WorldGen.InWorld(point2.X, point2.Y, 1);
                    if (flag31)
                    {
                        Tile tileSafely7 = Framing.GetTileSafely(point2.X, point2.Y);
                        if (!tileSafely7.HasUnactuatedTile || !TileID.Sets.InteractibleByNPCs[tileSafely7.TileType])
                        {
                            flag31 = false;
                        }
                    }
                    if (flag31)
                    {
                        npc.ai[0] = 9f;
                        npc.ai[1] = 40 + Main.rand.Next(90);
                        npc.velocity = Vector2.Zero;
                        npc.localAI[3] = 0f;
                        npc.netUpdate = true;
                    }
                }
                // New code. Randomly try to water plants
                else if (canWaterPlants && flag26 && AIState == States.Walking && npc.velocity.Y == 0f && waterPlantsChance > 0 && Main.rand.Next(waterPlantsChance) == 0)
                    StateTryWaterPlants.Update(npc);
                if (Main.netMode != 1 && npc.ai[0] < 2f && npc.velocity.Y == 0f && canThrowHealingSyringes && npc.breath > 0)
                {
                    int num114 = -1;
                    for (int num115 = 0; num115 < 200; num115++)
                    {
                        NPC nPC6 = Main.npc[num115];
                        if (nPC6.active && nPC6.townNPC && nPC6.life != nPC6.lifeMax && (num114 == -1 || nPC6.lifeMax - nPC6.life > Main.npc[num114].lifeMax - Main.npc[num114].life) && Collision.CanHitLine(npc.position, npc.width, npc.height, nPC6.position, nPC6.width, nPC6.height) && npc.Distance(nPC6.Center) < 500f)
                        {
                            num114 = num115;
                        }
                    }
                    if (num114 != -1)
                    {
                        npc.ai[0] = 13f;
                        npc.ai[1] = 34f;
                        npc.ai[2] = num114;
                        npc.localAI[3] = 0f;
                        npc.direction = ((npc.position.X < Main.npc[num114].position.X) ? 1 : (-1));
                        npc.netUpdate = true;
                    }
                }
                if (flag27 && npc.velocity.Y == 0f && NPCID.Sets.AttackType[npc.type] == 0 && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)
                {
                    int num116 = NPCID.Sets.AttackTime[npc.type];
                    int num117 = ((num11 == 1) ? num13 : num12);
                    int num118 = ((num11 == 1) ? num12 : num13);
                    if (num117 != -1 && !Collision.CanHit(npc.Center, 0, 0, Main.npc[num117].Center, 0, 0))
                    {
                        num117 = ((num118 == -1 || !Collision.CanHit(npc.Center, 0, 0, Main.npc[num118].Center, 0, 0)) ? (-1) : num118);
                    }
                    bool flag32 = num117 != -1;
                    if (flag32)
                    {
                        npc.localAI[2] = npc.ai[0];
                        npc.ai[0] = 10f;
                        npc.ai[1] = num116;
                        npc.ai[2] = 0f;
                        npc.localAI[3] = 0f;
                        npc.direction = ((npc.position.X < Main.npc[num117].position.X) ? 1 : (-1));
                        npc.netUpdate = true;
                    }
                }
                else if (flag27 && npc.velocity.Y == 0f && NPCID.Sets.AttackType[npc.type] == 1 && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)
                {
                    int num119 = NPCID.Sets.AttackTime[npc.type];
                    int num120 = ((num11 == 1) ? num13 : num12);
                    int num121 = ((num11 == 1) ? num12 : num13);
                    if (num120 != -1 && !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[num120].Center, 0, 0))
                    {
                        num120 = ((num121 == -1 || !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[num121].Center, 0, 0)) ? (-1) : num121);
                    }
                    if (num120 != -1)
                    {
                        Vector2 vector8 = npc.DirectionTo(Main.npc[num120].Center);
                        if (vector8.Y <= 0.5f && vector8.Y >= -0.5f)
                        {
                            npc.localAI[2] = npc.ai[0];
                            npc.ai[0] = 12f;
                            npc.ai[1] = num119;
                            npc.ai[2] = vector8.Y;
                            npc.localAI[3] = 0f;
                            npc.direction = ((npc.position.X < Main.npc[num120].position.X) ? 1 : (-1));
                            npc.netUpdate = true;
                        }
                    }
                }
                if (flag27 && npc.velocity.Y == 0f && NPCID.Sets.AttackType[npc.type] == 2 && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)
                {
                    int num122 = NPCID.Sets.AttackTime[npc.type];
                    int num123 = ((num11 == 1) ? num13 : num12);
                    int num124 = ((num11 == 1) ? num12 : num13);
                    if (num123 != -1 && !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[num123].Center, 0, 0))
                    {
                        num123 = ((num124 == -1 || !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[num124].Center, 0, 0)) ? (-1) : num124);
                    }
                    if (num123 != -1)
                    {
                        npc.localAI[2] = npc.ai[0];
                        npc.ai[0] = 14f;
                        npc.ai[1] = num122;
                        npc.ai[2] = 0f;
                        npc.localAI[3] = 0f;
                        npc.direction = ((npc.position.X < Main.npc[num123].position.X) ? 1 : (-1));
                        npc.netUpdate = true;
                    }
                }
                if (flag27 && npc.velocity.Y == 0f && NPCID.Sets.AttackType[npc.type] == 3 && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)
                {
                    int num125 = NPCID.Sets.AttackTime[npc.type];
                    int num126 = ((num11 == 1) ? num13 : num12);
                    int num127 = ((num11 == 1) ? num12 : num13);
                    if (num126 != -1 && !Collision.CanHit(npc.Center, 0, 0, Main.npc[num126].Center, 0, 0))
                    {
                        num126 = ((num127 == -1 || !Collision.CanHit(npc.Center, 0, 0, Main.npc[num127].Center, 0, 0)) ? (-1) : num127);
                    }
                    if (num126 != -1)
                    {
                        npc.localAI[2] = npc.ai[0];
                        npc.ai[0] = 15f;
                        npc.ai[1] = num125;
                        npc.ai[2] = 0f;
                        npc.localAI[3] = 0f;
                        npc.direction = ((npc.position.X < Main.npc[num126].position.X) ? 1 : (-1));
                        npc.netUpdate = true;
                    }
                }
            }
        }
    }
}
