using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Walls.Latex.White
{
    public class WhiteLatexFlowerWallUnsafe : BaseWhiteLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.FlowerUnsafe;
            Main.wallBlend[Type] = ModContent.WallType<WhiteLatexGrassWallUnsafe>();
        }
    }
}
