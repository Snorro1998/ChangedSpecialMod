using ChangedSpecialMod.Content.NPCs.AIMethods;
using Terraria;
using Terraria.ID;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateIdle
    {
        public static void Update(NPC npc, bool shouldStayInside, bool talkingWithPlayer, bool isFrogOrYellowTownSlime, bool flag8, bool flag9, int floorX, int floorY, int num6, int num7, ref bool keepWalking)
        {
            if (npc.localAI[3] > 0f)
            {
                npc.localAI[3] -= 1f;
            }
            int num16 = 120;
            if (npc.type == NPCID.TownDog)
            {
                num16 = 60;
            }
            if ((isFrogOrYellowTownSlime || flag8) && npc.wet)
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 200 + Main.rand.Next(500, 700);
                npc.ai[2] = 0f;
                npc.localAI[3] = 0f;
                npc.netUpdate = true;
            }
            else if (shouldStayInside && !talkingWithPlayer && !NPCID.Sets.TownCritter[npc.type])
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (num6 == floorX && num7 == floorY)
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
                    // Switch to walking
                    else
                    {
                        if (num6 > floorX)
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
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (!talkingWithPlayer && NPCID.Sets.IsTownPet[npc.type] && npc.ai[1] >= 100f && npc.ai[1] <= 150f)
                    {
                        AIMethodsPassive.AttemptToPlayIdleAnimationsForPets(npc, num16);
                    }
                    // Reducing the counter for whatever he is doing, walking etc
                    if (npc.ai[1] > 0f)
                    {
                        npc.ai[1] -= 1f;
                    }
                    bool flag16 = true;
                    int tileX2 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                    int tileY2 = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                    bool currentlyDrowning2 = npc.wet && !flag9;

                    AIMethodsPassive.GetWalkPrediction(npc, num6, floorX, flag9, currentlyDrowning2, tileX2, tileY2, out bool keepwalking2, out var avoidFalling2);
                    keepWalking = keepwalking2;

                    if (npc.wet && !flag9)
                    {
                        bool currentlyDrowning3 = Collision.DrownCollision(npc.position, npc.width, npc.height, 1f, includeSlopes: true);
                        // Keep walking if drowning
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
                        // Switch to walk state
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
            if (Main.netMode != NetmodeID.MultiplayerClient && (!shouldStayInside || AIMethodsPassive.IsInAGoodRestingSpot(npc, num6, num7, floorX, floorY)))
            {
                if (num6 < floorX - 25 || num6 > floorX + 25)
                {
                    if (npc.localAI[3] == 0f)
                    {
                        if (num6 < floorX - 50 && npc.direction == -1)
                        {
                            npc.direction = 1;
                            npc.netUpdate = true;
                        }
                        else if (num6 > floorX + 50 && npc.direction == 1)
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
    }
}
