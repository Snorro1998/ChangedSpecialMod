using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex.Black
{
    public class BlackLatexIceWallUnsafe : BaseBlackLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.IceUnsafe;
        }
    }
}
