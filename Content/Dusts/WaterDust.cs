using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ChangedSpecialMod.Content.Dusts
{
    public class WaterDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity = Vector2.Zero;
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale = 1.0f;
        }

        public override bool Update(Dust dust)
        {
            dust.scale += 0.1f;
            if (dust.scale > 5)
                dust.active = false;

            return false;
        }
    }
}
