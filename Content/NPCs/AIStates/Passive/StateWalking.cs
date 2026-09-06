using ChangedSpecialMod.Content.NPCs.AIMethods;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateWalking
    {
        public static void Update(NPC npc, bool shouldStayInside, int floorX, int floorY, bool enemyNearby, bool flag9, bool flag11, bool flag12, bool isFrogOrYellowTownSlime, int num6, int num7)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && shouldStayInside && AIMethodsPassive.IsInAGoodRestingSpot(npc, num6, num7, floorX, floorY) && !NPCID.Sets.TownCritter[npc.type])
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 200 + Main.rand.Next(200);
                npc.localAI[3] = 60f;
                npc.netUpdate = true;
            }
            else
            {
                bool flag17 = !flag9 && Collision.DrownCollision(npc.position, npc.width, npc.height, 1f, includeSlopes: true);
                if (!flag17)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient && !npc.homeless && !Main.tileDungeon[Main.tile[num6, num7].TileType] && (num6 < floorX - 35 || num6 > floorX + 35))
                    {
                        if (npc.position.X < (float)(floorX * 16) && npc.direction == -1)
                        {
                            npc.ai[1] -= 5f;
                        }
                        else if (npc.position.X > (float)(floorX * 16) && npc.direction == 1)
                        {
                            npc.ai[1] -= 5f;
                        }
                    }
                    npc.ai[1] -= 1f;
                }
                if (npc.ai[1] <= 0f)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 300 + Main.rand.Next(300);
                    npc.ai[2] = 0f;
                    if (NPCID.Sets.TownCritter[npc.type])
                    {
                        npc.ai[1] -= Main.rand.Next(100);
                    }
                    else
                    {
                        npc.ai[1] += Main.rand.Next(900);
                    }
                    npc.localAI[3] = 60f;
                    npc.netUpdate = true;
                }
                if (npc.closeDoor && ((npc.position.X + (float)(npc.width / 2)) / 16f > (float)(npc.doorX + 2) || (npc.position.X + (float)(npc.width / 2)) / 16f < (float)(npc.doorX - 2)))
                {
                    Tile tileSafely = Framing.GetTileSafely(npc.doorX, npc.doorY);
                    if (TileLoader.CloseDoorID(tileSafely) >= 0)
                    {
                        if (WorldGen.CloseDoor(npc.doorX, npc.doorY))
                        {
                            npc.closeDoor = false;
                            NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 1, npc.doorX, npc.doorY, npc.direction);
                        }
                        if ((npc.position.X + (float)(npc.width / 2)) / 16f > (float)(npc.doorX + 4) || (npc.position.X + (float)(npc.width / 2)) / 16f < (float)(npc.doorX - 4) || (npc.position.Y + (float)(npc.height / 2)) / 16f > (float)(npc.doorY + 4) || (npc.position.Y + (float)(npc.height / 2)) / 16f < (float)(npc.doorY - 4))
                        {
                            npc.closeDoor = false;
                        }
                    }
                    else if (tileSafely.TileType == TileID.TallGateOpen)
                    {
                        if (WorldGen.ShiftTallGate(npc.doorX, npc.doorY, closing: true))
                        {
                            npc.closeDoor = false;
                            NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 5, npc.doorX, npc.doorY);
                        }
                        if ((npc.position.X + (float)(npc.width / 2)) / 16f > (float)(npc.doorX + 4) || (npc.position.X + (float)(npc.width / 2)) / 16f < (float)(npc.doorX - 4) || (npc.position.Y + (float)(npc.height / 2)) / 16f > (float)(npc.doorY + 4) || (npc.position.Y + (float)(npc.height / 2)) / 16f < (float)(npc.doorY - 4))
                        {
                            npc.closeDoor = false;
                        }
                    }
                    else
                    {
                        npc.closeDoor = false;
                    }
                }
                float movementSpeed = 1f;
                float acceleration = 0.07f;

                if (npc.friendly && (enemyNearby || flag17))
                {
                    movementSpeed = 1.5f;
                    float num19 = 1f - (float)npc.life / (float)npc.lifeMax;
                    movementSpeed += num19 * 0.9f;
                    acceleration = 0.1f;
                }
                if (flag11 && npc.wet)
                {
                    movementSpeed = 2f;
                    acceleration = 0.2f;
                }
                if (isFrogOrYellowTownSlime && npc.wet)
                {
                    if (Math.Abs(npc.velocity.X) < 0.05f && Math.Abs(npc.velocity.Y) < 0.05f)
                    {
                        npc.velocity.X += movementSpeed * 10f * (float)npc.direction;
                    }
                    else
                    {
                        npc.velocity.X *= 0.9f;
                    }
                }
                else if (npc.velocity.X < 0f - movementSpeed || npc.velocity.X > movementSpeed)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= 0.8f;
                    }
                }
                else if (npc.velocity.X < movementSpeed && npc.direction == 1)
                {
                    npc.velocity.X += acceleration;
                    if (npc.velocity.X > movementSpeed)
                    {
                        npc.velocity.X = movementSpeed;
                    }
                }
                else if (npc.velocity.X > 0f - movementSpeed && npc.direction == -1)
                {
                    npc.velocity.X -= acceleration;
                    if (npc.velocity.X > movementSpeed)
                    {
                        npc.velocity.X = movementSpeed;
                    }
                }
                bool flag18 = true;
                if ((float)(npc.homeTileY * 16 - 32) > npc.position.Y)
                {
                    flag18 = false;
                }
                if (!flag18 && npc.velocity.Y == 0f)
                {
                    Collision.StepDown(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY);
                }
                if (npc.velocity.Y >= 0f)
                {
                    Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, flag18, 1);
                }
                if (npc.velocity.Y == 0f)
                {
                    int num20 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                    int num21 = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                    int num22 = 180;
                    AIMethodsPassive.GetWalkPrediction(npc, num6, floorX, flag9, flag17, num20, num21, out var keepwalking3, out var avoidFalling3);
                    bool flag19 = false;
                    bool flag20 = false;
                    if (npc.wet && !flag9 && npc.townNPC && (flag20 = AIMethodsPassive.CheckIfWillDrown(flag17)) && npc.localAI[3] <= 0f)
                    {
                        avoidFalling3 = true;
                        npc.localAI[3] = num22;
                        int num23 = 0;
                        for (int n = 0; n <= 10 && Framing.GetTileSafely(num20 - npc.direction, num21 - n).LiquidAmount != 0; n++)
                        {
                            num23++;
                        }
                        float num24 = 0.3f;
                        float num25 = (float)Math.Sqrt((float)(num23 * 16 + 16) * 2f * num24);
                        if (num25 > 26f)
                        {
                            num25 = 26f;
                        }
                        npc.velocity.Y = 0f - num25;
                        npc.localAI[3] = npc.position.X;
                        flag19 = true;
                    }
                    if (avoidFalling3 && !flag19)
                    {
                        int num26 = (int)((npc.position.X + (float)(npc.width / 2)) / 16f);
                        int num27 = 0;
                        for (int num28 = -1; num28 <= 1; num28++)
                        {
                            Tile tileSafely2 = Framing.GetTileSafely(num26 + num28, num21 + 1);
                            if (tileSafely2.HasUnactuatedTile && Main.tileSolid[tileSafely2.TileType])
                            {
                                num27++;
                            }
                        }
                        if (num27 <= 2)
                        {
                            if (npc.velocity.X != 0f)
                            {
                                npc.netUpdate = true;
                            }
                            keepwalking3 = (avoidFalling3 = false);
                            npc.ai[0] = 0f;
                            npc.ai[1] = 50 + Main.rand.Next(50);
                            npc.ai[2] = 0f;
                            npc.localAI[3] = 40f;
                        }
                    }
                    if (npc.position.X == npc.localAI[3] && !flag19)
                    {
                        npc.direction *= -1;
                        npc.netUpdate = true;
                        npc.localAI[3] = num22;
                    }
                    if (flag17 && !flag19)
                    {
                        if (npc.localAI[3] > (float)num22)
                        {
                            npc.localAI[3] = num22;
                        }
                        if (npc.localAI[3] > 0f)
                        {
                            npc.localAI[3] -= 1f;
                        }
                    }
                    else
                    {
                        npc.localAI[3] = -1f;
                    }
                    Tile tileSafely3 = Framing.GetTileSafely(num20, num21);
                    Tile tileSafely4 = Framing.GetTileSafely(num20, num21 - 1);
                    Tile tileSafely5 = Framing.GetTileSafely(num20, num21 - 2);
                    bool flag21 = npc.height / 16 < 3;
                    if ((npc.townNPC || NPCID.Sets.AllowDoorInteraction[npc.type]) && tileSafely5.HasUnactuatedTile && (TileLoader.IsClosedDoor(tileSafely5) || tileSafely5.TileType == TileID.TallGateClosed) && (Main.rand.Next(10) == 0 || shouldStayInside))
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (WorldGen.OpenDoor(num20, num21 - 2, npc.direction))
                            {
                                npc.closeDoor = true;
                                npc.doorX = num20;
                                npc.doorY = num21 - 2;
                                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num20, num21 - 2, npc.direction);
                                npc.netUpdate = true;
                                npc.ai[1] += 80f;
                            }
                            else if (WorldGen.OpenDoor(num20, num21 - 2, -npc.direction))
                            {
                                npc.closeDoor = true;
                                npc.doorX = num20;
                                npc.doorY = num21 - 2;
                                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num20, num21 - 2, -npc.direction);
                                npc.netUpdate = true;
                                npc.ai[1] += 80f;
                            }
                            else if (WorldGen.ShiftTallGate(num20, num21 - 2, closing: false))
                            {
                                npc.closeDoor = true;
                                npc.doorX = num20;
                                npc.doorY = num21 - 2;
                                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, num20, num21 - 2);
                                npc.netUpdate = true;
                                npc.ai[1] += 80f;
                            }
                            else
                            {
                                npc.direction *= -1;
                                npc.netUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        if ((npc.velocity.X < 0f && npc.direction == -1) || (npc.velocity.X > 0f && npc.direction == 1))
                        {
                            bool flag22 = false;
                            bool runFromEnemy = false;
                            if (tileSafely5.HasUnactuatedTile && Main.tileSolid[tileSafely5.TileType] && !Main.tileSolidTop[tileSafely5.TileType] && (!flag21 || (tileSafely4.HasUnactuatedTile && Main.tileSolid[tileSafely4.TileType] && !Main.tileSolidTop[tileSafely4.TileType])))
                            {
                                if (!Collision.SolidTilesVersatile(num20 - npc.direction * 2, num20 - npc.direction, num21 - 5, num21 - 1) && !Collision.SolidTiles(num20, num20, num21 - 5, num21 - 3))
                                {
                                    npc.velocity.Y = -6f;
                                    npc.netUpdate = true;
                                }
                                else if (enemyNearby)
                                {
                                    runFromEnemy = true;
                                    flag22 = true;
                                }
                                else if (!flag20)
                                {
                                    flag22 = true;
                                }
                            }
                            else if (tileSafely4.HasUnactuatedTile && Main.tileSolid[tileSafely4.TileType] && !Main.tileSolidTop[tileSafely4.TileType])
                            {
                                if (!Collision.SolidTilesVersatile(num20 - npc.direction * 2, num20 - npc.direction, num21 - 4, num21 - 1) && !Collision.SolidTiles(num20, num20, num21 - 4, num21 - 2))
                                {
                                    npc.velocity.Y = -5f;
                                    npc.netUpdate = true;
                                }
                                else if (enemyNearby)
                                {
                                    runFromEnemy = true;
                                    flag22 = true;
                                }
                                else
                                {
                                    flag22 = true;
                                }
                            }
                            else if (npc.position.Y + (float)npc.height - (float)(num21 * 16) > 20f && tileSafely3.HasUnactuatedTile && Main.tileSolid[tileSafely3.TileType] && !tileSafely3.TopSlope)
                            {
                                if (!Collision.SolidTilesVersatile(num20 - npc.direction * 2, num20, num21 - 3, num21 - 1))
                                {
                                    npc.velocity.Y = -4.4f;
                                    npc.netUpdate = true;
                                }
                                else if (enemyNearby)
                                {
                                    runFromEnemy = true;
                                    flag22 = true;
                                }
                                else
                                {
                                    flag22 = true;
                                }
                            }
                            else if (avoidFalling3)
                            {
                                if (!flag20)
                                {
                                    flag22 = true;
                                }
                                if (enemyNearby)
                                {
                                    runFromEnemy = true;
                                }
                            }
                            else if (flag12 && !Collision.SolidTilesVersatile(num20 - npc.direction * 2, num20 - npc.direction, num21 - 2, num21 - 1))
                            {
                                npc.velocity.Y = -5f;
                                npc.netUpdate = true;
                            }
                            if (runFromEnemy)
                            {
                                keepwalking3 = false;
                                npc.velocity.X = 0f;
                                npc.ai[0] = 8f;
                                npc.ai[1] = 240f;
                                npc.netUpdate = true;
                            }
                            if (flag22)
                            {
                                npc.direction *= -1;
                                npc.velocity.X *= -1f;
                                npc.netUpdate = true;
                            }
                            if (keepwalking3)
                            {
                                npc.ai[1] = 90f;
                                npc.netUpdate = true;
                            }
                            if (npc.velocity.Y < 0f)
                            {
                                npc.localAI[3] = npc.position.X;
                            }
                        }
                        if (npc.velocity.Y < 0f && npc.wet)
                        {
                            npc.velocity.Y *= 1.2f;
                        }
                        if (npc.velocity.Y < 0f && NPCID.Sets.TownCritter[npc.type])
                        {
                            npc.velocity.Y *= 1.2f;
                        }
                    }
                }
                else if (flag12 && !npc.wet)
                {
                    int num29 = (int)(npc.Center.X / 16f);
                    int num30 = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                    int num31 = 0;
                    for (int num32 = -1; num32 <= 1; num32++)
                    {
                        for (int num33 = 1; num33 <= 6; num33++)
                        {
                            Tile tileSafely6 = Framing.GetTileSafely(num29 + num32, num30 + num33);
                            if (tileSafely6.LiquidAmount > 0 || (tileSafely6.HasUnactuatedTile && Main.tileSolid[tileSafely6.TileType]))
                            {
                                num31++;
                            }
                        }
                    }
                    if (num31 <= 2)
                    {
                        if (npc.velocity.X != 0f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.velocity.X *= 0.2f;
                        npc.ai[0] = 0f;
                        npc.ai[1] = 50 + Main.rand.Next(50);
                        npc.ai[2] = 0f;
                        npc.localAI[3] = 40f;
                    }
                }
            }
        }
    }
}
