using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class ScreenShakeSystem : ModSystem
    {
        public static int shakeTime;
        public static float shakeIntensity;

        public override void ModifyScreenPosition()
        {
            if (shakeTime > 0)
            {
                Main.screenPosition += Main.rand.NextVector2Circular(
                    shakeIntensity,
                    shakeIntensity
                );
                shakeTime--;
            }
        }

        public static void Shake(float time, float intensity)
        {
            shakeTime = (int)(60 * time);
            shakeIntensity = intensity;
        }
    }
}
