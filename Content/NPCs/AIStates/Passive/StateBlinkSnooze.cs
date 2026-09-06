using Terraria;
using Terraria.ID;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateBlinkSnooze
    {
        public static void Update(NPC npc)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.localAI[3] -= 1f;
                if (Main.rand.Next(60) == 0 && npc.localAI[3] == 0f)
                {
                    npc.localAI[3] = 60f;
                    npc.direction *= -1;
                    npc.netUpdate = true;
                }
            }
            npc.ai[1] -= 1f;
            npc.velocity.X *= 0.8f;
            if (npc.ai[1] <= 0f)
            {
                npc.localAI[3] = 40f;
                npc.ai[0] = 0f;
                npc.ai[1] = 60 + Main.rand.Next(60);
                npc.netUpdate = true;
            }
        }
    }
}
