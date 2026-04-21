using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Mounts
{
    public class FlyingDarkLatexMount : ModMount
    {
        public override void SetStaticDefaults()
        {
            // Movement
            MountData.jumpHeight = 4;           // How high the mount can jump.
            MountData.acceleration = 0.12f;     //0.19 The rate at which the mount speeds up.
            MountData.jumpSpeed = 4f;           // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is pressed.
            MountData.blockExtraJumps = false;  // Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
            MountData.constantJump = true;      // Allows you to hold the jump button down.
            MountData.heightBoost = 20;         // Height between the mount and the ground
            MountData.fallDamage = 0f;          // Fall damage multiplier.
            MountData.runSpeed = 6f;            // The speed of the mount
            MountData.dashSpeed = 6f;           // The speed the mount moves when in the state of dashing.
            MountData.flightTimeMax = 60;       // The amount of time in frames a mount can be in the state of flying.

            // Misc
            MountData.fatigueMax = 0;
            MountData.buff = ModContent.BuffType<Buffs.FlyingDarkLatexMountBuff>();

            // Effects
            MountData.spawnDust = DustID.Asphalt;

            // Frame data and player offsets
            MountData.totalFrames = 4;


            int baseYOffset = 22;
            MountData.playerYOffsets = Enumerable.Repeat(baseYOffset, MountData.totalFrames).ToArray();
            MountData.playerYOffsets[0] = baseYOffset;
            MountData.playerYOffsets[1] = baseYOffset + 4;
            MountData.playerYOffsets[2] = baseYOffset + 8;
            MountData.playerYOffsets[3] = baseYOffset + 4;

            MountData.xOffset = 7;
            //MountData.yOffset = -12;
            MountData.playerHeadOffset = 22;
            MountData.bodyFrame = 3;

            // Standing
            MountData.standingFrameCount = 4;
            MountData.standingFrameDelay = 8;
            MountData.standingFrameStart = 0;
            
            // Running
            MountData.runningFrameCount = 4;
            MountData.runningFrameDelay = 48;
            MountData.runningFrameStart = 0;
            
            // Flying
            MountData.flyingFrameCount = 4;
            MountData.flyingFrameDelay = 8;
            MountData.flyingFrameStart = 0;
            
            // In-air
            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 8;
            MountData.inAirFrameStart = 0;
            
            // Idle
            MountData.idleFrameCount = 4;
            MountData.idleFrameDelay = 8;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;
            
            // Swimming
            MountData.swimFrameCount = 4;
            MountData.swimFrameDelay = 48;
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
            MountData.jumpHeight = 4;           // How high the mount can jump.
            MountData.acceleration = 0.19f;     // The rate at which the mount speeds up.
            MountData.jumpSpeed = 4f;
            MountData.runSpeed = 8f;            // The speed of the mount
            MountData.dashSpeed = 8f;           // The speed the mount moves when in the state of dashing.
            MountData.flightTimeMax = 150;      // The amount of time in frames a mount can be in the state of flying.
        }

        private void SetStatsPreHardmode()
        {
            MountData.jumpHeight = 4;           // How high the mount can jump.
            MountData.acceleration = 0.19f;     // The rate at which the mount speeds up.
            MountData.jumpSpeed = 4f;
            MountData.runSpeed = 6f;            // The speed of the mount
            MountData.dashSpeed = 6f;           // The speed the mount moves when in the state of dashing.
            MountData.flightTimeMax = 60;       // The amount of time in frames a mount can be in the state of flying.
        }
    }
}
