using System;
using Terraria;
using Microsoft.Xna.Framework;

namespace ChangedSpecialMod.Content.NPCs.AIStyles
{
    // Cleaned up version of vanilla Jellyfish AI, with lighting effects and other unnecessary parts removed
    public static class AIJellyFish
    {
        public static void Update(NPC npc)
        {
            bool flag12 = false;
            if (npc.wet && npc.ai[1] == 1f)
            {
                flag12 = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }
            float num271 = 1f;
            if (flag12)
            {
                num271 += 0.5f;
            }
            if (npc.direction == 0)
            {
                npc.TargetClosest();
            }
            if (flag12)
            {
                return;
            }
            if (npc.wet)
            {
                int num272 = (int)npc.Center.X / 16;
                int num273 = (int)(npc.position.Y + (float)npc.height) / 16;
                if (Main.tile[num272, num273].TopSlope)
                {
                    if (Main.tile[num272, num273].LeftSlope)
                    {
                        npc.direction = -1;
                        npc.velocity.X = Math.Abs(npc.velocity.X) * -1f;
                    }
                    else
                    {
                        npc.direction = 1;
                        npc.velocity.X = Math.Abs(npc.velocity.X);
                    }
                }
                else if (Main.tile[num272, num273 + 1].TopSlope)
                {
                    if (Main.tile[num272, num273 + 1].LeftSlope)
                    {
                        npc.direction = -1;
                        npc.velocity.X = Math.Abs(npc.velocity.X) * -1f;
                    }
                    else
                    {
                        npc.direction = 1;
                        npc.velocity.X = Math.Abs(npc.velocity.X);
                    }
                }
                if (npc.collideX)
                {
                    npc.velocity.X *= -1f;
                    npc.direction *= -1;
                }
                if (npc.collideY)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y = Math.Abs(npc.velocity.Y) * -1f;
                        npc.directionY = -1;
                        npc.ai[0] = -1f;
                    }
                    else if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y = Math.Abs(npc.velocity.Y);
                        npc.directionY = 1;
                        npc.ai[0] = 1f;
                    }
                }
                bool flag13 = false;
                if (!npc.friendly)
                {
                    npc.TargetClosest(faceTarget: false);
                    if (Main.player[npc.target].wet && !Main.player[npc.target].dead && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    {
                        flag13 = true;
                    }
                }
                if (flag13)
                {
                    npc.localAI[2] = 1f;
                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                    npc.velocity *= 0.98f;
                    float num274 = 0.2f;
                    if (npc.velocity.X > 0f - num274 && npc.velocity.X < num274 && npc.velocity.Y > 0f - num274 && npc.velocity.Y < num274)
                    {
                        npc.TargetClosest();
                        float num275 = 7f;
                        Vector2 vector31 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float num276 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector31.X;
                        float num277 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector31.Y;
                        float num278 = (float)Math.Sqrt(num276 * num276 + num277 * num277);
                        num278 = num275 / num278;
                        num276 *= num278;
                        num277 *= num278;
                        npc.velocity.X = num276;
                        npc.velocity.Y = num277;
                    }
                    return;
                }
                npc.localAI[2] = 0f;
                npc.velocity.X += (float)npc.direction * 0.02f;
                npc.rotation = npc.velocity.X * 0.4f;
                if (npc.velocity.X < -1f || npc.velocity.X > 1f)
                {
                    npc.velocity.X *= 0.95f;
                }
                if (npc.ai[0] == -1f)
                {
                    npc.velocity.Y -= 0.01f;
                    if (npc.velocity.Y < -1f)
                    {
                        npc.ai[0] = 1f;
                    }
                }
                else
                {
                    npc.velocity.Y += 0.01f;
                    if (npc.velocity.Y > 1f)
                    {
                        npc.ai[0] = -1f;
                    }
                }
                int num279 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
                int num280 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;

                if (Main.tile[num279, num280 - 1].LiquidAmount > 128)
                {
                    if (Main.tile[num279, num280 + 1].HasTile)
                    {
                        npc.ai[0] = -1f;
                    }
                    else if (Main.tile[num279, num280 + 2].HasTile)
                    {
                        npc.ai[0] = -1f;
                    }
                }
                else
                {
                    npc.ai[0] = 1f;
                }
                if ((double)npc.velocity.Y > 1.2 || (double)npc.velocity.Y < -1.2)
                {
                    npc.velocity.Y *= 0.99f;
                }
                return;
            }
            npc.rotation += npc.velocity.X * 0.1f;
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X *= 0.98f;
                if ((double)npc.velocity.X > -0.01 && (double)npc.velocity.X < 0.01)
                {
                    npc.velocity.X = 0f;
                }
            }
            npc.velocity.Y += 0.2f;
            if (npc.velocity.Y > 10f)
            {
                npc.velocity.Y = 10f;
            }
            npc.ai[0] = 1f;
            return;
        }
    }
}
