using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex.White
{
    public class WhiteLatexDirtWallUnsafe2 : BaseWhiteLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.DirtUnsafe2;
        }
    }
}
