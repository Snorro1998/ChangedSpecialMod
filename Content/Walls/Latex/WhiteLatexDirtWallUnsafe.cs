using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex
{
    public class WhiteLatexDirtWallUnsafe : BaseWhiteLatexWall
    {
        public override void SetStaticDefaults()
        {
            VanillaFallbackOnModDeletion = WallID.DirtUnsafe;
        }
    }
}
