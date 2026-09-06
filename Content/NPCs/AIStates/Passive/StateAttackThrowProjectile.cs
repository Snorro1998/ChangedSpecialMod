using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateAttackThrowProjectile
    {
        public static void Update(NPC npc, bool enemyNearby, float num2, int num11, int num12, int num13)
        {
            int npcProjectileType = 0;
            int num37 = 0;
            float knockBack = 0f;
            float num38 = 0f;
            int num39 = 0;
            int num40 = 0;
            int maxValue = 0;
            float num41 = 0f;
            float num42 = NPCID.Sets.DangerDetectRange[npc.type];
            float num43 = 0f;
            if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
            {
                npc.frameCounter = 0.0;
                npc.localAI[3] = 0f;
            }
            NPCLoader.TownNPCAttackStrength(npc, ref num37, ref knockBack);
            NPCLoader.TownNPCAttackCooldown(npc, ref num40, ref maxValue);
            NPCLoader.TownNPCAttackProj(npc, ref npcProjectileType, ref num39);
            NPCLoader.TownNPCAttackProjSpeed(npc, ref num38, ref num41, ref num43);
            if (Main.expertMode)
            {
                num37 = (int)((float)num37 * Main.GameModeInfo.TownNPCDamageMultiplier);
            }
            num37 = (int)((float)num37 * num2);
            npc.velocity.X *= 0.8f;
            npc.ai[1] -= 1f;
            npc.localAI[3] += 1f;
            if (npc.localAI[3] == (float)num39 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 vec = -Vector2.UnitY;
                if (num11 == 1 && npc.spriteDirection == 1 && num13 != -1)
                {
                    vec = npc.DirectionTo(Main.npc[num13].Center + new Vector2(0f, (0f - num41) * MathHelper.Clamp(npc.Distance(Main.npc[num13].Center) / num42, 0f, 1f)));
                }
                if (num11 == -1 && npc.spriteDirection == -1 && num12 != -1)
                {
                    vec = npc.DirectionTo(Main.npc[num12].Center + new Vector2(0f, (0f - num41) * MathHelper.Clamp(npc.Distance(Main.npc[num12].Center) / num42, 0f, 1f)));
                }
                if (vec.HasNaNs() || Math.Sign(vec.X) != npc.spriteDirection)
                {
                    vec = new Vector2(npc.spriteDirection, -1f);
                }
                vec *= num38;
                vec += Utils.RandomVector2(Main.rand, 0f - num43, num43);
                int num44 = 1000;
                num44 = ((npc.type == NPCID.Mechanic) ?
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec.X, vec.Y, npcProjectileType, num37, knockBack, Main.myPlayer, 0f, npc.whoAmI, npc.townNpcVariationIndex)
                    : ((npc.type != NPCID.SantaClaus) ? Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec.X, vec.Y, npcProjectileType, num37, knockBack, Main.myPlayer)
                    : Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec.X, vec.Y, npcProjectileType, num37, knockBack, Main.myPlayer, 0f, Main.rand.Next(5))));
                Main.projectile[num44].npcProj = true;
                Main.projectile[num44].noDropItem = true;
                if (npc.type == NPCID.Golfer)
                {
                    Main.projectile[num44].timeLeft = 480;
                }
            }
            if (npc.ai[1] <= 0f)
            {
                npc.ai[0] = ((npc.localAI[2] == 8f && enemyNearby) ? 8 : 0);
                npc.ai[1] = num40 + Main.rand.Next(maxValue);
                npc.ai[2] = 0f;
                npc.localAI[1] = (npc.localAI[3] = num40 / 2 + Main.rand.Next(maxValue));
                npc.netUpdate = true;
            }
        }
    }
}
