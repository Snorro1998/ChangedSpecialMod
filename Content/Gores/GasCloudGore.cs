using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Gores
{
    public class GasCloudGore : ModGore
    {
        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
            GoreID.Sets.SpecialAI[Type] = 6;
        }

        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            float num = (float)(255 - gore.alpha) / 255f;
            int r;
            int g;
            int b;

            r = 0;// (int)((float)(int)lightColor.R * num);
            g = 0;// (int)((float)(int)lightColor.G * num);
            b = (int)((float)(int)lightColor.B * num);
            int num2 = lightColor.A - gore.alpha;
            if (num2 < 0)
            {
                num2 = 0;
            }
            if (num2 > 255)
            {
                num2 = 255;
            }
            var col = new Color(r, g, b, (num2 < 20) ? num2 : 20);
            return col;
        }
    }
}
