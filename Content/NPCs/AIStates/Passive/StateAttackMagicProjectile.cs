using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateAttackMagicProjectile
    {
        public static void Update(NPC npc, bool enemyNearby, float num2, int num11, int num12, int num13)
        {
            int num55 = 0;
            int attackDamage = 0;
            float num57 = 0f;
            int attackDelay = 0;
            int num59 = 0;
            int maxValue3 = 0;
            float knockBack3 = 0f;
            float num60 = 0f;
            float num61 = NPCID.Sets.DangerDetectRange[npc.type];
            float num62 = 1f;
            float num63 = 0f;
            if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
            {
                npc.frameCounter = 0.0;
                npc.localAI[3] = 0f;
            }
            int num64 = -1;
            if (num11 == 1 && npc.spriteDirection == 1)
            {
                num64 = num13;
            }
            if (num11 == -1 && npc.spriteDirection == -1)
            {
                num64 = num12;
            }

            NPCLoader.TownNPCAttackStrength(npc, ref attackDamage, ref knockBack3);
            NPCLoader.TownNPCAttackCooldown(npc, ref num59, ref maxValue3);
            NPCLoader.TownNPCAttackProj(npc, ref num55, ref attackDelay);
            NPCLoader.TownNPCAttackProjSpeed(npc, ref num57, ref num60, ref num63);
            NPCLoader.TownNPCAttackMagic(npc, ref num62);
            if (Main.expertMode)
            {
                attackDamage = (int)((float)attackDamage * Main.GameModeInfo.TownNPCDamageMultiplier);
            }
            attackDamage = (int)((float)attackDamage * num2);
            npc.velocity.X *= 0.8f;
            npc.ai[1] -= 1f;
            npc.localAI[3] += 1f;
            if (npc.localAI[3] == (float)attackDelay && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 vec4 = Vector2.Zero;
                if (num64 != -1)
                {
                    vec4 = npc.DirectionTo(Main.npc[num64].Center + new Vector2(0f, (0f - num60) * MathHelper.Clamp(npc.Distance(Main.npc[num64].Center) / num61, 0f, 1f)));
                }
                if (vec4.HasNaNs() || Math.Sign(vec4.X) != npc.spriteDirection)
                {
                    vec4 = new Vector2(npc.spriteDirection, 0f);
                }
                vec4 *= num57;
                vec4 += Utils.RandomVector2(Main.rand, 0f - num63, num63);
                int num73 = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec4.X, vec4.Y, num55, attackDamage, knockBack3, Main.myPlayer);
                Main.projectile[num73].npcProj = true;
                Main.projectile[num73].noDropItem = true;
            }
            if (num62 > 0f)
            {
                Vector3 vector6 = npc.GetMagicAuraColor().ToVector3() * num62;
                Lighting.AddLight(npc.Center, vector6.X, vector6.Y, vector6.Z);
            }
            if (npc.ai[1] <= 0f)
            {
                npc.ai[0] = ((npc.localAI[2] == 8f && enemyNearby) ? 8 : 0);
                npc.ai[1] = num59 + Main.rand.Next(maxValue3);
                npc.ai[2] = 0f;
                npc.localAI[1] = (npc.localAI[3] = num59 / 2 + Main.rand.Next(maxValue3));
                npc.netUpdate = true;
            }
        }
    }
}
