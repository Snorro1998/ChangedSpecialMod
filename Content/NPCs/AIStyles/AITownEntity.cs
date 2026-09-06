using ChangedSpecialMod.Content.NPCs.AIMethods;
using ChangedSpecialMod.Content.NPCs.AIStates.Passive;
using Microsoft.Xna.Framework;
using Terraria;
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
            Shimmering = 24,

            // Custom states
            WateringPlants = 30,

            // Unknown
            State6 = 6,
            State7 = 7,
            State9 = 9,
            State16 = 16,
            State17 = 17,
            State18 = 18,
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

            // Abilities
            var canBreatheUnderwater = false;
            var canWaterPlants = isPrototype;
            var canThrowHealingSyringes = isDrK; // Nurse can throw healing syringes
            var canSnooze = false; // The pirate can snooze and shit himself. Why is this a thing in vanilla?! 

            bool shouldStayInside = Main.raining || !Main.dayTime || Main.eclipse || Main.slimeRain;
            NPC.ShimmeredTownNPCs[npc.type] = npc.IsShimmerVariant;
            float num2 = 1f;

            AIMethodsPassive.CalculateDefenseAndAttackMultiplier(npc, ref num2);

            npc.dontTakeDamage = false;
            // Shimmering
            if (npc.ai[0] == 25f)
            {
                State25.Update(npc);
                return;
            }
            if (npc.homeTileX == -1 && npc.homeTileY == -1 && npc.velocity.Y == 0f && !npc.shimmering)
            {
                npc.UpdateHomeTileState(npc.homeless, (int)npc.Center.X / 16, (int)(npc.position.Y + (float)npc.height + 4f) / 16);
            }
            bool talkingWithPlayer = false;
            int num6 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
            int num7 = (int)(npc.position.Y + (float)npc.height + 1f) / 16;
            AIMethodsPassive.FindGoodRestingSpot(npc, num6, num7, out var floorX, out var floorY);
            
            npc.directionY = -1;
            if (npc.direction == 0)
            {
                npc.direction = 1;
            }
            // If he is not shimmering
            if (npc.ai[0] != 24f)
            {
                for (int j = 0; j < Main.maxPlayers; j++)
                {
                    // If a player is talking to him
                    if (Main.player[j].active && Main.player[j].talkNPC == npc.whoAmI)
                    {
                        talkingWithPlayer = true;
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
            // Kills the NPC
            if (npc.ai[3] == 1f)
            {
                npc.life = -1;
                npc.HitEffect();
                npc.active = false;
                npc.netUpdate = true;
                return;
            }
            if (!WorldGen.InWorld(num6, num7) || (Main.netMode == NetmodeID.MultiplayerClient && !Main.sectionManager.TileLoaded(num6, num7)))
            {
                return;
            }
            if (!npc.homeless && Main.netMode != NetmodeID.MultiplayerClient && npc.townNPC && (shouldStayInside || (npc.type == NPCID.OldMan && Main.tileDungeon[Main.tile[num6, num7].TileType])) && !AIMethodsPassive.IsInAGoodRestingSpot(npc, num6, num7, floorX, floorY))
            {
                bool shouldTeleportHome = true;
                for (int k = 0; k < 2; k++)
                {
                    if (!shouldTeleportHome)
                        break;
                    Rectangle rectangle = new Rectangle((int)(npc.position.X + (float)(npc.width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(npc.position.Y + (float)(npc.height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    if (k == 1)
                        rectangle = new Rectangle(floorX * 16 + 8 - NPC.sWidth / 2 - NPC.safeRangeX, floorY * 16 + 8 - NPC.sHeight / 2 - NPC.safeRangeY, NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    for (int l = 0; l < 255; l++)
                    {
                        if (Main.player[l].active && new Rectangle((int)Main.player[l].position.X, (int)Main.player[l].position.Y, Main.player[l].width, Main.player[l].height).Intersects(rectangle))
                        {
                            shouldTeleportHome = false;
                            break;
                        }
                    }
                }
                if (shouldTeleportHome)
                    AIMethodsPassive.TeleportToHome(npc, floorX, floorY);
            }

            bool isFrogOrYellowTownSlime = npc.type == NPCID.Frog || npc.type == NPCID.GoldFrog || npc.type == NPCID.BoundTownSlimeYellow;
            bool flag8 = NPCID.Sets.IsTownSlime[npc.type];
            bool flag11 = flag8;
            bool flag12 = flag8;
            float dangerDetecRange = 200f;
            
            if (NPCID.Sets.DangerDetectRange[npc.type] != -1)
            {
                dangerDetecRange = NPCID.Sets.DangerDetectRange[npc.type];
            }

            bool enemyNearby = false;
            bool flag14 = false;
            float num9 = -1f;
            float num10 = -1f;
            int num11 = 0;
            int num12 = -1;
            int num13 = -1;
            bool keepwalking = false;

            // Check for enemies
            if (Main.netMode != NetmodeID.MultiplayerClient && !talkingWithPlayer)
            {
                for (int m = 0; m < 200; m++)
                {
                    if (!Main.npc[m].active || Main.npc[m].friendly || Main.npc[m].damage <= 0 || !(Main.npc[m].Distance(npc.Center) < dangerDetecRange) || (npc.type == NPCID.SkeletonMerchant && NPCID.Sets.Skeletons[Main.npc[m].type]) || (!Main.npc[m].noTileCollide && !Collision.CanHit(npc.Center, 0, 0, Main.npc[m].Center, 0, 0)) || !NPCLoader.CanHitNPC(Main.npc[m], npc))
                    {
                        continue;
                    }
                    bool flag15 = Main.npc[m].CanBeChasedBy(npc);
                    enemyNearby = true;
                    float xDifference = Main.npc[m].Center.X - npc.Center.X;

                    // Enemy to the right
                    if (xDifference < 0f && (num9 == -1f || xDifference > num9))
                    {
                        num9 = xDifference;
                        if (flag15)
                        {
                            num12 = m;
                        }
                    }
                    // Enemy to the left
                    if (xDifference > 0f && (num10 == -1f || xDifference < num10))
                    {
                        num10 = xDifference;
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
                    if (AIState == States.RunFromEnemy)
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
                        else if (AIState != States.Walking)
                        {
                            int tileX = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                            int tileY = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                            bool currentlyDrowning = npc.wet && !canBreatheUnderwater;
                            AIMethodsPassive.GetWalkPrediction(npc, num6, floorX, canBreatheUnderwater, currentlyDrowning, tileX, tileY, out keepwalking, out var avoidFalling);
                            if (!avoidFalling)
                            {
                                if (AIState == States.TalkedToByOtherNPC || AIState == States.TalkingToOtherNPC || AIState == States.State16 || AIState == States.State17)
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
                        else if (AIState == States.Walking && npc.direction != -num11)
                        {
                            npc.direction = -num11;
                            npc.netUpdate = true;
                        }
                    }
                }
            }

            switch(AIState)
            { 
                case States.Idle:
                    StateIdle.Update(npc, shouldStayInside, talkingWithPlayer, isFrogOrYellowTownSlime, flag8, canBreatheUnderwater, floorX, floorY, num6, num7, ref keepwalking);
                    break;
                case States.Walking:
                    StateWalking.Update(npc, shouldStayInside, floorX, floorY, enemyNearby, canBreatheUnderwater, flag11, flag12, isFrogOrYellowTownSlime, num6, num7);
                    break;
                case States.Blink:
                case States.Snooze:
                    StateBlinkSnooze.Update(npc);
                    break;

                case States.TalkedToByOtherNPC:
                case States.TalkingToOtherNPC:
                case States.Sitting:
                case States.RunFromEnemy:
                case States.State9:
                case States.State16:
                case States.State17:
                case States.State20:
                case States.State21:
                case States.State22:
                case States.State23:
                case States.WateringPlants:
                    StateMisc.Update(npc, AIState, enemyNearby);
                    break;

                case States.State6:
                case States.State7:
                case States.State18:
                case States.State19:
                    State6_7_18_19.Update(npc);
                    break;

                case States.AttackThrowProjectile:
                    StateAttackThrowProjectile.Update(npc, enemyNearby, num2, num11, num12, num13);
                    break;
                case States.AttackShoot:
                    State12.Update(npc, enemyNearby, num2, num11, num12, num13);
                    break;
                case States.HealingSyringe:
                    State13.Update(npc);
                    break;
                case States.AttackMagicProjectile:
                    StateAttackMagicProjectile.Update(npc, enemyNearby, num2, num11, num12, num13);
                    break;
                case States.AttackMeleeSwing:
                    State15.Update(npc, enemyNearby, num2, num11, num12, num13);
                    break;
                case States.Shimmering:
                    StateShimmering.Update(npc);
                    break;
            }

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
            if (Main.netMode != NetmodeID.MultiplayerClient && npc.isLikeATownNPC && !talkingWithPlayer)
            {
                bool noEnemyNearbyAndNotWet = npc.ai[0] < 2f && !enemyNearby && !npc.wet;
                bool flag27 = (npc.ai[0] < 2f || AIState == States.RunFromEnemy) && (enemyNearby || flag14);
                if (npc.localAI[1] > 0f)
                {
                    npc.localAI[1] -= 1f;
                }
                if (npc.localAI[1] > 0f)
                {
                    flag27 = false;
                }

                AIMethodsPassive.TryPickRandomAction(npc, noEnemyNearbyAndNotWet, AIState, canSnooze, canWaterPlants, flag14, sitDownChance, waterPlantsChance);
                
                // Throw healing syringes to injured NPCs like the nurse can do
                if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[0] < 2f && npc.velocity.Y == 0f && canThrowHealingSyringes && npc.breath > 0)
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
