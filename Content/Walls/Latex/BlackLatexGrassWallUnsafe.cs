using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex
{
    public class BlackLatexGrassWallUnsafe : BaseBlackLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.GrassUnsafe;
        }
    }
}
