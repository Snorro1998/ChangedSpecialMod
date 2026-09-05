using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    public static class AI_Fighter
    {
        public static void AI_003_Fighter(NPC npc)
        {
            // Set this to true to debug which code is executed. It will rapidly flip the sprite
            bool testflag = false;

            // Check which npc it is
            var isBlackCub = npc.type == ModContent.NPCType<DarkLatexCub>();
            var isWhiteCub = npc.type == ModContent.NPCType<WhiteLatexCub>();
            var isFennec = npc.type == ModContent.NPCType<Fennec>();
            var isSnake = npc.type == ModContent.NPCType<Snek>();
            var isExoSuitRobot = npc.type == ModContent.NPCType<ExoSuitRobot>();

            if (Main.player[npc.target].position.Y + (float)Main.player[npc.target].height == npc.position.Y + (float)npc.height)
            {
                npc.directionY = -1;
            }

            bool flag = false;
            bool flag5 = false;

            bool flag6 = false;
            if (npc.velocity.X == 0f)
            {
                flag6 = true;
            }
            if (npc.justHit)
            {
                flag6 = false;
            }

            int num56 = 60;
            bool flag7 = false;
            bool flag8 = true;
            if (isExoSuitRobot)
            {
                flag8 = false;
            }
            if (npc.velocity.Y == 0f && ((npc.velocity.X > 0f && npc.direction < 0) || (npc.velocity.X < 0f && npc.direction > 0)))
            {
                flag7 = true;
            }
            if (npc.position.X == npc.oldPosition.X || npc.ai[3] >= (float)num56 || flag7)
            {
                npc.ai[3] += 1f;
            }
            else if ((double)Math.Abs(npc.velocity.X) > 0.9 && npc.ai[3] > 0f)
            {
                npc.ai[3] -= 1f;
            }
            if (npc.ai[3] > (float)(num56 * 10))
            {
                npc.ai[3] = 0f;
            }
            if (npc.justHit)
            {
                npc.ai[3] = 0f;
            }
            if (npc.ai[3] == (float)num56)
            {
                npc.netUpdate = true;
            }
            if (Main.player[npc.target].Hitbox.Intersects(npc.Hitbox))
            {
                npc.ai[3] = 0f;
            }
            if (npc.ai[3] < (float)num56 /*&& NPC.DespawnEncouragement_AIStyle3_Fighters_NotDiscouraged(npc.type, npc.position, npc)*/)
            {
                npc.TargetClosest();
                if (npc.directionY > 0 && Main.player[npc.target].Center.Y <= npc.Bottom.Y)
                {
                    npc.directionY = -1;
                }
            }
            else if (!(npc.ai[2] > 0f) || !NPC.DespawnEncouragement_AIStyle3_Fighters_CanBeBusyWithAction(npc.type))
            {
                if (Main.IsItDay() && (double)(npc.position.Y / 16f) < Main.worldSurface)
                {
                    npc.EncourageDespawn(10);
                }
                if (npc.velocity.X == 0f)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.ai[0] += 1f;
                        if (npc.ai[0] >= 2f)
                        {
                            npc.direction *= -1;
                            npc.spriteDirection = npc.direction;
                            npc.ai[0] = 0f;
                        }
                    }
                }
                else
                {
                    npc.ai[0] = 0f;
                }
                if (npc.direction == 0)
                {
                    npc.direction = 1;
                }
            }
            if (isBlackCub || isWhiteCub || isFennec || isSnake)
            {
                float num80 = 1.5f;
                // Make them much faster if the target player is drunk
                if (isSnake || npc.HasValidTarget && ChangedUtils.IsDrunk(Main.player[npc.target]))
                {
                    num80 = 4f;
                }
                if (npc.velocity.X < 0f - num80 || npc.velocity.X > num80)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= 0.8f;
                    }
                }
                else if (npc.velocity.X < num80 && npc.direction == 1)
                {
                    npc.velocity.X += 0.07f;
                    if (npc.velocity.X > num80)
                    {
                        npc.velocity.X = num80;
                    }
                }
                else if (npc.velocity.X > 0f - num80 && npc.direction == -1)
                {
                    npc.velocity.X -= 0.07f;
                    if (npc.velocity.X < 0f - num80)
                    {
                        npc.velocity.X = 0f - num80;
                    }
                }
            }
            // Same as eyezor in vanilla
            else if (isExoSuitRobot)
            {
                float num89 = 1f;
                float num90 = 0.08f;
                num89 += (1f - (float)npc.life / (float)npc.lifeMax) * 2f;
                num90 += (1f - (float)npc.life / (float)npc.lifeMax) * 0.2f;
                if (npc.velocity.X < 0f - num89 || npc.velocity.X > num89)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= 0.7f;
                    }
                }
                else if (npc.velocity.X < num89 && npc.direction == 1)
                {
                    npc.velocity.X += num90;
                    if (npc.velocity.X > num89)
                    {
                        npc.velocity.X = num89;
                    }
                }
                else if (npc.velocity.X > 0f - num89 && npc.direction == -1)
                {
                    npc.velocity.X -= num90;
                    if (npc.velocity.X < 0f - num89)
                    {
                        npc.velocity.X = 0f - num89;
                    }
                }
            }
            if (isExoSuitRobot)
            {
                if (npc.justHit)
                {
                    npc.ai[2] -= Main.rand.Next(30);
                }
                if (npc.ai[2] < 0f)
                {
                    npc.ai[2] = 0f;
                }
                if (npc.confused)
                {
                    npc.ai[2] = 0f;
                }
                npc.ai[2] += 1f;
                float num138 = Main.rand.Next(60, 1800);
                num138 *= (float)npc.life / (float)npc.lifeMax;
                num138 += 15f;
                if (Main.netMode != 1 && npc.ai[2] >= num138 && npc.velocity.Y == 0f && !Main.player[npc.target].dead && !Main.player[npc.target].frozen && ((npc.direction > 0 && npc.Center.X < Main.player[npc.target].Center.X) || (npc.direction < 0 && npc.Center.X > Main.player[npc.target].Center.X)) && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    Vector2 vector34 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + 12f);
                    vector34.X += 6 * npc.direction;
                    float num139 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector34.X;
                    float num140 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector34.Y;
                    num139 += (float)Main.rand.Next(-40, 41);
                    num140 += (float)Main.rand.Next(-30, 0);
                    float num141 = (float)Math.Sqrt(num139 * num139 + num140 * num140);
                    npc.netUpdate = true;
                    num141 = 15f / num141;
                    num139 *= num141;
                    num140 *= num141;
                    int projectileDamage = npc.damage / 2;
                    int num143 = 83;
                    vector34.X += num139 * 3f;
                    vector34.Y += num140 * 3f;
                    Projectile.NewProjectile(npc.GetSource_FromAI(), vector34.X, vector34.Y, num139, num140, num143, projectileDamage, 0f, Main.myPlayer);
                    npc.ai[2] = 0f;
                }
            }
            if (npc.velocity.Y == 0f || flag)
            {
                int num181 = (int)(npc.position.Y + (float)npc.height + 7f) / 16;
                int num182 = (int)(npc.position.Y - 9f) / 16;
                int num183 = (int)npc.position.X / 16;
                int num184 = (int)(npc.position.X + (float)npc.width) / 16;
                int num185 = (int)(npc.position.X + 8f) / 16;
                int num186 = (int)(npc.position.X + (float)npc.width - 8f) / 16;
                bool flag22 = false;
                for (int num187 = num185; num187 <= num186; num187++)
                {
                    if (num187 >= num183 && num187 <= num184 && Main.tile[num187, num181] == null)
                    {
                        flag22 = true;
                        continue;
                    }
                    if (Main.tile[num187, num182] != null && Main.tile[num187, num182].HasUnactuatedTile && Main.tileSolid[Main.tile[num187, num182].TileType])
                    {
                        flag5 = false;
                        break;
                    }
                    if (!flag22 && num187 >= num183 && num187 <= num184 && Main.tile[num187, num181].HasUnactuatedTile && Main.tileSolid[Main.tile[num187, num181].TileType])
                    {
                        flag5 = true;
                    }
                }
                if (!flag5 && npc.velocity.Y < 0f)
                {
                    npc.velocity.Y = 0f;
                }
                if (flag22)
                {
                    return;
                }
            }
            if (npc.velocity.Y >= 0f && (npc.directionY != 1))
            {
                int num188 = 0;
                if (npc.velocity.X < 0f)
                {
                    num188 = -1;
                }
                if (npc.velocity.X > 0f)
                {
                    num188 = 1;
                }
                Vector2 vector39 = npc.position;
                vector39.X += npc.velocity.X;
                int num189 = (int)((vector39.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 1) * num188)) / 16f);
                int num190 = (int)((vector39.Y + (float)npc.height - 1f) / 16f);
                if (WorldGen.InWorld(num189, num190, 4))
                {
                    if ((float)(num189 * 16) < vector39.X + (float)npc.width && (float)(num189 * 16 + 16) > vector39.X && ((Main.tile[num189, num190].HasUnactuatedTile && !Main.tile[num189, num190].TopSlope && !Main.tile[num189, num190 - 1].TopSlope && Main.tileSolid[Main.tile[num189, num190].TileType] && !Main.tileSolidTop[Main.tile[num189, num190].TileType]) || (Main.tile[num189, num190 - 1].IsHalfBlock && Main.tile[num189, num190 - 1].HasUnactuatedTile)) && (!Main.tile[num189, num190 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189, num190 - 1].TileType] || Main.tileSolidTop[Main.tile[num189, num190 - 1].TileType] || (Main.tile[num189, num190 - 1].IsHalfBlock && (!Main.tile[num189, num190 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189, num190 - 4].TileType] || Main.tileSolidTop[Main.tile[num189, num190 - 4].TileType]))) && (!Main.tile[num189, num190 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189, num190 - 2].TileType] || Main.tileSolidTop[Main.tile[num189, num190 - 2].TileType]) && (!Main.tile[num189, num190 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189, num190 - 3].TileType] || Main.tileSolidTop[Main.tile[num189, num190 - 3].TileType]) && (!Main.tile[num189 - num188, num190 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189 - num188, num190 - 3].TileType]))
                    {
                        float num191 = num190 * 16;
                        if (Main.tile[num189, num190].IsHalfBlock)
                        {
                            num191 += 8f;
                        }
                        if (Main.tile[num189, num190 - 1].IsHalfBlock)
                        {
                            num191 -= 8f;
                        }
                        if (num191 < vector39.Y + (float)npc.height)
                        {
                            float num192 = vector39.Y + (float)npc.height - num191;
                            float num193 = 16.1f;
                            if (num192 <= num193)
                            {
                                npc.gfxOffY += npc.position.Y + (float)npc.height - num191;
                                npc.position.Y = num191 - (float)npc.height;
                                if (num192 < 9f)
                                {
                                    npc.stepSpeed = 1f;
                                }
                                else
                                {
                                    npc.stepSpeed = 2f;
                                }
                            }
                        }
                    }
                }
            }
            if (flag5)
            {
                int num194 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                int num195 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);

                if (isSnake)
                {
                    num194 = (int)((npc.position.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 16) * npc.direction)) / 16f);
                }

                if (Main.tile[num194, num195 - 1].HasUnactuatedTile && (TileLoader.IsClosedDoor(Main.tile[num194, num195 - 1]) || Main.tile[num194, num195 - 1].TileType == 388) && flag8)
                {
                    npc.ai[2] += 1f;
                    npc.ai[3] = 0f;
                    if (npc.ai[2] >= 60f)
                    {
                        bool flag23 = npc.type == NPCID.Zombie || npc.type == NPCID.ArmedZombie || npc.type == NPCID.TorchZombie || npc.type == NPCID.ZombieXmas || npc.type == NPCID.ZombieSweater || npc.type == NPCID.BaldZombie || npc.type == NPCID.ZombieEskimo || npc.type == 186 || npc.type == 187 || npc.type == NPCID.SwampZombie || npc.type == 189 || npc.type == NPCID.FemaleZombie || npc.type == 223 || npc.type == 320 || npc.type == 321 || npc.type == 319 || npc.type == 21 || npc.type == 324 || npc.type == 323 || npc.type == 322 || npc.type == NPCID.UndeadMiner || npc.type == 196 || npc.type == 167 || npc.type == NPCID.ArmoredSkeleton || npc.type == 197 || npc.type == 202 || npc.type == 203 || npc.type == 449 || npc.type == 450 || npc.type == 451 || npc.type == 452 || npc.type == NPCID.GreekSkeleton || npc.type == NPCID.HeadacheSkeleton || npc.type == NPCID.SporeSkeleton;
                        bool flag24 = Main.player[npc.target].ZoneGraveyard && Main.rand.Next(60) == 0;
                        if ((!Main.bloodMoon || Main.getGoodWorld) && !flag24 && flag23)
                        {
                            npc.ai[1] = 0f;
                        }
                        npc.velocity.X = 0.5f * (float)(-npc.direction);
                        int num196 = 5;
                        if (Main.tile[num194, num195 - 1].TileType == TileID.TallGateClosed)
                        {
                            num196 = 2;
                        }
                        npc.ai[1] += num196;
                        npc.ai[2] = 0f;
                        bool flag25 = false;
                        if (npc.ai[1] >= 10f)
                        {
                            flag25 = true;
                            npc.ai[1] = 10f;
                        }
                        WorldGen.KillTile(num194, num195 - 1, fail: true);
                        if ((Main.netMode != NetmodeID.MultiplayerClient || !flag25) && flag25 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (TileLoader.IsClosedDoor(Main.tile[num194, num195 - 1]))
                            {
                                bool flag26 = WorldGen.OpenDoor(num194, num195 - 1, npc.direction);
                                if (!flag26)
                                {
                                    npc.ai[3] = num56;
                                    npc.netUpdate = true;
                                }
                                if (Main.netMode == NetmodeID.Server && flag26)
                                {
                                    NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num194, num195 - 1, npc.direction);
                                }
                            }
                            if (Main.tile[num194, num195 - 1].TileType == TileID.TallGateClosed)
                            {
                                bool flag27 = WorldGen.ShiftTallGate(num194, num195 - 1, closing: false);
                                if (!flag27)
                                {
                                    npc.ai[3] = num56;
                                    npc.netUpdate = true;
                                }
                                if (Main.netMode == NetmodeID.Server && flag27)
                                {
                                    NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, num194, num195 - 1);
                                }
                            }
                        }
                    }
                }
                else
                {
                    int num197 = npc.spriteDirection;
                    if ((npc.velocity.X < 0f && num197 == -1) || (npc.velocity.X > 0f && num197 == 1))
                    {
                        if (npc.height >= 32 && Main.tile[num194, num195 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num194, num195 - 2].TileType])
                        {
                            if (Main.tile[num194, num195 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num194, num195 - 3].TileType])
                            {
                                npc.velocity.Y = -8f;
                                npc.netUpdate = true;
                            }
                            else
                            {
                                npc.velocity.Y = -7f;
                                npc.netUpdate = true;
                            }
                        }
                        else if (Main.tile[num194, num195 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num194, num195 - 1].TileType])
                        {
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                        else if (npc.position.Y + (float)npc.height - (float)(num195 * 16) > 20f && Main.tile[num194, num195].HasUnactuatedTile && !Main.tile[num194, num195].TopSlope && Main.tileSolid[Main.tile[num194, num195].TileType])
                        {
                            npc.velocity.Y = -5f;
                            npc.netUpdate = true;
                        }
                        else if (npc.directionY < 0 && npc.type != NPCID.Crab && (!Main.tile[num194, num195 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num194, num195 + 1].TileType]) && (!Main.tile[num194 + npc.direction, num195 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num194 + npc.direction, num195 + 1].TileType]))
                        {
                            npc.velocity.Y = -8f;
                            npc.velocity.X *= 1.5f;
                            npc.netUpdate = true;
                        }
                        else if (flag8)
                        {
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                        }
                        if (npc.velocity.Y == 0f && flag6 && npc.ai[3] == 1f)
                        {
                            npc.velocity.Y = -5f;
                        }
                        if (npc.velocity.Y == 0f && Main.expertMode && Main.player[npc.target].Bottom.Y < npc.Top.Y && Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < (float)(Main.player[npc.target].width * 3) && Collision.CanHit(npc, Main.player[npc.target]))
                        {
                            if (npc.type == NPCID.ZombieMerman)
                            {
                                int num199 = (int)((npc.Bottom.Y - 16f - Main.player[npc.target].Bottom.Y) / 16f);
                                if (num199 < 14 && Collision.CanHit(npc, Main.player[npc.target]))
                                {
                                    if (num199 < 7)
                                    {
                                        npc.velocity.Y = -8.8f;
                                    }
                                    else if (num199 < 8)
                                    {
                                        npc.velocity.Y = -9.2f;
                                    }
                                    else if (num199 < 9)
                                    {
                                        npc.velocity.Y = -9.7f;
                                    }
                                    else if (num199 < 10)
                                    {
                                        npc.velocity.Y = -10.3f;
                                    }
                                    else if (num199 < 11)
                                    {
                                        npc.velocity.Y = -10.6f;
                                    }
                                    else
                                    {
                                        npc.velocity.Y = -11f;
                                    }
                                }
                            }
                            if (npc.velocity.Y == 0f)
                            {
                                int num200 = 6;
                                if (Main.player[npc.target].Bottom.Y > npc.Top.Y - (float)(num200 * 16))
                                {
                                    npc.velocity.Y = -7.9f;
                                }
                                else
                                {
                                    int num201 = (int)(npc.Center.X / 16f);
                                    int num202 = (int)(npc.Bottom.Y / 16f) - 1;
                                    for (int num203 = num202; num203 > num202 - num200; num203--)
                                    {
                                        if (Main.tile[num201, num203].HasUnactuatedTile && TileID.Sets.Platforms[Main.tile[num201, num203].TileType])
                                        {
                                            npc.velocity.Y = -7.9f;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (flag8)
            {
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
            }
            if (testflag)
                npc.spriteDirection *= -1;
        }
    }
}
