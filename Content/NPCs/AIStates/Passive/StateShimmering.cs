using Microsoft.Xna.Framework;
using Terraria;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateShimmering
    {
        public static void Update(NPC npc)
        {
            npc.velocity.X *= 0.8f;
            npc.ai[1] -= 1f;
            npc.localAI[3] += 1f;
            npc.direction = 1;
            npc.spriteDirection = 1;
            Vector3 vector7 = npc.GetMagicAuraColor().ToVector3();
            Lighting.AddLight(npc.Center, vector7.X, vector7.Y, vector7.Z);
            if (npc.ai[1] <= 0f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 480f;
                npc.ai[2] = 0f;
                npc.localAI[1] = 480f;
                npc.netUpdate = true;
            }
        }
    }
}
