using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateAttackShoot
    {
        public static void Update(NPC npc, bool enemyNearby, float damageMultiplier, int num11, int num12, int num13)
        {
            int num45 = 0;
            int num46 = 0;
            float num47 = 0f;
            int num48 = 0;
            int num49 = 0;
            int maxValue2 = 0;
            float knockBack2 = 0f;
            float num50 = 0f;
            bool flag24 = false;
            float num51 = 0f;
            if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
            {
                npc.frameCounter = 0.0;
                npc.localAI[3] = 0f;
            }
            int num52 = -1;
            if (num11 == 1 && npc.spriteDirection == 1)
            {
                num52 = num13;
            }
            if (num11 == -1 && npc.spriteDirection == -1)
            {
                num52 = num12;
            }

            NPCLoader.TownNPCAttackStrength(npc, ref num46, ref knockBack2);
            NPCLoader.TownNPCAttackCooldown(npc, ref num49, ref maxValue2);
            NPCLoader.TownNPCAttackProj(npc, ref num45, ref num48);
            NPCLoader.TownNPCAttackProjSpeed(npc, ref num47, ref num50, ref num51);
            NPCLoader.TownNPCAttackShoot(npc, ref flag24);
            if (Main.expertMode)
            {
                num46 = (int)((float)num46 * Main.GameModeInfo.TownNPCDamageMultiplier);
            }
            num46 = (int)((float)num46 * damageMultiplier);
            npc.velocity.X *= 0.8f;
            npc.ai[1] -= 1f;
            npc.localAI[3] += 1f;
            if (npc.localAI[3] == (float)num48 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 vec2 = Vector2.Zero;
                if (num52 != -1)
                {
                    vec2 = npc.DirectionTo(Main.npc[num52].Center + new Vector2(0f, 0f - num50));
                }
                if (vec2.HasNaNs() || Math.Sign(vec2.X) != npc.spriteDirection)
                {
                    vec2 = new Vector2(npc.spriteDirection, 0f);
                }
                vec2 *= num47;
                vec2 += Utils.RandomVector2(Main.rand, 0f - num51, num51);
                int num53 = 1000;
                num53 = ((npc.type != NPCID.Painter) ? Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec2.X, vec2.Y, num45, num46, knockBack2, Main.myPlayer) : Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec2.X, vec2.Y, num45, num46, knockBack2, Main.myPlayer, 0f, (float)Main.rand.Next(12) / 6f));
                Main.projectile[num53].npcProj = true;
                Main.projectile[num53].noDropItem = true;
            }
            if (npc.localAI[3] == (float)num48 && flag24 && num52 != -1)
            {
                Vector2 vector2 = npc.DirectionTo(Main.npc[num52].Center);
                if (vector2.Y <= 0.5f && vector2.Y >= -0.5f)
                {
                    npc.ai[2] = vector2.Y;
                }
            }
            if (npc.ai[1] <= 0f)
            {
                npc.ai[0] = ((npc.localAI[2] == 8f && enemyNearby) ? 8 : 0);
                npc.ai[1] = num49 + Main.rand.Next(maxValue2);
                npc.ai[2] = 0f;
                npc.localAI[1] = (npc.localAI[3] = num49 / 2 + Main.rand.Next(maxValue2));
                npc.netUpdate = true;
            }
        }
    }
}
