using Terraria;
using static ChangedSpecialMod.Content.NPCs.AIStyles.AIPassive;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateMisc
    {
        public static void Update(NPC npc, States AIState, bool enemyNearby)
        {
            npc.velocity.X *= 0.8f;
            npc.ai[1] -= 1f;
            // Extend running time if enemy is still nearby
            if (AIState == States.RunFromEnemy && npc.ai[1] < 60f && enemyNearby)
            {
                npc.ai[1] = 180f;
                npc.netUpdate = true;
            }
            if (AIState == States.Sitting)
                StateSitting.Update(npc);
            // Switch back to normal
            if (npc.ai[1] <= 0f)
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
