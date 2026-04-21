using System;
using Terraria;

namespace ChangedSpecialMod.Content.NPCs
{
    public static class Animations
    {
        // Used by Winged Black Latex, Lion and Snep.
        // The white latex taur uses something similar with extra logic. Maybe it can be merged with this one
        public static void AnimRunner(NPC npc, int frameHeight, float animSpeed = 0.6f)
        {
            int frameNumber = npc.frame.Y / frameHeight;

            if (npc.IsABestiaryIconDummy)
            {
                animSpeed = 1;
                if (npc.velocity.X == 0f)
                {
                    frameNumber = 1;
                }
            }
            // Falling
            if (npc.velocity.Y < 0f)
            {
                frameNumber = 0;
            }
            // Jumping
            else if (npc.velocity.Y > 0f)
            {
                frameNumber = 0;
            }
            // Standing still
            else if (npc.velocity.X == 0f)
            {
                frameNumber = 0;
                npc.frameCounter = 0.0;
            }
            else
            {
                if (npc.HasValidTarget)
                    npc.spriteDirection = npc.direction;
                else
                    npc.spriteDirection = ((!(npc.velocity.X < 0f)) ? 1 : -1);
                
                if (npc.spriteDirection == Math.Sign(npc.velocity.X))
                    npc.frameCounter += Math.Abs(npc.velocity.X) * animSpeed;

                // Normal running animation
                if (npc.frameCounter > 8.0)
                {
                    npc.frameCounter = 0.0f;
                    frameNumber++;
                    if (frameNumber > 3)
                    {
                        frameNumber = 0;
                    }
                }
            }
            npc.frame.Y = frameNumber * frameHeight;
        }
    }
}
