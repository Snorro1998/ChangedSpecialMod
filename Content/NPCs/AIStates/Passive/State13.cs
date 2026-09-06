using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class State13
    {
        public static void Update(NPC npc)
        {
            npc.velocity.X *= 0.8f;
            if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
            {
                npc.frameCounter = 0.0;
            }
            npc.ai[1] -= 1f;
            npc.localAI[3] += 1f;
            if (npc.localAI[3] == 1f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 vec3 = npc.DirectionTo(Main.npc[(int)npc.ai[2]].Center + new Vector2(0f, -20f));
                if (vec3.HasNaNs() || Math.Sign(vec3.X) == -npc.spriteDirection)
                {
                    vec3 = new Vector2(npc.spriteDirection, -1f);
                }
                vec3 *= 8f;
                int num54 = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec3.X, vec3.Y, ProjectileID.NurseSyringeHeal, 0, 0f, Main.myPlayer, npc.ai[2]);
                Main.projectile[num54].npcProj = true;
                Main.projectile[num54].noDropItem = true;
            }
            if (npc.ai[1] <= 0f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 10 + Main.rand.Next(10);
                npc.ai[2] = 0f;
                npc.localAI[3] = 5 + Main.rand.Next(10);
                npc.netUpdate = true;
            }
        }
    }
}
