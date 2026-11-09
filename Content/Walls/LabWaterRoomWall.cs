using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Walls
{
    public class LabWaterRoomWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            DustType = DustID.Asphalt;
            AddMapEntry(new Color(5, 5, 5));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
