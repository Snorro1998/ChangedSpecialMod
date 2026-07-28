using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex
{
    public class WhiteLatexGrassWallUnsafe : BaseWhiteLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.GrassUnsafe;
        }
    }
}
