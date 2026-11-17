using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Dusts
{
    public class FireworksLingerDust : ModDust
    {
        public override void SetStaticDefaults()
        {
            UpdateType = DustID.Firework_Red;
        }
    }
}
