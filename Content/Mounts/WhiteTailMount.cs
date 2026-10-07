using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Mounts
{
    public class WhiteTailMount : ModMount
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
            MountData.runSpeed = 8f;            // The speed of the mount
            MountData.dashSpeed = 20f;          // The speed the mount moves when in the state of dashing.
            MountData.flightTimeMax = 0;        // The amount of time in frames a mount can be in the state of flying.

            // Misc
            MountData.fatigueMax = 0;
            MountData.buff = ModContent.BuffType<Buffs.WhiteTailMountBuff>();

            // Effects
            MountData.spawnDust = DustID.SnowSpray;

            // Frame data and player offsets
            MountData.totalFrames = 4;

            int baseYOffset = 22;
            MountData.playerYOffsets = Enumerable.Repeat(baseYOffset, MountData.totalFrames).ToArray();
            /*
            MountData.playerYOffsets[8] = baseYOffset + 2;
            MountData.playerYOffsets[4] = baseYOffset + 3;
            MountData.playerYOffsets[5] = baseYOffset + 4;
            MountData.playerYOffsets[7] = baseYOffset + 4;
            MountData.playerYOffsets[6] = baseYOffset + 5;
            */

            MountData.xOffset = -2;
            //MountData.yOffset = -12;

            MountData.playerHeadOffset = 22;
            MountData.bodyFrame = 3;
            
            // Standing
            MountData.standingFrameCount = 1;
            MountData.standingFrameDelay = 12;
            MountData.standingFrameStart = 0;
            
            // Running
            MountData.runningFrameCount = 4;
            MountData.runningFrameDelay = 12;
            MountData.runningFrameStart = 0;
            
            // Flying
            MountData.flyingFrameCount = 1;
            MountData.flyingFrameDelay = 12;
            MountData.flyingFrameStart = 0;
            
            // In-air
            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 12;
            MountData.inAirFrameStart = 0;
            
            // Idle
            MountData.idleFrameCount = 1;
            MountData.idleFrameDelay = 12;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;
            
            // Swimming
            MountData.swimFrameCount = 4;
            MountData.swimFrameDelay = 12;
            MountData.swimFrameStart = 0;

            if (Main.netMode != NetmodeID.Server)
            {
                MountData.textureWidth = MountData.backTexture.Width();
                MountData.textureHeight = MountData.backTexture.Height();
            }
        }

        public override void UpdateEffects(Player player)
        {
            int dashDir = 0;
            if (player.controlRight && player.releaseRight && player.doubleTapCardinalTimer[2] < 15 && player.doubleTapCardinalTimer[3] == 0)
            {
                dashDir = 1;
            }
            else if (player.controlLeft && player.releaseLeft && player.doubleTapCardinalTimer[3] < 15 && player.doubleTapCardinalTimer[2] == 0)
            {
                dashDir = -1;
            }

            switch (dashDir)
            {
                // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                case -1 when player.velocity.X > -MountData.dashSpeed:
                case 1 when player.velocity.X < MountData.dashSpeed:
                    player.velocity.X = dashDir * MountData.dashSpeed;
                    break;
            }

            float deaccel = 0.3f;
            if (player.velocity.X > MountData.runSpeed)
                player.velocity.X = Math.Max(player.velocity.X - deaccel, MountData.runSpeed);
            else if (player.velocity.X < -MountData.runSpeed)
                player.velocity.X = Math.Min(player.velocity.X + deaccel, -MountData.runSpeed);
        }
    }
}
