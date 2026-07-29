using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex.White
{
    public class WhiteLatexSnowWallUnsafe : BaseWhiteLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.SnowWallUnsafe;
        }
    }
}
