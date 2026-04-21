using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Clouds
{
    public class PuroCloud : ModCloud
    {
        public override bool RareCloud => false;

        public override float SpawnChance()
        {            
            if (!Main.gameMenu && ChangedUtils.IsDrunk(Main.LocalPlayer))
            {
                return 10f;
            }

            return 0.5f;
        }
        public override void OnSpawn(Cloud cloud)
        {
            cloud.scale = Main.rand.NextFloat(1f, 1.5f);
        }
    }
}
