using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Walls.Latex
{
    public abstract class BaseBlackLatexWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            DustType = DustID.Asphalt;
            AddMapEntry(new Color(5, 5, 5));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void RandomUpdate(int i, int j)
        {
            WorldGenerator.Corrupt(i, j, NPCs.GooType.Black);
        }
    }
}
