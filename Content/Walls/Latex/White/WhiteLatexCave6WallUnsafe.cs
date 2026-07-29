using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Walls.Latex.White
{
    public class WhiteLatexCave6WallUnsafe : BaseWhiteLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.Cave6Unsafe;
            Main.wallBlend[Type] = ModContent.WallType<WhiteLatexDirtWallUnsafe>();
        }
    }
}
