using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Mounts
{
    public class WhiteLatexTaurMount : ModMount
    {
        public override void SetStaticDefaults()
        {
            // Movement
            MountData.jumpHeight = 5;           // How high the mount can jump.
            MountData.acceleration = 0.1f;      //0.19 The rate at which the mount speeds up.
            MountData.jumpSpeed = 10f;          // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is pressed.
            MountData.blockExtraJumps = true;   // Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
            MountData.constantJump = true;      // Allows you to hold the jump button down.
            MountData.heightBoost = 20;         // Height between the mount and the ground
            MountData.fallDamage = 0.5f;        // Fall damage multiplier.
            MountData.runSpeed = 7f;            // The speed of the mount
            MountData.dashSpeed = 7f;           // The speed the mount moves when in the state of dashing.
            MountData.flightTimeMax = 0;        // The amount of time in frames a mount can be in the state of flying.

            // Misc
            MountData.fatigueMax = 0;
            MountData.buff = ModContent.BuffType<Buffs.WhiteLatexTaurMountBuff>();

            // Effects
            MountData.spawnDust = DustID.SnowSpray;

            // Frame data and player offsets
            MountData.totalFrames = 10;

            int baseYOffset = 22;
            MountData.playerYOffsets = Enumerable.Repeat(baseYOffset, MountData.totalFrames).ToArray();
            MountData.playerYOffsets[8] = baseYOffset + 2;
            MountData.playerYOffsets[4] = baseYOffset + 3;
            MountData.playerYOffsets[5] = baseYOffset + 4;
            MountData.playerYOffsets[7] = baseYOffset + 4;
            MountData.playerYOffsets[6] = baseYOffset + 5;

            MountData.xOffset = 7;
            //MountData.yOffset = -12;

            MountData.playerHeadOffset = 22;
            MountData.bodyFrame = 3;
            
            // Standing
            MountData.standingFrameCount = 1;
            MountData.standingFrameDelay = 12;
            MountData.standingFrameStart = 0;
            
            // Running
            MountData.runningFrameCount = 7;
            MountData.runningFrameDelay = 12;
            MountData.runningFrameStart = 2;
            
            // Flying
            MountData.flyingFrameCount = 1;
            MountData.flyingFrameDelay = 12;
            MountData.flyingFrameStart = 1;
            
            // In-air
            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 12;
            MountData.inAirFrameStart = 1;
            
            // Idle
            MountData.idleFrameCount = 1;
            MountData.idleFrameDelay = 12;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;
            
            // Swimming
            MountData.swimFrameCount = 7;
            MountData.swimFrameDelay = 12;
            MountData.swimFrameStart = 0;

            if (Main.netMode != NetmodeID.Server)
            {
                MountData.textureWidth = MountData.backTexture.Width();
                MountData.textureHeight = MountData.backTexture.Height();
            }
        }

        public override void SetMount(Player player, ref bool skipDust)
        {
            if (Main.hardMode)
                SetStatsHardmode();
            else
                SetStatsPreHardmode();
        }

        private void SetStatsHardmode()
        {
            MountData.jumpHeight = 12;
            MountData.runSpeed = 10f;
        }

        private void SetStatsPreHardmode()
        {
            MountData.jumpHeight = 5;
            MountData.runSpeed = 7f;
        }
    }
}
