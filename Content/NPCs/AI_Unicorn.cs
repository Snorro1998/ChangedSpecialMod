using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace ChangedSpecialMod.Content.NPCs
{
    public static class AI_Unicorn
    {
        public static void AI_026_Unicorns(NPC npc)
        {
            bool testflag = false;

            int num = 30;
            int num2 = 10;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            if (npc.velocity.Y == 0f && ((npc.velocity.X > 0f && npc.direction < 0) || (npc.velocity.X < 0f && npc.direction > 0)))
            {
                flag2 = true;
                npc.ai[3] += 1f;
            }
            if (npc.position.X == npc.oldPosition.X || npc.ai[3] >= (float)num || flag2)
            {
                npc.ai[3] += 1f;
                flag3 = true;
            }
            else if (npc.ai[3] > 0f)
            {
                npc.ai[3] -= 1f;
            }
            if (npc.ai[3] > (float)(num * num2))
            {
                npc.ai[3] = 0f;
            }
            if (npc.justHit)
            {
                npc.ai[3] = 0f;
            }
            if (npc.ai[3] == (float)num)
            {
                npc.netUpdate = true;
            }
            Vector2 vector3 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
            float num7 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector3.X;
            float num8 = Main.player[npc.target].position.Y - vector3.Y;
            float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
            if (num9 < 200f && !flag3)
            {
                npc.ai[3] = 0f;
            }
            if (npc.type == NPCID.Wolf || npc.type == NPCID.Hellhound)
            {
                if (npc.velocity.Y == 0f && num9 < 100f && Math.Abs(npc.velocity.X) > 3f && ((npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) && npc.velocity.X > 0f) || (npc.position.X + (float)(npc.width / 2) > Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) && npc.velocity.X < 0f)))
                {
                    npc.velocity.Y -= 4f;
                }
            }
            if (npc.ai[3] < (float)num)
            {
                npc.TargetClosest();
            }
            else
            {
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
                npc.directionY = -1;
                if (npc.direction == 0)
                {
                    npc.direction = 1;
                }
            }
            float num11 = 6f;
            float num12 = 0.07f;
            if (!flag && (npc.velocity.Y == 0f || npc.wet || (npc.velocity.X <= 0f && npc.direction < 0) || (npc.velocity.X >= 0f && npc.direction > 0)))
            {
                if (npc.type == 155)
                {
                    if (npc.velocity.X > 0f && npc.direction < 0)
                    {
                        npc.velocity.X *= 0.95f;
                    }
                    if (npc.velocity.X < 0f && npc.direction > 0)
                    {
                        npc.velocity.X *= 0.95f;
                    }
                }
                if (npc.velocity.X < 0f - num11 || npc.velocity.X > num11)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= 0.8f;
                    }
                }
                else if (npc.velocity.X < num11 && npc.direction == 1)
                {
                    npc.velocity.X += num12;
                    if (npc.velocity.X > num11)
                    {
                        npc.velocity.X = num11;
                    }
                }
                else if (npc.velocity.X > 0f - num11 && npc.direction == -1)
                {
                    npc.velocity.X -= num12;
                    if (npc.velocity.X < 0f - num11)
                    {
                        npc.velocity.X = 0f - num11;
                    }
                }
            }
            if (npc.velocity.Y >= 0f)
            {
                int num14 = 0;
                if (npc.velocity.X < 0f)
                {
                    num14 = -1;
                }
                if (npc.velocity.X > 0f)
                {
                    num14 = 1;
                }
                Vector2 vector8 = npc.position;
                vector8.X += npc.velocity.X;
                int num15 = (int)((vector8.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 1) * num14)) / 16f);
                int num16 = (int)((vector8.Y + (float)npc.height - 1f) / 16f);
                /*
                if (Main.tile[num15, num16] == null)
                {
                    Main.tile[num15, num16] = default(Tile);
                }
                if (Main.tile[num15, num16 - 1] == null)
                {
                    Main.tile[num15, num16 - 1] = default(Tile);
                }
                if (Main.tile[num15, num16 - 2] == null)
                {
                    Main.tile[num15, num16 - 2] = default(Tile);
                }
                if (Main.tile[num15, num16 - 3] == null)
                {
                    Main.tile[num15, num16 - 3] = default(Tile);
                }
                if (Main.tile[num15, num16 + 1] == null)
                {
                    Main.tile[num15, num16 + 1] = default(Tile);
                }
                */
                if ((float)(num15 * 16) < vector8.X + (float)npc.width && (float)(num15 * 16 + 16) > vector8.X && ((Main.tile[num15, num16].HasUnactuatedTile && !Main.tile[num15, num16].TopSlope && !Main.tile[num15, num16 - 1].TopSlope && Main.tileSolid[Main.tile[num15, num16].TileType] && !Main.tileSolidTop[Main.tile[num15, num16].TileType]) || (Main.tile[num15, num16 - 1].IsHalfBlock && Main.tile[num15, num16 - 1].HasUnactuatedTile)) && (!Main.tile[num15, num16 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num15, num16 - 1].TileType] || Main.tileSolidTop[Main.tile[num15, num16 - 1].TileType] || (Main.tile[num15, num16 - 1].IsHalfBlock && (!Main.tile[num15, num16 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num15, num16 - 4].TileType] || Main.tileSolidTop[Main.tile[num15, num16 - 4].TileType]))) && (!Main.tile[num15, num16 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num15, num16 - 2].TileType] || Main.tileSolidTop[Main.tile[num15, num16 - 2].TileType]) && (!Main.tile[num15, num16 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num15, num16 - 3].TileType] || Main.tileSolidTop[Main.tile[num15, num16 - 3].TileType]) && (!Main.tile[num15 - num14, num16 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num15 - num14, num16 - 3].TileType]))
                {
                    float num17 = num16 * 16;
                    if (Main.tile[num15, num16].IsHalfBlock)
                    {
                        num17 += 8f;
                    }
                    if (Main.tile[num15, num16 - 1].IsHalfBlock)
                    {
                        num17 -= 8f;
                    }
                    if (num17 < vector8.Y + (float)npc.height)
                    {
                        float num18 = vector8.Y + (float)npc.height - num17;
                        if ((double)num18 <= 16.1)
                        {
                            npc.gfxOffY += npc.position.Y + (float)npc.height - num17;
                            npc.position.Y = num17 - (float)npc.height;
                            if (num18 < 9f)
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
            if (npc.velocity.Y == 0f)
            {
                bool flag6 = true;
                int num19 = (int)(npc.position.Y - 7f) / 16;
                int num20 = (int)(npc.position.X - 7f) / 16;
                int num21 = (int)(npc.position.X + (float)npc.width + 7f) / 16;
                for (int m = num20; m <= num21; m++)
                {
                    if (
                        Main.tile[m, num19] != null 
                        && Main.tile[m, num19].HasUnactuatedTile 
                        && Main.tileSolid[Main.tile[m, num19].TileType])
                    {
                        flag6 = false;
                        break;
                    }
                }
                if (flag6)
                {
                    int num22 = (int)((npc.position.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 2) * npc.direction) + npc.velocity.X * 5f) / 16f);
                    int num23 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);

                    int varSpriteDirection = npc.spriteDirection;
                    
                    // Facing the same direction as moving in
                    if ((npc.velocity.X < 0f && varSpriteDirection == -1) || (npc.velocity.X > 0f && varSpriteDirection == 1))
                    {
                        // Forcing this to true fixes the problem.
                        bool flag7 = true;
                        float num25 = 3f;
                        if (Main.tile[num22, num23 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num22, num23 - 2].TileType])
                        {
                            if (Main.tile[num22, num23 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num22, num23 - 3].TileType])
                            {
                                npc.velocity.Y = -8.5f;
                                npc.netUpdate = true;
                            }
                            else
                            {
                                npc.velocity.Y = -7.5f;
                                npc.netUpdate = true;
                            }
                        }
                        else if (Main.tile[num22, num23 - 1].HasUnactuatedTile && !Main.tile[num22, num23 - 1].TopSlope && Main.tileSolid[Main.tile[num22, num23 - 1].TileType])
                        {
                            npc.velocity.Y = -7f;
                            npc.netUpdate = true;
                        }
                        else if (npc.position.Y + (float)npc.height - (float)(num23 * 16) > 20f && Main.tile[num22, num23].HasUnactuatedTile && !Main.tile[num22, num23].TopSlope && Main.tileSolid[Main.tile[num22, num23].TileType])
                        {
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                        else if 
                        (
                            (npc.directionY < 0 || Math.Abs(npc.velocity.X) > num25) 
                            && (!flag7 || !Main.tile[num22, num23 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num22, num23 + 1].TileType]) 
                            && (!Main.tile[num22, num23 + 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num22, num23 + 2].TileType]) 
                            && (!Main.tile[num22 + npc.direction, num23 + 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num22 + npc.direction, num23 + 3].TileType])
                        )
                        {
                            //testflag = true;
                            npc.velocity.Y = -8f;
                            npc.netUpdate = true;
                        }
                    }
                }
            }
            //if (testflag)
            //    npc.rotation = Main.rand.NextFloat((float)Math.PI * 2);
        }
    }
}
