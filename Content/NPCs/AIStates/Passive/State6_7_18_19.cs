using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class State6_7_18_19
    {
        public static void Update(NPC npc)
        {
            if (npc.ai[0] == 18f && (npc.localAI[3] < 1f || npc.localAI[3] > 2f))
            {
                npc.localAI[3] = 2f;
            }
            npc.velocity.X *= 0.8f;
            npc.ai[1] -= 1f;
            int num34 = (int)npc.ai[2];
            if (num34 < 0 || num34 > 255 || !Main.player[num34].CanBeTalkedTo || Main.player[num34].Distance(npc.Center) > 200f || !Collision.CanHitLine(npc.Top, 0, 0, Main.player[num34].Top, 0, 0))
            {
                npc.ai[1] = 0f;
            }
            if (npc.ai[1] > 0f)
            {
                int num35 = ((npc.Center.X < Main.player[num34].Center.X) ? 1 : (-1));
                if (num35 != npc.direction)
                {
                    npc.netUpdate = true;
                }
                npc.direction = num35;
            }
            else
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 60 + Main.rand.Next(60);
                npc.ai[2] = 0f;
                npc.localAI[3] = 30 + Main.rand.Next(60);
                npc.netUpdate = true;
            }
        }
    }
}
