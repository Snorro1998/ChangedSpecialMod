using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class State15
    {
        public static void Update(NPC npc, bool enemyNearby, float num2, int num11, int num12, int num13)
        {
            int num74 = 0;
            int maxValue4 = 0;
            if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
            {
                npc.frameCounter = 0.0;
                npc.localAI[3] = 0f;
            }
            int num75 = 0;
            float num76 = 0f;
            int num77 = 0;
            int num78 = 0;
            if (num11 == 1)
            {
                _ = npc.spriteDirection;
            }
            if (num11 == -1)
            {
                _ = npc.spriteDirection;
            }

            NPCLoader.TownNPCAttackStrength(npc, ref num75, ref num76);
            NPCLoader.TownNPCAttackCooldown(npc, ref num74, ref maxValue4);
            NPCLoader.TownNPCAttackSwing(npc, ref num77, ref num78);
            if (Main.expertMode)
            {
                num75 = (int)((float)num75 * Main.GameModeInfo.TownNPCDamageMultiplier);
            }
            num75 = (int)((float)num75 * num2);
            npc.velocity.X *= 0.8f;
            npc.ai[1] -= 1f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Tuple<Vector2, float> swingStats = npc.GetSwingStats(NPCID.Sets.AttackTime[npc.type] * 2, (int)npc.ai[1], npc.spriteDirection, num77, num78);
                Rectangle itemRectangle = new Rectangle((int)swingStats.Item1.X, (int)swingStats.Item1.Y, num77, num78);
                if (npc.spriteDirection == -1)
                {
                    itemRectangle.X -= num77;
                }
                itemRectangle.Y -= num78;
                npc.TweakSwingStats(NPCID.Sets.AttackTime[npc.type] * 2, (int)npc.ai[1], npc.spriteDirection, ref itemRectangle);
                int myPlayer = Main.myPlayer;
                for (int num79 = 0; num79 < 200; num79++)
                {
                    NPC nPC2 = Main.npc[num79];
                    if (nPC2.active && nPC2.immune[myPlayer] == 0 && !nPC2.dontTakeDamage && !nPC2.friendly && nPC2.damage > 0 && itemRectangle.Intersects(nPC2.Hitbox) && (nPC2.noTileCollide || Collision.CanHit(npc.position, npc.width, npc.height, nPC2.position, nPC2.width, nPC2.height)))
                    {
                        var hit = new NPC.HitInfo();
                        hit.Damage = num75;
                        hit.Knockback = num76;
                        hit.HitDirection = npc.spriteDirection;
                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, num79, num75, num76, npc.spriteDirection);
                        }
                        nPC2.netUpdate = true;
                        nPC2.immune[myPlayer] = (int)npc.ai[1] + 2;
                    }
                }
            }
            if (npc.ai[1] <= 0f)
            {
                bool flag25 = false;
                if (enemyNearby)
                {
                    int num80 = -num11;
                    if (!Collision.CanHit(npc.Center, 0, 0, npc.Center + Vector2.UnitX * num80 * 32f, 0, 0) || npc.localAI[2] == 8f)
                    {
                        flag25 = true;
                    }
                    if (flag25)
                    {
                        int num81 = NPCID.Sets.AttackTime[npc.type];
                        int num82 = ((num11 == 1) ? num13 : num12);
                        int num83 = ((num11 == 1) ? num12 : num13);
                        if (num82 != -1 && !Collision.CanHit(npc.Center, 0, 0, Main.npc[num82].Center, 0, 0))
                        {
                            num82 = ((num83 == -1 || !Collision.CanHit(npc.Center, 0, 0, Main.npc[num83].Center, 0, 0)) ? (-1) : num83);
                        }
                        if (num82 != -1)
                        {
                            npc.ai[0] = 15f;
                            npc.ai[1] = num81;
                            npc.ai[2] = 0f;
                            npc.localAI[3] = 0f;
                            npc.direction = ((npc.position.X < Main.npc[num82].position.X) ? 1 : (-1));
                            npc.netUpdate = true;
                        }
                        else
                        {
                            flag25 = false;
                        }
                    }
                }
                if (!flag25)
                {
                    npc.ai[0] = ((npc.localAI[2] == 8f && enemyNearby) ? 8 : 0);
                    npc.ai[1] = num74 + Main.rand.Next(maxValue4);
                    npc.ai[2] = 0f;
                    npc.localAI[1] = (npc.localAI[3] = num74 / 2 + Main.rand.Next(maxValue4));
                    npc.netUpdate = true;
                }
            }
        }
    }
}
