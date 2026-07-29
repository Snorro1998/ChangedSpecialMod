using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Walls.Latex.Black
{
    public class BlackLatexFlowerWallUnsafe : BaseBlackLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.FlowerUnsafe;
            Main.wallBlend[Type] = ModContent.WallType<BlackLatexGrassWallUnsafe>();
        }
    }
}
