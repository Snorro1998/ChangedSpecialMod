using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Walls.Latex.Black
{
    public class BlackLatexCave6WallUnsafe : BaseBlackLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.Cave6Unsafe;
            Main.wallBlend[Type] = ModContent.WallType<BlackLatexDirtWallUnsafe>();
        }
    }
}
