using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex.Black
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
